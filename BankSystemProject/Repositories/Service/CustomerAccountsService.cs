using BankSystemProject.Data;
using BankSystemProject.Helpers;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace BankSystemProject.Repositories.Service
{
    public class CustomerAccountsService : ICustomerAccount
    {
        private readonly Bank_DbContext _context;
        public CustomerAccountsService(Bank_DbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<Res_CustomersAccounts>> GetCustomersAccountsInfoAsync(bool IncludeDelete)
        {
            var customerAccounts = await _context.CustomersAccounts
             .Include(ca => ca.User)
             .Include(ca => ca.AccountType)
             .Select(ca => new Res_CustomersAccounts
             {
                 FullName = ca.User.FullName,
                 Email = ca.User.Email,
                 Phone = ca.User.PhoneNumber,
                 Address = ca.User.Address,
                 UserRole = ca.User.Role.ToString(),
                 IsDeleted = ca.User.IsDeleted,
                 Gender = ca.User.Gender,
                 AccountTypeName = ca.AccountType.AccountTypeName,
                 // Balance = ca.Balance,
                 CreatedDate = ca.CreatedDate,

             }).Where(c => IncludeDelete ? c.IsDeleted : !c.IsDeleted)
                 .ToListAsync();

            return customerAccounts;
        }
        public async Task<Res_UpdateAccTypeInCustomerAcc> UpdateAccountInfo(Req_UpdateAccTypeInCustomerAcc req_UpdateAccTypeIn, int customerAccountID)
        {

            var customerAccount = await _context.CustomersAccounts
                .Include(ca => ca.User)
                .Include(ca => ca.AccountType)
                .FirstOrDefaultAsync(acc => acc.CustomerAccountId == customerAccountID);

            if (customerAccount == null)
            {
                throw new KeyNotFoundException($"No customer account found with ID: {customerAccount}");
            }

            if (customerAccount.User == null)
            {
                throw new InvalidOperationException("User information is not available for this customer account.");
            }

            if (customerAccount.AccountType == null)
            {
                throw new InvalidOperationException("AccountType information is not available for this customer account.");
            }


            customerAccount.AccountTypeId = (int)req_UpdateAccTypeIn.AccountTypeName;

            if (customerAccount.AccountNumber == Base64Helper.Encode(req_UpdateAccTypeIn.OldAccountNumber))
            {
                var newAccountNumber = Base64Helper.Encode(req_UpdateAccTypeIn.NewAccountNumber);
                var confirmAccountNumber = Base64Helper.Encode(req_UpdateAccTypeIn.ConfirmNewAccountNumber);

                if (newAccountNumber == confirmAccountNumber)
                {
                    customerAccount.AccountNumber = newAccountNumber;
                }
                else
                {
                    throw new InvalidOperationException("The new account number and confirmation do not match.");
                }
            }
            else
            {
                throw new InvalidOperationException("The provided old account number does not match the current account number.");
            }
            await _context.SaveChangesAsync();


            return new Res_UpdateAccTypeInCustomerAcc
            {
                FullName = customerAccount.User.FullName,
                AccountTypeName = req_UpdateAccTypeIn.AccountTypeName.ToString(),
                AccountNumber = $"New account number is {req_UpdateAccTypeIn.NewAccountNumber}",
                Message = "Account type has been successfully updated."
            };
        }

        public async Task<Res_UpdateAccTypeInCustomerAcc> GetAccountTypeNameBycustomerAccountID(int customerAccountID)
        {
            var customerAccount = await _context.CustomersAccounts
             .Include(ca => ca.User)
             .Include(ca => ca.AccountType)
             .FirstOrDefaultAsync(acc => acc.CustomerAccountId == customerAccountID);

            if (customerAccount == null)
            {
                throw new KeyNotFoundException($"No customer account found with ID: {customerAccountID}");
            }

            if (customerAccount.User == null)
            {
                throw new InvalidOperationException("User information is not available for this customer account.");
            }

            if (customerAccount.AccountType == null)
            {
                throw new InvalidOperationException("AccountType information is not available for this customer account.");
            }

            return new Res_UpdateAccTypeInCustomerAcc
            {
                FullName = customerAccount.User.FullName,
                AccountTypeName = customerAccount.AccountType.AccountTypeName
            };
        }

        public async Task<Res_CustomersAccounts> GetAccountInfoByAccountNum(string AccountNum)
        {
            var customerAccounts = await _context.CustomersAccounts
           .Include(ca => ca.User)
           .Include(ca => ca.AccountType)
           .FirstOrDefaultAsync(u => u.AccountNumber == Base64Helper.Encode(AccountNum));

            if (customerAccounts == null)
            {
                return null;
            }
            var AccountInfo = new Res_CustomersAccounts
            {
                FullName = customerAccounts.User.FullName,
                Email = customerAccounts.User.Email,
                Phone = customerAccounts.User.PhoneNumber,
                Address = customerAccounts.User.Address,
                UserRole = customerAccounts.User.Role.ToString(),
                IsDeleted = customerAccounts.User.IsDeleted,
                Gender = customerAccounts.User.Gender,
                AccountTypeName = customerAccounts.AccountType.AccountTypeName,
                CreatedDate = customerAccounts.CreatedDate,
                AccountNumber=Base64Helper.Decode( customerAccounts.AccountNumber),
                Balance=customerAccounts.Balance,
                //Image=customerAccounts.Image
            };



            return AccountInfo;
        }

        public async Task<bool> UpdateCustomerInfoAsync(Req_UpdateCustomerInfoDto request, int customerAccountID)
        {
            // Fetch the customerAccount and customer account
            var customerAccount = await _context.CustomersAccounts.Include(u => u.User).FirstOrDefaultAsync(u => u.CustomerAccountId == customerAccountID);

            if (customerAccount == null)
            {
                throw new KeyNotFoundException($"User with ID {customerAccountID} not found.");
            }

            // UpdateLoanApplication customerAccount information
            customerAccount.User.PhoneNumber = request.PhoneNumber;
            customerAccount.User.Email = request.Email;
            customerAccount.User.UserName = request.UserName;

            // Check if an image is uploaded
            //if (request.ImageUrl != null)
            //{
            //    // Save the image and update the ImageUrl field
            //    var filePath = Path.Combine("wwwroot/images", request.ImageUrl.FileName);
            //    using (var stream = new FileStream(filePath, FileMode.SubmitLoanApplicationAsync))
            //    {
            //        await request.ImageUrl.CopyToAsync(stream);
            //    }
            //    customerAccount.ImageUrl = $"/images/{request.ImageUrl.FileName}";
            //}

            // UpdateLoanApplication the customer account
            if (customerAccount != null)
            {
                customerAccount.User.Address = request.Address;
            }

            // Save changes
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeleteCustomerAccountAsync(int CustomerAccountID)
        {
           
            var customerAccount = await _context.CustomersAccounts.Include(ca => ca.User)
                .FirstOrDefaultAsync(ca => ca.CustomerAccountId == CustomerAccountID);

            if (customerAccount == null)
            {
                throw new KeyNotFoundException($"Customer account not found for CustomerAccount ID: {CustomerAccountID}");
            }

           
            customerAccount.IsDeleted = true;
           
            var user = customerAccount.User;
            if (user != null)
            {
                user.IsDeleted = true;
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
    }
