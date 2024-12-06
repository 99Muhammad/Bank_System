using BankSystemProject.Shared.Enums;

namespace BankSystemProject.Models.DTOs
{
    public class Req_UpdateLoanApplicationDto
    {
      //  public string ApplicantName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public double LoanAmount { get; set; }
        public enLoanTermMonth LoanTermMonths { get; set; }
        public enEmploymentStatus EmploymentStatus { get; set; }
        public bool IsSecuredLoan { get; set; }
        public enCollateralType CollateralDescription { get; set; }
        public enRepaymentSchedule RepaymentSchedule { get; set; }
        //public enLoanAndApplicationStatus ApplicationStatus { get; set; }
    }
}
