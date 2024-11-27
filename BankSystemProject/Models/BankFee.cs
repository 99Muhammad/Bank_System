namespace BankSystemProject.Model
{
    public class BankFee
    {
        public int BankFeeId { get; set; }
        public string FeeType { get; set; }
        public double Amount { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }

}
