namespace BankSystemProject.Model
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int CustomerAccountId { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? BankFeeId { get; set; }

        public CustomerAccount CustomerAccount { get; set; }
        public BankFee BankFee { get; set; }
    }

}
