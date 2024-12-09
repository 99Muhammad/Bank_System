using BankSystemProject.Controllers.AdminControllers;
using BankSystemProject.Data;
using BankSystemProject.Helpers;
using BankSystemProject.Model;
using BankSystemProject.Models;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using BankSystemProject.Shared.Enums;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
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

        public AccountService(Bank_DbContext _context, UserManager<Users> _userManager
            , SignInManager<Users> _signInManager, ILogger<AccountController> logger
            ,JWTService _jwtService)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            _logger = logger;
            this._jwtService = _jwtService;

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


        public async Task<Res_Registration> RegisterUserAsync(Req_Registration registerDto)
        {
            // Validate the user details
            var validationResult = await ValidateUserAsync(registerDto);
            if (!validationResult.Success)
                return validationResult;

            // Create the user
            var userInfo = CreateUserInfo(registerDto);
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
            else
            {
                await HandleEmployeeAsync(userInfo.Id, registerDto);
            }

            // Generate Refresh Token for the new user
            var refreshToken = _jwtService.GenerateRefreshToken();
            await SaveRefreshTokenAsync(userInfo.Id, refreshToken);

            return new Res_Registration
            {
                UserName = registerDto.UserName,
                AccountNmber = registerDto.UserRole == enUserRole.Customer ? accountCreationResult.AccountNmber : null,
                Success = true
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
        private Users CreateUserInfo(Req_Registration registerDto)
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
                PersonalImage = "default.png",
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

    }
}
