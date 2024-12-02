using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BankSystemProject.Repositories.Service
{
    public class RepaymentLoanService:IRepaymentLoan

    {
        private readonly Bank_DbContext _context;

        public RepaymentLoanService(Bank_DbContext context)
        {
            _context = context;
        }

        public async Task<List<LoanRepayment>> GetRepaymentsByLoanIdAsync(int loanId)
        {
            return await _context.LoanRepayments
                .Where(r => r.LoanId == loanId)
                .ToListAsync();
        }

        public async Task<LoanRepayment> GetRepaymentByIdAsync(int id)
        {
            return await _context.LoanRepayments.FindAsync(id);
        }

        public async Task CreateLoanRepaymentAsync(LoanRepayment loanRepayment)
        {
            _context.LoanRepayments.Add(loanRepayment);
            await _context.SaveChangesAsync();
        }
    }
}
