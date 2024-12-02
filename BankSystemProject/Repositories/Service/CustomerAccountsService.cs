using BankSystemProject.Data;
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

        public async Task<List<Res_CustomersAccounts>> GetCustomersAccountsInfoAsync()
        {
            var customerAccounts = await _context.CustomersAccounts
             .Include(ca => ca.User) 
             .Include(ca => ca.AccountType) 
             .Select(ca => new Res_CustomersAccounts
                 {
                     FullName = ca.User.FullName,
                     Email = ca.User.Email,
                     Phone=ca.User.PhoneNumber,
                     Address=ca.User.Address,
                     UserRole =ca.User.Role.ToString(),
                     IsDeleted=ca.User.IsDeleted, 
                     Gender=ca.User.Gender,
                     AccountTypeName = ca.AccountType.AccountTypeName,
                    // Balance = ca.Balance,
                     CreatedDate = ca.CreatedDate,
    
               })
                 .ToListAsync();

            return customerAccounts;
        }
        public async Task<Res_UpdateAccTypeInCustomerAcc> UpdateAccTypeInCustomerAccByUserID(Req_UpdateAccTypeInCustomerAcc req_UpdateAccTypeIn, string UserID)
        {

            // Load customer account along with customerAccount and AccountType data
            // Fetch customer account with related entities
            var customerAccount = await _context.CustomersAccounts
                .Include(ca => ca.User)
                .Include(ca => ca.AccountType)
                .FirstOrDefaultAsync(acc => acc.UserId == UserID);

            if (customerAccount == null)
            {
                throw new KeyNotFoundException($"No customer account found with ID: {UserID}");
            }

            if (customerAccount.User == null)
            {
                throw new InvalidOperationException("User information is not available for this customer account.");
            }

            if (customerAccount.AccountType == null)
            {
                throw new InvalidOperationException("AccountType information is not available for this customer account.");
            }

            // Update AccountTypeId
            customerAccount.AccountTypeId = (int)req_UpdateAccTypeIn.AccountTypeName;

            await _context.SaveChangesAsync();

            // Return updated response
            return new Res_UpdateAccTypeInCustomerAcc
            {
                FullName = customerAccount.User.FullName,
                AccountTypeName = customerAccount.AccountType.AccountTypeName
            };
        }
        public async Task<Res_UpdateAccTypeInCustomerAcc> GetAccountTypeNameByUserID(string UserID)
        {
            var customerAccount = await _context.CustomersAccounts
             .Include(ca => ca.User)
             .Include(ca => ca.AccountType)
             .FirstOrDefaultAsync(acc => acc.User.Id == UserID);

            if (customerAccount == null)
            {
                throw new KeyNotFoundException($"No customer account found with ID: {UserID}");
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
           .FirstOrDefaultAsync(u => u.AccountNumber == AccountNum);

            if(customerAccounts==null)
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
           };
         
               

            return AccountInfo;
        }

    }
}
