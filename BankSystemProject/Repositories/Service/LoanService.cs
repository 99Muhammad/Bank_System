using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Shared.Enums;
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

        public async Task<List<Res_GetAllLoans>> GetAllLoansAsync()
        {
            var loans = await _context.Loans
                .Include(l => l.LoanApplication)
                .Include(l => l.LoanType)
                .Include(l => l.CustomerAccount)
                .ToListAsync();  

            var result = loans.Select(l => new Res_GetAllLoans
            {
                ApplicantName = l.LoanApplication.ApplicantName,
                LoanTypeName = l.LoanType.LoanTypeName,  
                LoanAmount = (int)l.LoanAmount,
                LoanStartDate = l.StartDate,
                LoanEndDate = l.EndDate,
                Status = l.Status,
                ApprovedDate = l.ApprovedDate
            }).ToList();

            return result;
        }


        public async Task<Res_GetAllLoans> GetLoanByIdAsync(int LoanID)
        {
            

            var loans = await _context.Loans
               .Include(l => l.LoanApplication)
               .Include(l => l.LoanType)
               .Include(l => l.CustomerAccount)
               .FirstOrDefaultAsync(l => l.LoanId == LoanID); ;

            if(loans==null)
            {
                return null;
            }
            var result = new Res_GetAllLoans
            {
                ApplicantName = loans.LoanApplication.ApplicantName,
                LoanTypeName = loans.LoanType.LoanTypeName,
                LoanAmount = (int)loans.LoanAmount,
                LoanStartDate = loans.StartDate,
                LoanEndDate = loans.EndDate,
                Status = loans.Status,
                ApprovedDate = loans.ApprovedDate
            };

            return result;
        }

        //public async Task CreateLoanAsync(Loan loan)
        //{
        //    _context.Loans.Add(loan);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdateLoanAsync(int id, Loan loan)
        //{
        //    var existingLoan = await _context.Loans.FindAsync(id);
        //    if (existingLoan == null) throw new KeyNotFoundException("Loan not found");

        //    _context.Entry(existingLoan).CurrentValues.SetValues(loan);
        //    await _context.SaveChangesAsync();
        //}

        public async Task DeleteLoanAsync(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null) throw new KeyNotFoundException("Loan not found");

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Res_GetAllLoans>> GetLoansByStatusAsync(enLoanAndApplicationStatus status)
        {
            var loans = await _context.Loans
                .Include(l => l.LoanApplication)
                .Include(l => l.LoanType)
                .Include(l => l.CustomerAccount)
                .Where(l => l.Status == status.ToString())
                .ToListAsync();

            var result = loans.Select(l => new Res_GetAllLoans
            {
                ApplicantName = l.LoanApplication.ApplicantName,
                LoanTypeName = l.LoanType.LoanTypeName,
                LoanAmount = (int)l.LoanAmount,
                LoanStartDate = l.StartDate,
                LoanEndDate = l.EndDate,
                Status = l.Status,
                ApprovedDate = l.ApprovedDate
            }).ToList();

            return result;
        }
    }
}
