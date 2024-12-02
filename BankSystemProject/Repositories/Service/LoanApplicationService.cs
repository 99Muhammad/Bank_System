using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BankSystemProject.Repositories.Service
{
    public class LoanApplicationService:ILoanApplication
    {
        private readonly Bank_DbContext _context;

        public LoanApplicationService(Bank_DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoanApplication>> GetAllAsync()
        {
            return await _context.LoanApplications
                .Include(la => la.CustomerAccount)
                .Include(la => la.LoanType)
                .ToListAsync();
        }

        public async Task<LoanApplication> GetByIdAsync(int ApplicationID)
        {

            return await _context.LoanApplications
                .Include(la => la.CustomerAccount)
                .Include(la => la.LoanType)
                .FirstOrDefaultAsync(la => la.LoanApplicationId == ApplicationID);
        }

        public async Task<LoanApplication> CreateAsync(LoanApplication loanApplication)
        {
            _context.LoanApplications.Add(loanApplication);
            await _context.SaveChangesAsync();
            return loanApplication;
        }

        public async Task<LoanApplication> UpdateAsync(int id, LoanApplication loanApplication)
        {
            var existingLoanApplication = await _context.LoanApplications.FindAsync(id);
            if (existingLoanApplication == null) return null;

            existingLoanApplication.ApplicantName = loanApplication.ApplicantName;
            existingLoanApplication.Address = loanApplication.Address;
            existingLoanApplication.ContactNumber = loanApplication.ContactNumber;
            existingLoanApplication.Email = loanApplication.Email;
            existingLoanApplication.Income = loanApplication.Income;
            existingLoanApplication.EmploymentStatus = loanApplication.EmploymentStatus;
            existingLoanApplication.BankAccountNumber = loanApplication.BankAccountNumber;
            existingLoanApplication.LoanAmount = loanApplication.LoanAmount;
            existingLoanApplication.InterestRate = loanApplication.InterestRate;
            existingLoanApplication.LoanTermMonths = loanApplication.LoanTermMonths;
            existingLoanApplication.CustomerAccountId = loanApplication.CustomerAccountId;
            existingLoanApplication.LoanTypeId = loanApplication.LoanTypeId;
            existingLoanApplication.ApplicationDate = loanApplication.ApplicationDate;
            existingLoanApplication.RepaymentSchedule = loanApplication.RepaymentSchedule;
            existingLoanApplication.IsSecuredLoan = loanApplication.IsSecuredLoan;
            existingLoanApplication.CollateralDescription = loanApplication.CollateralDescription;
            existingLoanApplication.ApplicationStatus = loanApplication.ApplicationStatus;

            await _context.SaveChangesAsync();
            return existingLoanApplication;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var loanApplication = await _context.LoanApplications.FindAsync(id);
            if (loanApplication == null) return false;

            _context.LoanApplications.Remove(loanApplication);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
