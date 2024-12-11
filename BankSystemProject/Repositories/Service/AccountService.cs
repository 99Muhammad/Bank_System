using BankSystemProject.Controllers.AdminControllers;
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
        private readonly ILogger<AccountController> _logger;
        private readonly JWTService _jwtService;
        private readonly IEmail _Email;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration configuration;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IUrlHelper _urlHelper;
        private readonly string? _appUrl;


        public AccountService(Bank_DbContext _context, UserManager<Users> _userManager
            , SignInManager<Users> _signInManager, ILogger<AccountController> logger
            , JWTService _jwtService, IEmail _Email, IWebHostEnvironment _env
            , IConfiguration _configuration, IUrlHelperFactory _urlHelperFactory
            , IHttpContextAccessor httpContextAccessor)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            _logger = logger;
            this._jwtService = _jwtService;
            this._Email = _Email;
            this._env = _env;
            configuration = _configuration;
            this._urlHelperFactory = _urlHelperFactory;
            _appUrl = _configuration["App:Url"];

            _urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(
        httpContextAccessor.HttpContext,
        httpContextAccessor.HttpContext.GetRouteData(),
        new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()));

        }
        //public async Task<string> LoginAsync(Req_Login loginDto)
        //{
        //    var user = await _userManager.FindByNameAsync(loginDto.UserName); 
        //    if (user == null)
        //    {
        //        return null; 
        //    }
        //    if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
        //    {
        //        return null; 
        //    }

        //    var jwtToken = _jwtService.GenerateJwtToken(user); 

        //    return jwtToken;
        //}

        public async Task<Res_TokenDto> LoginAsync(Req_Login loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return null;
            }

            // Generate Access and Refresh Tokens
            var accessToken = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Save the Refresh Token to the database
            await SaveRefreshTokenAsync(user.Id, refreshToken);

            return new Res_TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }


        //public async Task<Res_Registration> RegisterUserAsync(Req_Registration registerDto)
        //{
        //    // Validate the user details
        //    var validationResult = await ValidateUserAsync(registerDto);
        //    if (!validationResult.Success)
        //        return validationResult;

        //    // Create the user
        //    var userInfo = CreateUserInfo(registerDto);
        //    var result = await _userManager.CreateAsync(userInfo, registerDto.Password);
        //    if (!result.Succeeded)
        //    {
        //        return new Res_Registration { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
        //    }

        //    // Assign role
        //    var roleResult = await _userManager.AddToRoleAsync(userInfo, registerDto.UserRole.ToString());
        //    if (!roleResult.Succeeded)
        //    {
        //        return new Res_Registration { Message = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
        //    }

        //    // Handle role-specific logic
        //    Res_Registration accountCreationResult = new Res_Registration();
        //    if (registerDto.UserRole == enUserRole.Customer)
        //    {
        //        accountCreationResult = await HandleCustomerAccountAsync(userInfo.Id, registerDto);
        //        if (!accountCreationResult.Success)
        //            return accountCreationResult;
        //    }
        //    else
        //    {
        //        await HandleEmployeeAsync(userInfo.Id, registerDto);
        //    }

        //    // Generate Refresh Token for the new user
        //    var refreshToken = _jwtService.GenerateRefreshToken();
        //    await SaveRefreshTokenAsync(userInfo.Id, refreshToken);

        //    return new Res_Registration
        //    {
        //        UserName = registerDto.UserName,
        //        AccountNmber = registerDto.UserRole == enUserRole.Customer ? accountCreationResult.AccountNmber : null,
        //        Success = true
        //    };
        //}


        //public async Task<Res_Registration> RegisterUserAsync(Req_Registration registerDto)
        //{
        //    string savedFileName = null;

        //    if (registerDto.ImageUrl != null)
        //    {
        //        savedFileName = await SaveImageAsync(registerDto.ImageUrl);
        //    }
        //    // Validate the user details
        //    var validationResult = await ValidateUserAsync(registerDto);
        //    if (!validationResult.Success)
        //        return validationResult;

        //    // Create the user
        //    var userInfo = CreateUserInfo(registerDto,savedFileName);
        //    var result = await _userManager.CreateAsync(userInfo, registerDto.Password);
        //    if (!result.Succeeded)
        //    {
        //        return new Res_Registration { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
        //    }

        //    // Assign role
        //    var roleResult = await _userManager.AddToRoleAsync(userInfo, registerDto.UserRole.ToString());
        //    if (!roleResult.Succeeded)
        //    {
        //        return new Res_Registration { Message = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
        //    }

        //    // Handle role-specific logic
        //    Res_Registration accountCreationResult = new Res_Registration();
        //    if (registerDto.UserRole == enUserRole.Customer)
        //    {
        //        // Handle customer-specific logic
        //        accountCreationResult = await HandleCustomerAccountAsync(userInfo.Id, registerDto);
        //        if (!accountCreationResult.Success)
        //            return accountCreationResult;

        //        // Send email with account number for customers
        //        try
        //        {
        //            await _Email.SendRegistrationEmailAsync(
        //                registerDto.Email,
        //                registerDto.UserName,
        //                accountCreationResult.AccountNmber // Pass the account number
        //            );
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Failed to send email to customer: {ex.Message}");
        //        }
        //    }
        //    else
        //    {
        //        // Handle employee-specific logic
        //        await HandleEmployeeAsync(userInfo.Id, registerDto);

        //        // Send a generic welcome email for non-customers
        //        try
        //        {
        //            await _Email.SendRegistrationEmailAsync(
        //                registerDto.Email,
        //                registerDto.UserName 
        //            );
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Failed to send welcome email: {ex.Message}");
        //        }
        //    }

        //    // Generate Refresh Token for the new user
        //    var refreshToken = _jwtService.GenerateRefreshToken();
        //    await SaveRefreshTokenAsync(userInfo.Id, refreshToken);

        //    return new Res_Registration
        //    {
        //        UserName = registerDto.UserName,
        //        //AccountNmber = registerDto.UserRole == enUserRole.Customer ? accountCreationResult.AccountNmber : null,
        //        Success = true
        //    };
        //}


        //public async Task<Res_Registration> RegisterUserAsync(Req_Registration registerDto)
        //{
        //    string savedFileName = null;

        //    if (registerDto.ImageUrl != null)
        //    {
        //        savedFileName = await SaveImageAsync(registerDto.ImageUrl);
        //    }

        //    // Validate the user details
        //    var validationResult = await ValidateUserAsync(registerDto);
        //    if (!validationResult.Success)
        //        return validationResult;

        //    // Create the user
        //    var userInfo = CreateUserInfo(registerDto, savedFileName);
        //    var result = await _userManager.CreateAsync(userInfo, registerDto.Password);
        //    if (!result.Succeeded)
        //    {
        //        return new Res_Registration { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
        //    }

        //    // Assign role
        //    var roleResult = await _userManager.AddToRoleAsync(userInfo, registerDto.UserRole.ToString());
        //    if (!roleResult.Succeeded)
        //    {
        //        return new Res_Registration { Message = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
        //    }
        //    Res_Registration accountCreationResult = new Res_Registration();
        //    if (registerDto.UserRole == enUserRole.Customer)
        //    {
        //        accountCreationResult = await HandleCustomerAccountAsync(userInfo.Id, registerDto);
        //        if (!accountCreationResult.Success)
        //            return accountCreationResult;
        //    }
        //    // Generate Email Confirmation Token
        //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(userInfo);
        //    var confirmationLink = $"{configuration["FrontendUrl"]}/confirm-email?email={userInfo.Email}&token={Uri.EscapeDataString(token)}";

        //    // Send email with confirmation link
        //    try
        //    {
        //        await _Email.SendRegistrationEmailAsync(
        //            registerDto.Email,
        //            registerDto.UserName,
        //            registerDto.UserRole == enUserRole.Customer ? accountCreationResult.AccountNmber : null,
        //            confirmationLink
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Failed to send confirmation email: {ex.Message}");
        //    }

        //    return new Res_Registration
        //    {
        //        UserName = registerDto.UserName,
        //        Success = true
        //    };
        //}

        /*public async Task<Res_Registration> RegisterUserAsync(Req_Registration registerDto)
        {
            string savedFileName = null;

            // Save image if provided
            if (registerDto.ImageUrl != null)
            {
                savedFileName = await SaveImageAsync(registerDto.ImageUrl);
            }

            // Validate the user details
            var validationResult = await ValidateUserAsync(registerDto);
            if (!validationResult.Success)
                return validationResult;

            // Create the user
            var userInfo = CreateUserInfo(registerDto, savedFileName);
            var result = await _userManager.CreateAsync(userInfo, registerDto.Password);
            if (!result.Succeeded)
            {
                return new Res_Registration { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
            }

            // Assign role
            var roleResult = await _userManager.AddToRoleAsync(userInfo, registerDto.UserRole.ToString());
            if (!roleResult.Succeeded)
            {
                return new Res_Registration { Message = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
            }

            // Handle role-specific logic
            Res_Registration accountCreationResult = new Res_Registration();
            if (registerDto.UserRole == enUserRole.Customer)
            {
                accountCreationResult = await HandleCustomerAccountAsync(userInfo.Id, registerDto);
                if (!accountCreationResult.Success)
                    return accountCreationResult;
            }

           
          


          
            // Generate JWT Access Token
            var accessToken = _jwtService.GenerateJwtToken(userInfo);

            // Return successful registration response with token
            return new Res_Registration
            {
                UserName = registerDto.UserName,
                Success = true,
                AccessToken = accessToken,// Include access token in response
               // confirmLink = confirmationLink,
            };
        }*/


        public async Task<Res_Registration> RegisterUser(Req_Registration registerDto)
        {
            string savedFileName = null;

            // Save image if provided
            if (registerDto.ImageUrl != null)
            {
                savedFileName = await SaveImageAsync(registerDto.ImageUrl);
            }

            // Validate the user details
            // var validationResult = await ValidateUser (registerDto);
            // if (!validationResult.Success)
            //     return validationResult;

            // Create the user
            var userInfo = CreateUserInfo(registerDto, savedFileName);
            var result = await _userManager.CreateAsync(userInfo, registerDto.Password);
            if (!result.Succeeded)
            {
                return new Res_Registration { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
            }

            // Assign role
            var roleResult = await _userManager.AddToRoleAsync(userInfo, registerDto.UserRole.ToString());
            if (!roleResult.Succeeded)
            {
                return new Res_Registration { Message = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
            }

            // Generate confirmation token
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(userInfo);
            //var confirmationLink = $"{configuration["AppUrl"]}/api/confirm-email?userId={userInfo.Id}&token={Uri.EscapeDataString(confirmationToken)}";
            var confirmationUrl = _urlHelper.Action(
            action: "ConfirmEmail",
            controller: "Account",
            values: new { userId = userInfo.Id, token = confirmationToken },
            protocol: "https"
        );

            // Create HTML content for the email
            string emailHtmlContent = $@"
        <html>
        <body>
            <p>Please confirm your account by clicking this link:</p>
            <p><a href='{confirmationUrl}'>Confirm your account</a></p>
        </body>
        </html>";

            // Send confirmation email
            await _Email.SendEmailAsync(registerDto.Email, "Confirm your email", emailHtmlContent, emailHtmlContent);

            // Handle role-specific logic
            Res_Registration accountCreationResult = new Res_Registration();
            if (registerDto.UserRole == enUserRole.Customer)
    {
                accountCreationResult = await HandleCustomerAccountAsync(userInfo.Id, registerDto);
                if (!accountCreationResult.Success)
                    return accountCreationResult;
            }

            // Generate JWT Access Token
            var accessToken = _jwtService.GenerateJwtToken(userInfo);

            // Return successful registration response with token
            return new Res_Registration
            {
                UserName = registerDto.UserName,
                Success = true,
                AccessToken = accessToken,
                confirmLink=confirmationToken           // Include access token in response
                                           // confirmLink = confirmationLink, // Optional: you can include the confirmation link in the response
            };
        }

        private async Task SaveRefreshTokenAsync(string userId, string refreshToken)
        {
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId);
            if (existingToken != null)
            {
                existingToken.Token = refreshToken;
                existingToken.ExpiryDate = DateTime.UtcNow.AddDays(7); // Set expiry for 7 days
            }
            else
            {
                var newToken = new RefreshToken
                {
                    UserId = userId,
                    Token = refreshToken,
                    ExpiryDate = DateTime.UtcNow.AddDays(7) // Set expiry for 7 days
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
                return null; // Invalid or expired token
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user == null)
            {
                return null; // User not found
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
        private async Task HandleEmployeeAsync(string userId, Req_Registration registerDto)
        {
            var newEmployee = new Employee
            {
                UserId = userId,
                HireDate = DateTime.Now,
                EmployeeSalary = registerDto.Salary,
                BranchID = (int)registerDto.BranchName,
            };
            _context.Employee.Add(newEmployee);
            await _context.SaveChangesAsync();
        }
        //public async Task<Res_Registration> RegisterUserAsync(Req_Registration registerDto)
        //{
        //    // Validate the user details
        //    var validationResult = await ValidateUserAsync(registerDto);
        //    if (!validationResult.Success)
        //        return validationResult;

        //    // SubmitLoanApplicationAsync the user
        //    var userInfo = CreateUserInfo(registerDto);

        //    var result = await _userManager.CreateAsync(userInfo, registerDto.Password);
        //    if (!result.Succeeded)
        //    {
        //        return new Res_Registration { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
        //    }

        //    // Assign role
        //    var roleResult = await _userManager.AddToRoleAsync(userInfo, registerDto.UserRole.ToString());
        //    if (!roleResult.Succeeded)
        //    {
        //        return new Res_Registration { Message = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
        //    }
        //    var accountCreationResult=new Res_Registration() ;
        //    // Handle role-specific logic
        //    if (registerDto.UserRole == enUserRole.Customer)
        //    {
        //         accountCreationResult = await HandleCustomerAccountAsync(userInfo.Id, registerDto);
        //        if (!accountCreationResult.Success)
        //            return accountCreationResult;
        //    }
        //    else
        //    {
        //        await HandleEmployeeAsync(userInfo.Id, registerDto);
        //    }

        //    return new Res_Registration
        //    {
        //        UserName = registerDto.UserName,
        //        AccountNmber = registerDto.UserRole == enUserRole.Customer ? accountCreationResult.AccountNmber : null,
        //        Success = true
        //    };
        //}
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

            // Generate unique file name
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadFolder, fileName);

            // Save the file
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

        //public async Task<bool> ResetPasswordAsync(ResetPasswordReqDTO resetPasswordDto)
        //{
        //    var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    //var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordDto.Token));

        //    var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
        //    return result.Succeeded;
        //}

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
    }
}
