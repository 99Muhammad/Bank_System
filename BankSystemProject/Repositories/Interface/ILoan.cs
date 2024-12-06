using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Shared.Enums;

namespace BankSystemProject.Repositories.Interface
{
    public interface ILoan
    {
        Task<List<Res_GetAllLoans>> GetAllLoansAsync();
        Task<Res_GetAllLoans> GetLoanByIdAsync(int id);
        // Task CreateLoanAsync(Loan loan);
        //  Task UpdateLoanAsync(int id, Loan loan);
        Task<List<Res_GetAllLoans>> GetLoansByStatusAsync(enLoanAndApplicationStatus status);
        Task DeleteLoanAsync(int id);
    }
}
