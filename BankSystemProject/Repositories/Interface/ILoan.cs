using BankSystemProject.Model;

namespace BankSystemProject.Repositories.Interface
{
    public interface ILoan
    {
        Task<IEnumerable<Loan>> GetAllLoansAsync();
        Task<Loan> GetLoanByIdAsync(int id);
        Task CreateLoanAsync(Loan loan);
        Task UpdateLoanAsync(int id, Loan loan);
        Task DeleteLoanAsync(int id);
    }
}
