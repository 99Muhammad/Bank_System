using BankSystemProject.Data;
using BankSystemProject.Helpers;
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

        public async Task<List<Req_LoanRepaymentForGetAllDto>> GetAllRepaymentsAsync()
        {
            var repayments = await _context.LoanRepayments
                .Include(r => r.Loan)  // Optional: Include related loan info if needed
                .Select(r => new Req_LoanRepaymentForGetAllDto
                {
                    LoanRepaymentId = r.LoanRepaymentId,
                    LoanId = r.LoanId,
                    AmountPaid = r.AmountPaid,
                    PaymentDate = r.PaymentDate,
                    RemainingBalance = r.RemainingBalance
                })
                .ToListAsync();

            return repayments;
        }

        //public async Task CreateLoanRepaymentAsync(LoanRepayment loanRepayment)
        //{
        //    _context.LoanRepayments.Add(loanRepayment);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<List<Req_LoanRepaymentForGetAllDto>> GetRepaymentsByAccountNumberAsync(string accountNumber)
        {
            var loanRepayments = await _context.Loans
                .Where(l => l.CustomerAccount.AccountNumber == Base64Helper.Encode( accountNumber)) 
                .SelectMany(l => l.LoanRepayments)
                .Select(r => new Req_LoanRepaymentForGetAllDto
                {
                    LoanRepaymentId = r.LoanRepaymentId,
                    AmountPaid = r.AmountPaid,
                    PaymentDate = r.PaymentDate,
                    RemainingBalance = r.RemainingBalance
                })
                .ToListAsync();

            return loanRepayments;
        }

        public async Task<Res_LoanRepaymentDto> ProcessLoanRepaymentAsync(Req_LoanRepaymentDto repaymentDto)
        {

          
            var loan = await _context.Loans
                .Include(l => l.LoanRepayments)
                .Include(l => l.LoanApplication)  
                .FirstOrDefaultAsync(l => l.LoanId == repaymentDto.LoanId);

            if (loan == null)
            {
                return new Res_LoanRepaymentDto { Message = "Loan not found." };
            }

            // Calculate total amount paid so far and the remaining balance
            double totalPaid = loan.LoanRepayments.Sum(r => r.AmountPaid);
            double remainingBalance = loan.LoanAmount - totalPaid;

            
            if (repaymentDto.AmountPaid > remainingBalance)
            {
                
                return new Res_LoanRepaymentDto { Message = "Payment exceeds remaining balance." };
            }

            // Create a new repayment entry
            var repayment = new LoanRepayment
            {
                LoanId = loan.LoanId,
                AmountPaid = repaymentDto.AmountPaid,
                PaymentDate = DateTime.Now,
                RemainingBalance = remainingBalance - repaymentDto.AmountPaid
            };

            // Update the loan balance after repayment
            loan.LoanAmount = remainingBalance - repaymentDto.AmountPaid;
            loan.LoanApplication.LoanAmount = repaymentDto.AmountPaid;

            // If the loan is fully repaid, mark the loan and loan application as completed
            if (loan.LoanAmount == 0)
            {
                loan.Status = enLoanAndApplicationStatus.Completed.ToString();
                loan.LoanApplication.ApplicationStatus = enLoanAndApplicationStatus.Completed.ToString();
            }

            // Update the customer account balance by subtracting the repayment amount
            var customerAccount = await _context.CustomersAccounts
                .FirstOrDefaultAsync(ca => ca.CustomerAccountId == loan.CustomerAccountId);

            if (customerAccount == null)
            {
                return new Res_LoanRepaymentDto { Message = "Customer account not found." };
            }

            // Subtract the repayment amount from the customer account balance
            customerAccount.Balance -= repaymentDto.AmountPaid;

            // Add the repayment to the LoanRepayments table
            _context.LoanRepayments.Add(repayment);

            // Save all changes (LoanRepayment, Loan, LoanApplication, and CustomerAccount updates)
            await _context.SaveChangesAsync();

            return new Res_LoanRepaymentDto
            {
                Message = "Repayment processed successfully.",
                RemainingBalance = repayment.RemainingBalance
            };
        }
    }
}
