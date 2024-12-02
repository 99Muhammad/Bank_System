using BankSystemProject.Model;

namespace BankSystemProject.Repositories.Interface
{
    public interface IRepaymentLoan
    {
        Task<List<LoanRepayment>> GetRepaymentsByLoanIdAsync(int loanId);
        Task<LoanRepayment> GetRepaymentByIdAsync(int id);
        Task CreateLoanRepaymentAsync(LoanRepayment loanRepayment);
    }
}
