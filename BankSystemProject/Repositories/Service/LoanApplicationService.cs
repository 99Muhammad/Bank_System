using BankSystemProject.Data;
using BankSystemProject.Helpers;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Shared.Enums;
using Mailjet.Client.TransactionalEmails;
using Mailjet.Client;
using Microsoft.EntityFrameworkCore;
using System;

namespace BankSystemProject.Repositories.Service
{
    public class LoanApplicationService : ILoanApplication
    {
        private readonly Bank_DbContext _context;
        private readonly IEmail _emailService;

        public LoanApplicationService(Bank_DbContext context, IEmail emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<List<Res_GetLoanApplicationDto>> GetAllApplicationsAsync()
        {
            return await _context.LoanApplications
        .Include(la => la.CustomersAccounts)
        .Include(la => la.LoanType)
        .Select(la => new Res_GetLoanApplicationDto
        {
            LoanApplicationId = la.LoanApplicationId,
            ApplicantName = la.ApplicantName,
            Address = la.Address,
            ContactNumber = la.ContactNumber,
            Email = la.Email,
            LoanAmount = la.LoanAmount,
            LoanTermMonths = la.LoanTermMonths,
            LoanTypeName = la.LoanType.LoanTypeName,
            ApplicationStatus = la.ApplicationStatus,
            ApplicationDate = la.ApplicationDate,
            //CustomerAccountName = la.CustomersAccounts.User.FullName,
        })
        .ToListAsync();


        }

        public async Task<Res_GetLoanApplicationDto> GetApplicationByIdAsync(int ApplicationID)
        {
            var loanApplication = await _context.LoanApplications
       .Include(la => la.CustomersAccounts)
       .ThenInclude(ca => ca.User) // Ensure User is included
       .Include(la => la.LoanType)
       .FirstOrDefaultAsync(e => e.LoanApplicationId == ApplicationID);

            if (loanApplication == null)
            {
                return null;
            }

            var result = new Res_GetLoanApplicationDto
            {
                LoanApplicationId = loanApplication.LoanApplicationId,
                ApplicantName = loanApplication.ApplicantName,
                Address = loanApplication.Address,
                ContactNumber = loanApplication.ContactNumber,
                Email = loanApplication.CustomersAccounts?.User?.Email, // Use null conditional operator
                LoanAmount = loanApplication.LoanAmount,
                LoanTermMonths = loanApplication.LoanTermMonths,
                LoanTypeName = loanApplication.LoanType?.LoanTypeName, // Use null conditional operator
                                                                       // CustomerAccountName = loanApplication.CustomersAccounts?.User?.FullName, 
                ApplicationStatus = loanApplication.ApplicationStatus,
                ApplicationDate = loanApplication.ApplicationDate,
            };

            return result;
        }

        public async Task<Res_LoanApplicationDto> SubmitLoanApplicationAsync(Req_LoanApplicationDto loanApplication)
        {
            var customerAccount = await _context.CustomersAccounts
                .Include(c => c.User)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.AccountNumber ==
                Base64Helper.Encode(loanApplication.BankAccountNumber));


            var loanType = await _context.LoanTypes.FirstOrDefaultAsync(l => l.LoanTypeId == (int)loanApplication.LoanType);

            if (loanType == null)
            {
                return new Res_LoanApplicationDto
                {
                    Message = "Invalid loan type."
                };
            }

            if (customerAccount == null)
            {
                return new Res_LoanApplicationDto
                {
                    Message = "Customer account not found."
                };
            }

            var isExist = _context.LoanApplications.Any(c => c.BankAccountNumber
            == Base64Helper.Encode(loanApplication.BankAccountNumber));

            if (isExist)
            {
                return new Res_LoanApplicationDto
                {
                    Message = "You cann't apply loan application multiple time! ."
                };
            }

            var loanApp = new LoanApplication
            {
                ApplicantName = loanApplication.ApplicantName,
                Address = loanApplication.Address,
                ContactNumber = loanApplication.ContactNumber,
                Email = loanApplication.Email,
                Income = loanApplication.Income,
                EmploymentStatus = loanApplication.EmploymentStatus.ToString(),
                BankAccountNumber = Base64Helper.Encode(loanApplication.BankAccountNumber),
                LoanAmount = loanApplication.LoanAmount,
                LoanTermMonths = (int)loanApplication.LoanTermMonths,
                CustomerAccountId = customerAccount.CustomerAccountId,
                LoanTypeId = (int)loanApplication.LoanType,
                ApplicationDate = DateTime.Now,
                RepaymentSchedule = loanApplication.RepaymentSchedule.ToString(),
                IsSecuredLoan = loanApplication.IsSecuredLoan,
                CollateralDescription = loanApplication.CollateralDescription.ToString(),
                ApplicationStatus = enLoanAndApplicationStatus.Pending.ToString(),

                InterestRate = loanType.InterestRate
            };

            _context.LoanApplications.Add(loanApp);
            await _context.SaveChangesAsync();

            // Return the response with status and message
            return new Res_LoanApplicationDto
            {
                ApplicationStatus = enLoanAndApplicationStatus.Pending.ToString(),
                Message = "Loan application submitted successfully."
            };
        }

        public async Task<bool> UpdateLoanApplicationAsync(int id, Req_UpdateLoanApplicationDto updateDto)
        {
            var loanApplication = await _context.LoanApplications
                .Include(l=>l.loan).FirstOrDefaultAsync(la => la.LoanApplicationId == id);

            if (loanApplication == null)
            {
                return false; // loan application not found
            }

            // UpdateLoanApplication the properties
            // loanApplication.ApplicantName = updateDto.ApplicantName ?? loanApplication.ApplicantName;
            loanApplication.Address = updateDto.Address ?? loanApplication.Address;
            loanApplication.ContactNumber = updateDto.ContactNumber ?? loanApplication.ContactNumber;
            loanApplication.Email = updateDto.Email ?? loanApplication.Email;
            loanApplication.LoanAmount = updateDto.LoanAmount > 0 ? updateDto.LoanAmount : loanApplication.LoanAmount;
            loanApplication.LoanTermMonths = updateDto.LoanTermMonths > 0 ? (int)updateDto.LoanTermMonths : loanApplication.LoanTermMonths;
            loanApplication.EmploymentStatus = updateDto.EmploymentStatus.ToString() ?? loanApplication.EmploymentStatus;
            loanApplication.IsSecuredLoan = updateDto.IsSecuredLoan;
            loanApplication.CollateralDescription = updateDto.CollateralDescription.ToString() ?? loanApplication.CollateralDescription;
            loanApplication.RepaymentSchedule = updateDto.RepaymentSchedule.ToString() ?? loanApplication.RepaymentSchedule;
            // loanApplication.ApplicationStatus = updateDto.ApplicationStatus.ToString() ?? loanApplication.ApplicationStatus;
            loanApplication.ApplicationStatus = enLoanAndApplicationStatus.Pending.ToString();
            loanApplication.loan.Status = enLoanAndApplicationStatus.Pending.ToString();
            _context.LoanApplications.Update(loanApplication);
            await _context.SaveChangesAsync();

            return true; // UpdateLoanApplication successful
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var loanApplication = await _context.LoanApplications.FindAsync(id);
            if (loanApplication == null) return false;

            _context.LoanApplications.Remove(loanApplication);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Res_GetLoanApplicationDto>> GetApplicationsByStatusAsync(string status)
        {

            var loanApplications = await _context.LoanApplications
                .Include(la => la.CustomersAccounts)
                .Include(la => la.LoanType)
                .Where(la => la.ApplicationStatus == status)
                .Select(la => new Res_GetLoanApplicationDto
                {
                    LoanApplicationId = la.LoanApplicationId,
                    ApplicantName = la.CustomersAccounts.User.FullName,
                    Email = la.Email,
                    Address = la.Address,
                    ApplicationStatus = la.ApplicationStatus,
                    LoanAmount = la.LoanAmount,
                    LoanTermMonths = la.LoanTermMonths,
                    LoanTypeName = la.LoanType.LoanTypeName,
                    ApplicationDate = la.ApplicationDate
                })
                .ToListAsync();

            return loanApplications;
        }


        public async Task<Res_EmiDto> CalculateStandaloneEmiAsync(Req_StandaloneEmiDto emiRequest)
        {
            double emi = EmiCalculator.CalculateEMI(emiRequest.LoanAmount, emiRequest.AnnualInterestRate, emiRequest.LoanTermMonths);
            double totalPayment = emi * emiRequest.LoanTermMonths;
            double totalInterest = totalPayment - emiRequest.LoanAmount;

            return new Res_EmiDto
            {
                EMI = emi,
                TotalPayment = totalPayment,
                TotalInterest = totalInterest
            };
        }


        public async Task<Res_EmiDto> CalculateEmiForApplicationAsync(int loanApplicationId)
        {
            var loanApplication = await _context.LoanApplications
                .FirstOrDefaultAsync(la => la.LoanApplicationId == loanApplicationId);

            if (loanApplication == null)
                throw new KeyNotFoundException($"Loan application with ID {loanApplicationId} not found.");

            double emi = EmiCalculator.CalculateEMI(loanApplication.LoanAmount, loanApplication.InterestRate, loanApplication.LoanTermMonths);
            double totalPayment = emi * loanApplication.LoanTermMonths;
            double totalInterest = totalPayment - loanApplication.LoanAmount;

            return new Res_EmiDto
            {
                EMI = emi,
                TotalPayment = totalPayment,
                TotalInterest = totalInterest
            };
        }

        public async Task<bool> ApproveLoanApplicationAsync(int loanApplicationId)
        {
            
            var loanApplication = await _context.LoanApplications
                .Include(la => la.LoanType)
                .Include(la => la.CustomersAccounts)
                .FirstOrDefaultAsync(la =>
                    la.LoanApplicationId == loanApplicationId
                    && la.ApplicationStatus == enLoanAndApplicationStatus.Pending.ToString());

            if (loanApplication == null)
                throw new InvalidOperationException("Loan application not found or not in Pending status.");

           
            var existingLoan = await _context.Loans
                .FirstOrDefaultAsync(l => l.LoanApplicationId == loanApplicationId);

            if (existingLoan != null && existingLoan.Status == enLoanAndApplicationStatus.Approved.ToString())
                throw new InvalidOperationException("A loan for this application has already been approved.");

    
            loanApplication.ApplicationStatus = enLoanAndApplicationStatus.Approved.ToString();
            _context.LoanApplications.Update(loanApplication);

          
            var loan = new Loan
            {
                LoanApplicationId = loanApplicationId,
                LoanAmount = loanApplication.LoanAmount,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(loanApplication.LoanTermMonths),
                Status = loanApplication.ApplicationStatus,
                LoanTypeId = loanApplication.LoanTypeId,
                CustomerAccountId = loanApplication.CustomerAccountId,
                ApprovedDate = DateTime.Now,
            };

            _context.Loans.Update(loan);

          
            loanApplication.CustomersAccounts.Balance += loan.LoanAmount;

           
            await _context.SaveChangesAsync();

            //Send approval email to the customer
                await _emailService.SendEmailAsync(
                    loanApplication.Email,
                    loanApplication.ApplicantName,
                    "Loan Application Approved",
                    $@"
                <p>Dear {loanApplication.ApplicantName},</p>
                <p>Congratulations! Your loan application with ID <strong>{loanApplication.LoanApplicationId}</strong> has been approved.</p>
                <p>Loan Amount: {loanApplication.LoanAmount:C}</p>
                <p>Loan Term: {loanApplication.LoanTermMonths} months</p>
                <p>Thank you for choosing our services.</p>
                <p>Best regards,</p>
                <p>NovaBank</p>
            "
                );

            return true;
        }
       
    }
}
