using BankSystemProject.Model;

namespace BankSystemProject.Repositories.Interface
{
    public interface ILoanApplication
    {
        Task<IEnumerable<LoanApplication>> GetAllAsync();
        Task<LoanApplication> GetByIdAsync(int id);
        Task<LoanApplication> CreateAsync(LoanApplication loanApplication);
        Task<LoanApplication> UpdateAsync(int id, LoanApplication loanApplication);
        Task<bool> DeleteAsync(int id);
    }
}
