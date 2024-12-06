namespace BankSystemProject.Model
{
    public class LoanRepayment
    {
        public int LoanRepaymentId { get; set; }
        public int LoanId { get; set; }
        public double AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public double RemainingBalance { get; set; }

        public Loan Loan { get; set; }


    }


}
