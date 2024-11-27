namespace BankSystemProject.Model
{
    public class Loan
    {
        public int LoanId { get; set; }
        public int CustomerAccountId { get; set; }
        public int LoanTypeId { get; set; }
        public double LoanAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public CustomerAccount CustomerAccount { get; set; }
        public LoanType LoanType { get; set; }
        public ICollection<LoanRepayment> LoanRepayments { get; set; }
     
    }

}
