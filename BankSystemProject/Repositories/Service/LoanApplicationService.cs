using BankSystemProject.Data;
using BankSystemProject.Helpers;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Shared.Enums;
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
            ApplicationStatus= la.ApplicationStatus,
            ApplicationDate=la.ApplicationDate,
            //CustomerAccountName = la.CustomersAccounts.User.FullName,
        })
        .ToListAsync();


        }

        public async Task<Res_GetLoanApplicationDto> GetApplicationByIdAsync(int ApplicationID)
        {
            var loanApplication =await _context.LoanApplications
                  .Include(la => la.CustomersAccounts)
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
                Email = loanApplication.CustomersAccounts.User.Email,
                LoanAmount = loanApplication.LoanAmount,
                LoanTermMonths = loanApplication.LoanTermMonths,
                LoanTypeName = loanApplication.LoanType.LoanTypeName,
                // CustomerAccountName = loanApplication.CustomersAccounts.User.FullName,
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

            var isExist= _context.LoanApplications.Any(c => c.BankAccountNumber
            == Base64Helper.Encode(loanApplication.BankAccountNumber));
            
            if(isExist)
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
            var loanApplication = await _context.LoanApplications.FirstOrDefaultAsync(la => la.LoanApplicationId == id);

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
                .Include(la=>la.CustomersAccounts)
                .Include(la=>la.LoanType)
                .Where(la => la.ApplicationStatus == status)
                .Select(la => new Res_GetLoanApplicationDto
                {
                    LoanApplicationId = la.LoanApplicationId,
                    ApplicantName = la.CustomersAccounts.User.FullName,
                    Email = la.Email,
                    Address=la.Address,
                    ApplicationStatus= la.ApplicationStatus,
                    LoanAmount = la.LoanAmount,
                    LoanTermMonths = la.LoanTermMonths,
                    LoanTypeName=la.LoanType.LoanTypeName,
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
           
            // Load the LoanApplication, loan, and CustomerAccount (with explicit includes)
            var loanApplication = await _context.LoanApplications
                .Include(la => la.LoanType)
                .Include(l => l.CustomersAccounts)
                // Ensure CustomerAccount is included
                .FirstOrDefaultAsync(la => 
                la.LoanApplicationId == loanApplicationId
                && la.ApplicationStatus == enLoanAndApplicationStatus.Pending.ToString());
          
            loanApplication.ApplicationStatus = enLoanAndApplicationStatus.Approved.ToString();
            _context.LoanApplications.Update(loanApplication);

            // Create new loan
            var loan = new Loan
            {
                LoanApplicationId = loanApplicationId,
                LoanAmount = loanApplication.LoanAmount,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(loanApplication.LoanTermMonths),
                Status = loanApplication.ApplicationStatus,
                LoanTypeId = loanApplication.LoanTypeId,
                CustomerAccountId = loanApplication.CustomerAccountId,
                ApprovedDate=DateTime.Now,
            };

            _context.Loans.Add(loan);

            // Update CustomerAccount balance
            loanApplication.CustomersAccounts.Balance += loan.LoanAmount;

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
