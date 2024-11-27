using BankSystemProject.Data;
using BankSystemProject.Helper;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;

namespace BankSystemProject.Repositories.Service
{
    public class AccountService : IAccount
    {

        private readonly Bank_DbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public AccountService(Bank_DbContext _context, UserManager<Users> _userManager
            , SignInManager<Users> _signInManager)
        {
            this._context = _context;   
            this._userManager = _userManager;
            this._signInManager = _signInManager;
        }

        public async Task<string> LoginAsync(Req_Login loginDto)
        {
          
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return "Invalid credentials.";
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

            if (signInResult.Succeeded)
            {
               
                return "Login successful!";
            }
            return "";
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
                DateOfBirth=registerDto.DateOfBirth,
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
                CreatedDate =DateTime.Now
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
