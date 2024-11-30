using BankSystemProject.Shared.Enums;

namespace BankSystemProject.Models.DTOs
{
    public class Req_ImplementTransactionDto
    {
        public string AccountNumber { get; set; }
        public enTransactionType transactionType { get; set; }
        public double transactionAmount { get; set; }
    }
}
