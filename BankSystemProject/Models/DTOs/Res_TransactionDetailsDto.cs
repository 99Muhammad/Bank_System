namespace BankSystemProject.Models.DTOs
{
    public class Res_TransactionDetailsDto
    {
        public int TransactionId { get; set; }
        public string CustomerAccountName { get; set; }
        public string TransactionType { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
     
    }
}
