namespace BankSystemProject.Model
{
    public class TransactionsDepWi
    {

        public int TransactionId { get; set; }
        public int CustomerAccountId { get; set; }
        public string TransactionType { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? BankFeeId { get; set; }

        public CustomerAccount customerAccount { get; set; }
        public BankFee BankFee { get; set; }
    }

}
