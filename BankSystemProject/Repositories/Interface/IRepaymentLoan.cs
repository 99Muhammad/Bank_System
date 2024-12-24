using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface IRepaymentLoan
    {
        Task<List<LoanRepayment>> GetRepaymentsByLoanIdAsync(int loanId);
        Task<LoanRepayment> GetRepaymentByIdAsync(int id);
        //Task CreateLoanRepaymentAsync(LoanRepayment loanRepayment);
        Task<List<Req_LoanRepaymentForGetAllDto>> GetAllRepaymentsAsync();
        Task<List<Req_LoanRepaymentForGetAllDto>> GetRepaymentsByAccountNumberAsync(string accountNumber);

        Task<Res_LoanRepaymentDto> ProcessLoanRepaymentAsync(Req_LoanRepaymentDto repaymentDto);

    }
}
