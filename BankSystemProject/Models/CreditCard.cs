namespace BankSystemProject.Model
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }
        public int CustomerAccountId { get; set; }
        public string CardType { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal Balance { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }

        public CustomerAccount CustomerAccount { get; set; }
    }


}
