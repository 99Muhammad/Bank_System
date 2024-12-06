using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface ILoanApplication
    {
        Task<List<Res_GetLoanApplicationDto>> GetAllApplicationsAsync();
        Task<Res_GetLoanApplicationDto> GetApplicationByIdAsync(int ApplicationID);
        Task<Res_LoanApplicationDto> SubmitLoanApplicationAsync(Req_LoanApplicationDto loanApplication);
        Task<bool> UpdateLoanApplicationAsync(int loanApplicationID, Req_UpdateLoanApplicationDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<List<Res_GetLoanApplicationDto>> GetApplicationsByStatusAsync(string status);

        Task<Res_EmiDto> CalculateStandaloneEmiAsync(Req_StandaloneEmiDto emiRequest);
        Task<Res_EmiDto> CalculateEmiForApplicationAsync(int loanApplicationId);

        Task<bool> ApproveLoanApplicationAsync(int loanApplicationId);
    }
}
