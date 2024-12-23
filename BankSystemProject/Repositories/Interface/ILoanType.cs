using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface ILoanType
    {
        Task<List<Res_LoanTypeDto>> GetAllLoanTypesAsync();
        Task<Res_LoanTypeDto> GetLoanTypeByIdAsync(int loanTypeId);
        Task<Res_LoanTypeDto> CreateLoanTypeAsync(Req_LoanTypeDto loanTypeDto);
        Task<Res_LoanTypeDto> UpdateLoanTypeAsync(int loanTypeId, Req_LoanTypeDto loanTypeDto);
        Task<bool> DeleteLoanTypeAsync(int loanTypeId);
    }
}
