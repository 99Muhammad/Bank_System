namespace BankSystemProject.Models.DTOs
{
    public class Res_GetCreditCardInfoDto
    {
        public int CreditCardId { get; set; }
        // public int CustomerAccountId { get; set; }
        public string CardHolderName { get; set; }
        public string CardType { get; set; }
        public double CreditLimit { get; set; }
        public double Balance { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
