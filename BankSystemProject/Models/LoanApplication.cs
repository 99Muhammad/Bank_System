using BankSystemProject.Shared.Enums;

namespace BankSystemProject.Model
{
    public class LoanApplication
    {
        public int LoanApplicationId { get; set; }
        public string ApplicantName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public double Income { get; set; }
        public string EmploymentStatus { get; set; } 
        public string BankAccountNumber { get; set; }
        public double LoanAmount { get; set; }
        public double InterestRate { get; set; }
        public int LoanTermMonths { get; set; }  // Term in months
        public int CustomerAccountId { get; set; }//fk
        public int LoanTypeId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public CustomerAccount CustomersAccounts { get; set; }
        public LoanType LoanType { get; set; }
        public string RepaymentSchedule { get; set; }                                             
        public bool IsSecuredLoan { get; set; }
        public string CollateralDescription { get; set; }
        public string ApplicationStatus { get; set; }
        public Loan loan { get; set; }

    }

}
