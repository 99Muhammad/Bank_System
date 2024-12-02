namespace BankSystemProject.Model
{
    public class LoanType
    {
        public int LoanTypeId { get; set; }
        public string LoanTypeName { get; set; }
        public double InterestRate { get; set; }
        public string Description { get; set; }

        public int LoanTermMonths { get; set;}

        public ICollection<Loan> Loans { get; set; }
        public ICollection<LoanApplication> LoanApplications { get; set; }
    }

}
