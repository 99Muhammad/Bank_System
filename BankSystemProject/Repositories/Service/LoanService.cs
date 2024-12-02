using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BankSystemProject.Repositories.Service
{
    public class LoanService:ILoan
    {
        private readonly Bank_DbContext _context;

        public LoanService(Bank_DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetAllLoansAsync()
        {
            return await _context.Loans.Include(l => l.LoanType).Include(l => l.CustomerAccount).ToListAsync();
        }

        public async Task<Loan> GetLoanByIdAsync(int id)
        {
            return await _context.Loans
                .Include(l => l.LoanType)
                .Include(l => l.CustomerAccount)
                .FirstOrDefaultAsync(l => l.LoanId == id);
        }

        public async Task CreateLoanAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLoanAsync(int id, Loan loan)
        {
            var existingLoan = await _context.Loans.FindAsync(id);
            if (existingLoan == null) throw new KeyNotFoundException("Loan not found");

            _context.Entry(existingLoan).CurrentValues.SetValues(loan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLoanAsync(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null) throw new KeyNotFoundException("Loan not found");

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
        }
    }
}
