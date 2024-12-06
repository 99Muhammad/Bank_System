using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace BankSystemProject.Repositories.Service
{
    public class RepaymentLoanService : IRepaymentLoan

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


        public async Task<Res_LoanRepaymentDto> ProcessLoanRepaymentAsync(Req_LoanRepaymentDto repaymentDto)
        {

            var loan = await _context.Loans
                .Include(l => l.LoanRepayments)
                .FirstOrDefaultAsync(l => l.LoanId == repaymentDto.LoanId);

            if (loan == null)
            {
                return new Res_LoanRepaymentDto { Message = "Loan not found." };
            }


            double totalPaid = loan.LoanRepayments.Sum(r => r.AmountPaid);
            double remainingBalance = loan.LoanAmount - totalPaid;

            if (repaymentDto.AmountPaid > remainingBalance)
            {
                return new Res_LoanRepaymentDto { Message = "Payment exceeds remaining balance." };
            }


            var repayment = new LoanRepayment
            {
                LoanId = loan.LoanId,
                AmountPaid = repaymentDto.AmountPaid,
                PaymentDate = DateTime.Now,
                RemainingBalance = remainingBalance - repaymentDto.AmountPaid
            };
            
            if(remainingBalance==0)
            {
                loan.LoanApplication.loan.Status = enLoanAndApplicationStatus.Completed.ToString();
                loan.Status = enLoanAndApplicationStatus.Completed.ToString();
            }

            _context.LoanRepayments.Add(repayment);
            await _context.SaveChangesAsync();

            return new Res_LoanRepaymentDto
            {
                Message = "Repayment processed successfully.",
                RemainingBalance = repayment.RemainingBalance
            };
        }
    }
}
