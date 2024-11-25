namespace BankSystemProject.Model
{
    public class BankFee
    {
        public int BankFeeId { get; set; }
        public string FeeType { get; set; }
        public decimal Amount { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }

}
