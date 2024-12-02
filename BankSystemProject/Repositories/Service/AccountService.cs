using BankSystemProject.Controllers.AdminControllers;
using BankSystemProject.Data;
using BankSystemProject.Helpers;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using BankSystemProject.Shared.Enums;
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

            // Step 1: Fetch customerAccount and related customerAccount
            var customerAccount = await _context.CustomersAccounts
                .Include(ca => ca.User) // Include related customerAccount entity
                .FirstOrDefaultAsync(ca => ca.AccountNumber == loginDto.AccountNumber);

            // Step 2: Validate if account and user exist
            if (customerAccount == null || customerAccount.User == null)
            {
                return null; // Account or user not found
            }

            var user = customerAccount.User;

            // Step 3: Validate password
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return null; // Invalid password
            }

            // Step 4: Return a response (example includes JWT generation)
            return new Users
            {
                
                FullName = user.FullName,
               //AccountNumber = customerAccount.AccountNumber,
                // Token =  //enerate a JWT token for authentication
            };
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

            var result = await _userManager.CreateAsync(userInfo, registerDto.Password);
            if (!result.Succeeded)
            {
                return new Res_Registration { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
            }

            // Assign role to user
            var roleResult = await _userManager.AddToRoleAsync(userInfo, registerDto.UserRole.ToString());
           
            if (!roleResult.Succeeded)
            {
                return new Res_Registration { Message = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
            }

            if (registerDto.UserRole == enUserRole.Customer)
            {
                var userAccountInfo = new CustomerAccount
                {
                    UserId = userInfo.Id,
                    AccountNumber = hashedAccountNumber,
                    AccountTypeId = (int)registerDto.accountType,
                    CreatedDate = DateTime.Now
                };
                _context.CustomersAccounts.Add(userAccountInfo);
                await _context.SaveChangesAsync();
            }


            if (registerDto.UserRole != enUserRole.Customer)
            {
                var newEmployee = new Employee
                {
                    UserId = userInfo.Id,
                    HireDate = DateTime.Now,
                    EmployeeSalary = registerDto.Salary,
                    BranchID=(int)registerDto.BranchName,
                };
                _context.Employee.Add(newEmployee);
                await _context.SaveChangesAsync();
            }

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
