using BankSystemProject.Controllers;
using BankSystemProject.Data;
using BankSystemProject.Helpers;
using BankSystemProject.Model;
using BankSystemProject.Models;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using BankSystemProject.Shared.Enums;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace BankSystemProject.Repositories.Service
{
    public class AccountService : IAccount
    {

        private readonly Bank_DbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly JWTService _jwtService;
        private readonly IEmail _Email;
        private readonly IConfiguration configuration;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IUrlHelper _urlHelper;
        private readonly string? _appUrl;


        public AccountService(Bank_DbContext _context, UserManager<Users> _userManager
            , SignInManager<Users> _signInManager
            , JWTService _jwtService, IEmail _Email
            , IConfiguration _configuration, IUrlHelperFactory _urlHelperFactory
            , IHttpContextAccessor httpContextAccessor)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._jwtService = _jwtService;
            this._Email = _Email;
            configuration = _configuration;
            this._urlHelperFactory = _urlHelperFactory;
            _appUrl = _configuration["App:Url"];

            _urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(
        httpContextAccessor.HttpContext,
        httpContextAccessor.HttpContext.GetRouteData(),
        new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()));

        }
        public async Task<Res_TokenDto> LoginAsync(Req_Login loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return null;
            }

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
            {
                
                throw new InvalidOperationException("Your email is not confirmed yet. Please confirm your email to proceed.");
            }

                var trackingEntry = new TrackingLoggedInUser
                {
                    LoginTime = DateTime.UtcNow,
                    UserID = user.Id, 
                   
                };
                _context.TrackingLoggedInUsers.Add(trackingEntry);
                await _context.SaveChangesAsync();


            var accessToken = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            await SaveRefreshTokenAsync(user.Id, refreshToken);

            return new Res_TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        public async Task<Res_Registration> RegisterUser(Req_Registration registerDto)
        {
            string bankName = "NovaBank";
            string savedFileName = null;

            
            if (registerDto.ImageUrl != null)
            {
                savedFileName = await SaveImageAsync(registerDto.ImageUrl);
            }

            var userInfo = CreateUserInfo(registerDto, savedFileName);
            var result = await _userManager.CreateAsync(userInfo, registerDto.Password);
           
            if (!result.Succeeded)
            {
                return new Res_Registration { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
            }

            var roleResult = await _userManager.AddToRoleAsync(userInfo, registerDto.UserRole.ToString());
            if (!roleResult.Succeeded)
            {
                return new Res_Registration { Message = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
            }

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(userInfo);
            
            var confirmationUrl = _urlHelper.Action(
                action: "confirmemail",
                controller: "Account",
                values: new { userId = userInfo.Id, token = confirmationToken },
                protocol: "https"
                
            );

            string emailHtmlContent = EmailContentForEmployees(bankName,registerDto.UserName, confirmationUrl);

            if (registerDto.UserRole == enUserRole.Customer)
            {
               
                var accountCreationResult = await HandleCustomerAccountAsync(userInfo.Id, registerDto);
                if (!accountCreationResult.Success)
                    return accountCreationResult;

                string accountNumber = accountCreationResult.AccountNmber;

                emailHtmlContent = EmailContentForCustomers(bankName, registerDto.UserName, confirmationUrl, accountNumber);
            }
            else
            {
                var employeeCreationResult = await HandleEmployeeAsync(userInfo.Id, registerDto);
                
                if (!employeeCreationResult.Success)
                    return employeeCreationResult;

               

                emailHtmlContent = EmailContentForEmployees(bankName, registerDto.UserName, confirmationUrl);
            }

            // Send confirmation email
            await _Email.SendEmailAsync(registerDto.Email,"NovaBank",$"{bankName} - Confirm Your Email", emailHtmlContent);

            var accessToken = _jwtService.GenerateJwtToken(userInfo);

            return new Res_Registration
            {
                UserName = registerDto.UserName,
                Success = true,
                //AccessToken = accessToken,
            };
        }
        private async Task SaveRefreshTokenAsync(string userId, string refreshToken)
        {
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId);
            if (existingToken != null)
            {
                existingToken.Token = refreshToken;
                existingToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
            }
            else
            {
                var newToken = new RefreshToken
                {
                    UserId = userId,
                    Token = refreshToken,
                    ExpiryDate = DateTime.UtcNow.AddDays(7)
                };
                _context.RefreshTokens.Add(newToken);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<Res_TokenDto> RefreshTokensAsync(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
           
            if (storedToken == null || storedToken.ExpiryDate <= DateTime.UtcNow)
            {
                return null; 
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user == null)
            {
                return null;
            }

            // Generate new tokens
            var newAccessToken = _jwtService.GenerateJwtToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // Update the refresh token in the database
            storedToken.Token = newRefreshToken;
            storedToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new Res_TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiry= storedToken.ExpiryDate,

            };
        }
        private async Task<Res_Registration> ValidateUserAsync(Req_Registration registerDto)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUserByEmail != null)
            {
                return new Res_Registration { Message = "A user with this email already exists.", Success = false };
            }

            var existingUserByName = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUserByName != null)
            {
                return new Res_Registration { Message = "A user with this username already exists.", Success = false };
            }

            return new Res_Registration { Success = true };
        }
        private Users CreateUserInfo(Req_Registration registerDto,string savedFile)
        {
            return new Users
            {
                FullName = string.Join(" ", registerDto.FirstName, registerDto.SecondName, registerDto.ThirdName, registerDto.LastName).Trim(),
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                Gender = registerDto.Gender.ToString(),
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Role = registerDto.UserRole.ToString(),
                DateOfBirth = registerDto.DateOfBirth,
                PersonalImage = savedFile,
                LockoutEnd = DateTime.Now,
            };
        }
        private async Task<Res_Registration> HandleCustomerAccountAsync(string userId, Req_Registration registerDto)
        {
            var existingCustomerAccount = await _context.CustomersAccounts
                .FirstOrDefaultAsync(ca => ca.UserId == userId && ca.AccountTypeId == (int)registerDto.accountType);

            if (existingCustomerAccount != null)
            {
                return new Res_Registration
                {
                    Message = "A customer account with the same type already exists.",
                    Success = false
                };
            }

            string accountNumber = await GenerateUniqueAccountNumAsync();
            string encodedAccountNumber = Base64Helper.Encode(accountNumber);

            var userAccountInfo = new CustomerAccount
            {
                UserId = userId,
                AccountNumber = encodedAccountNumber,
                AccountTypeId = (int)registerDto.accountType,
                CreatedDate = DateTime.Now
            };

            _context.CustomersAccounts.Add(userAccountInfo);
            await _context.SaveChangesAsync();

            return new Res_Registration
            {
                Success = true,
                AccountNmber = accountNumber
            };
        }
        private async Task<Res_Registration> HandleEmployeeAsync(string userId, Req_Registration registerDto)
        {
            var existingEmployee = await _context.Employee
               .FirstOrDefaultAsync(ca => ca.UserId == userId);

            if(existingEmployee!=null)
            {
                return new Res_Registration
                {
                    Message = "An Employee already exists.",
                    Success = false
                };
            }
            var newEmployee = new Employee
            {
                UserId = userId,
                HireDate = DateTime.Now,
                EmployeeSalary = registerDto.Salary,
                BranchID = (int)registerDto.BranchName,
            };
            _context.Employee.Add(newEmployee);
            await _context.SaveChangesAsync();
           
            return new Res_Registration
            {
                Success = true,
            };
        }
       
        private async Task<string> GenerateUniqueAccountNumAsync()
        {
            Random random = new Random();
            string AccountNum = "";
            bool isUnique = false;

            while (!isUnique)
            {

                 AccountNum = random.Next(1000, 10000).ToString();
                var existingUser = await _context.CustomersAccounts
                    .FirstOrDefaultAsync(u => u.AccountNumber == AccountNum);

                if (existingUser == null)
                {
                    isUnique = true;
                }
            }
            return AccountNum;
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Invalid file");

            // Define the upload folder path relative to the project directory
            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

           
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadFolder, fileName);

          
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            return fileName;
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;
        }
        public async Task Logout(ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user != null)
            {
                //user.RefreshTokenExpireTime = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordReqDTO forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            //var baseUrl = "http://localhost:5085/api/Account";
            var resetLink = $"{_appUrl}/api/Account/reset-password?email={user.Email}&token={code}";
            var subject = "Reset Password";
            var emailBody = $@"
            <p>To reset your password, please click the following link:</p>
            <p><a href='{resetLink}'>Reset Password</a></p>";

            await _Email.SendEmailAsync(user.Email, subject, emailBody,emailBody);

            return true;
        }
        private string EmailContentForEmployees(string bankName,string UserName,string confirmationUrl)
        {
         string content=   $@"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
            color: #333;
            line-height: 1.6;
        }}
        .email-container {{
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            background-color: #ffffff;
            border: 1px solid #dddddd;
            border-radius: 10px;
        }}
        .email-header {{
            text-align: center;
            margin-bottom: 20px;
        }}
        .email-header h1 {{
            color: #0073e6;
        }}
        .email-footer {{
            margin-top: 30px;
            text-align: center;
            font-size: 0.9em;
            color: #555;
        }}
        .btn {{
            display: inline-block;
            background-color: #0073e6;
            color: #ffffff;
            padding: 10px 20px;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 20px;
        }}
        .btn:hover {{
            background-color: #005bb5;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='email-header'>
            <h1>Welcome to {bankName}</h1>
        </div>
        <p>Dear {UserName},</p>
        <p>Thank you for choosing <strong>{bankName}</strong>. We are thrilled to have you onboard!</p>
        <p>Please confirm your account by clicking the button below:</p>
        <a class='btn' href='{confirmationUrl}'>Confirm Your Account</a>
    </div>
</body>
</html>";

            return content;
        }

        private string EmailContentForCustomers(string bankName,string UserName,string confirmationUrl,string accountNumber)
        {
           string content= $@"
<html>
<head>
    <style>
        /* Same styles as above */
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='email-header'>
            <h1>Welcome to {bankName}</h1>
        </div>
        <p>Dear {UserName},</p>
        <p>Thank you for choosing <strong>{bankName}</strong>. We are thrilled to have you onboard!</p>
        <p>Your account number is: <strong>{accountNumber}</strong></p>
        <p>Please confirm your account by clicking the button below:</p>
        <a class='btn' href='{confirmationUrl}'>Confirm Your Account</a>
    </div>
</body>
</html>";
            return content;
        }
    }
}
