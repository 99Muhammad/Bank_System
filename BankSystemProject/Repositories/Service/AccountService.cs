using BankSystemProject.Controllers.AdminControllers;
using BankSystemProject.Data;
using BankSystemProject.Helpers;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;

namespace BankSystemProject.Repositories.Service
{
    public class AccountService : IAccount
    {

        private readonly Bank_DbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountService(Bank_DbContext _context, UserManager<Users> _userManager
            , SignInManager<Users> _signInManager, ILogger<AccountController> logger)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            _logger = logger;

        }

        public async Task<Users> LoginAsync(Req_Login loginDto)
        {
            // _logger.LogInformation("Attempting to log in with email: {Email}", loginDto.Email);

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return null; // User not found
            }

            // Check if the user's email or password hash is null (just in case)
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
            {
                return null; // Handle the case where the user record is incomplete
            }

            // Now check the password
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return null; // Invalid password
            }

            return user; // User successfully authenticated
        }


        public async Task<Res_Registration> RegisterUserAsync(Req_Registration registerDto)
        {
            // Check for existing user by email and username
            var existingUserByEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUserByEmail != null)
            {
                return new Res_Registration { Message = "A user with this email already exists." };
            }

            var existingUserByName = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUserByName != null)
            {
                return new Res_Registration { Message = "A user with this username already exists." };
            }

            // Generate account number and create user object
            string accountNumber = await GenerateUniqueAccountNumAsync();
            string hashedAccountNumber = HashingHelper.HashString(accountNumber);
            var userInfo = new Users
            {
                FullName = string.Join(" ", registerDto.FirstName, registerDto.SecondName, registerDto.ThirdName, registerDto.LastName).Trim(),
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                Gender = registerDto.Gender.ToString(),
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Role = registerDto.UserRole.ToString(),
                DateOfBirth = registerDto.DateOfBirth,
                PersonalImage = "jj.png",
                LockoutEnd = DateTime.Now,
            };

            // Create user
            var result = await _userManager.CreateAsync(userInfo, registerDto.Password);
            if (!result.Succeeded)
            {
                return new Res_Registration { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
            }

            // Ensure role exists
            //await EnsureRoleExistsAsync(registerDto.enUserRole.ToString());

            // Assign role to user
            var roleResult = await _userManager.AddToRoleAsync(userInfo, registerDto.UserRole.ToString());
            if (!roleResult.Succeeded)
            {
                return new Res_Registration { Message = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
            }



            // Create user account
            var userAccountInfo = new CustomerAccount
            {
                UserId = userInfo.Id,
                Balance = 0, // Default balance
                AccountNumber = hashedAccountNumber,
                PinCode = HashingHelper.HashString(registerDto.PinCode),
                AccountTypeId = (int)registerDto.accountType,
                CreatedDate = DateTime.Now
            };

            _context.CustomersAccounts.Add(userAccountInfo);
            await _context.SaveChangesAsync();

            return new Res_Registration
            {
                UserName = registerDto.UserName,
                AccountNmber = accountNumber,
                Success = true
            };
        }

        private async Task<string> GenerateUniqueAccountNumAsync()
        {
            Random random = new Random();
            string AccountNum = "";
            bool isUnique = false;

            while (!isUnique)
            {

                AccountNum = random.NextInt64(1000000000, 9999999999L).ToString();
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
