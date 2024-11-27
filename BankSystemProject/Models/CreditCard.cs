namespace BankSystemProject.Model
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }
        public int CustomerAccountId { get; set; }
        public string CardType { get; set; }
        public double CreditLimit { get; set; }
        public double Balance { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }

        public CustomerAccount CustomerAccount { get; set; }
    }


}
