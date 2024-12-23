using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BankSystemProject.Repositories.Service
{
    public class AccountTypeService:IAccountType
    {
        private readonly Bank_DbContext _dbContext;

        public AccountTypeService(Bank_DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public async Task<List<Res_AccountTypeDto>> GetAllAccountTypesAsync()
        {
            return await _dbContext.AccountTypes
                .Select(at => new Res_AccountTypeDto
                {
                    AccountTypeId = at.AccountTypeId,
                    AccountTypeName = at.AccountTypeName,
                    Description = at.Description
                })
                .ToListAsync();
        }

        

        public async Task<Res_AccountTypeDto> GetAccountTypeByIdAsync(int accountTypeId)
        {
            var accountType = await _dbContext.AccountTypes.FindAsync(accountTypeId);
            if (accountType == null) return null;

            return new Res_AccountTypeDto
            {
                AccountTypeId = accountType.AccountTypeId,
                AccountTypeName = accountType.AccountTypeName,
                Description = accountType.Description
            };
        }

        public async Task<Res_AccountTypeDto> CreateAccountTypeAsync(Req_AccountTypeDto accountTypeDto)
        {
            var accountType = new AccountType
            {
                AccountTypeName = accountTypeDto.AccountTypeName,
                Description = accountTypeDto.Description
            };

            _dbContext.AccountTypes.Add(accountType);
            await _dbContext.SaveChangesAsync();

            return new Res_AccountTypeDto
            {
                AccountTypeId = accountType.AccountTypeId,
                AccountTypeName = accountType.AccountTypeName,
                Description = accountType.Description
            };
        }

        public async Task<Res_AccountTypeDto> UpdateAccountTypeAsync(int accountTypeId, Req_AccountTypeDto accountTypeDto)
        {
            var accountType = await _dbContext.AccountTypes.FindAsync(accountTypeId);
            if (accountType == null) return null;

            accountType.AccountTypeName = accountTypeDto.AccountTypeName;
            accountType.Description = accountTypeDto.Description;

            await _dbContext.SaveChangesAsync();

            return new Res_AccountTypeDto
            {
                AccountTypeId = accountType.AccountTypeId,
                AccountTypeName = accountType.AccountTypeName,
                Description = accountType.Description
            };
        }

        public async Task<bool> DeleteAccountTypeAsync(int accountTypeId)
        {
            var accountType = await _dbContext.AccountTypes.FindAsync(accountTypeId);
            if (accountType == null) return false;

            _dbContext.AccountTypes.Remove(accountType);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
