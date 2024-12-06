using BankSystemProject.Shared.Enums;
using Mailjet.Client.Resources;
using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Models.DTOs
{
    public class Req_LoanApplicationDto
    {
        [Required(ErrorMessage = "Applicant name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Applicant name must be between 3 and 50 characters.")]
        public string ApplicantName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [Phone(ErrorMessage = "Invalid contact number format.")]
        [StringLength(15, ErrorMessage = "Contact number must be a maximum of 15 digits.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Income is required.")]
        [Range(1000, double.MaxValue, ErrorMessage = "Income must be at least 1000.")]
        public double Income { get; set; }

        [Required(ErrorMessage = "Employment status is required.")]
        public enEmploymentStatus EmploymentStatus { get; set; }

        [Required(ErrorMessage = "Bank account number is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Bank account number must be between 10 and 20 characters.")]
        public string BankAccountNumber { get; set;}

        [Required(ErrorMessage = "Loan amount is required.")]
        [Range(1000, double.MaxValue, ErrorMessage = "Loan amount must be at least 1000.")]
        public double LoanAmount { get; set; }

        //[Required(ErrorMessage = "Interest rate is required.")]
        //[Range(0.1, 100, ErrorMessage = "Interest rate must be between 0.1% and 100%.")]
        //public double InterestRate { get; set; }

        [Required(ErrorMessage = "Loan term is required.")]
        [Range(6, 360, ErrorMessage = "Loan term must be between 6 and 360 months.")]
        public enLoanTermMonth LoanTermMonths { get; set; }

        [Required(ErrorMessage = "Loan type is required.")]
        public enLoansTypes LoanType { get; set; }

        [Required(ErrorMessage = "Repayment schedule is required.")]
        public enRepaymentSchedule RepaymentSchedule { get; set; }

        public bool IsSecuredLoan { get; set; }
        [Required(ErrorMessage = "Collateral description is required for secured loans.")]
      //  [Required(nameof(IsSecuredLoan), true, ErrorMessage = "")]
        public enCollateralType CollateralDescription { get; set; }
    }


}
