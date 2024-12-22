using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BankSystemProject.Repositories.Service
{
    public class LoanTypeService:ILoanType
    {
        private readonly Bank_DbContext _dbContext;

        public LoanTypeService(Bank_DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Res_LoanTypeDto>> GetAllLoanTypesAsync()
        {
            return await _dbContext.LoanTypes
                .Select(lt => new Res_LoanTypeDto
                {
                    //LoanTypeId = lt.LoanTypeId,
                    LoanTypeName = lt.LoanTypeName,
                    InterestRate = lt.InterestRate,
                    Description = lt.Description,
                    LoanTermMonths = lt.LoanTermMonths
                })
                .ToListAsync();
        }

        public async Task<Res_LoanTypeDto> GetLoanTypeByIdAsync(int loanTypeId)
        {
            var loanType = await _dbContext.LoanTypes.FindAsync(loanTypeId);
            if (loanType == null) return null;

            return new Res_LoanTypeDto
            {
                //LoanTypeId = loanType.LoanTypeId,
                LoanTypeName = loanType.LoanTypeName,
                InterestRate = loanType.InterestRate,
                Description = loanType.Description,
                LoanTermMonths = loanType.LoanTermMonths
            };
        }

        public async Task<Res_LoanTypeDto> CreateLoanTypeAsync(Req_LoanTypeDto loanTypeDto)
        {
            var loanType = new LoanType
            {
                LoanTypeName = loanTypeDto.LoanTypeName,
                InterestRate = loanTypeDto.InterestRate,
                Description = loanTypeDto.Description,
                LoanTermMonths = loanTypeDto.LoanTermMonths
            };

            _dbContext.LoanTypes.Add(loanType);
            await _dbContext.SaveChangesAsync();

            return new Res_LoanTypeDto
            {
                //LoanTypeId = loanType.LoanTypeId,
                LoanTypeName = loanType.LoanTypeName,
                InterestRate = loanType.InterestRate,
                Description = loanType.Description,
                LoanTermMonths = loanType.LoanTermMonths
            };
        }

        public async Task<Res_LoanTypeDto> UpdateLoanTypeAsync(int loanTypeId, Req_LoanTypeDto loanTypeDto)
        {
            var loanType = await _dbContext.LoanTypes.FindAsync(loanTypeId);
            if (loanType == null) return null;

            loanType.LoanTypeName = loanTypeDto.LoanTypeName;
            loanType.InterestRate = loanTypeDto.InterestRate;
            loanType.Description = loanTypeDto.Description;
            loanType.LoanTermMonths = loanTypeDto.LoanTermMonths;

            await _dbContext.SaveChangesAsync();

            return new Res_LoanTypeDto
            {
                //LoanTypeId = loanType.LoanTypeId,
                LoanTypeName = loanType.LoanTypeName,
                InterestRate = loanType.InterestRate,
                Description = loanType.Description,
                LoanTermMonths = loanType.LoanTermMonths
            };
        }

        public async Task<bool> DeleteLoanTypeAsync(int loanTypeId)
        {
            var loanType = await _dbContext.LoanTypes.FindAsync(loanTypeId);
            if (loanType == null) return false;

            _dbContext.LoanTypes.Remove(loanType);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
