namespace BankSystemProject.Model
{
    public class LoanApplication
    {
        public int LoanApplicationId { get; set; }
        public int CustomerAccountId { get; set; }
        public int LoanTypeId { get; set; }
        public decimal AmountRequested { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
        public DateTime? ApprovalDate { get; set; }

        public CustomerAccount CustomerAccount { get; set; }
        public LoanType LoanType { get; set; }
    }

}
