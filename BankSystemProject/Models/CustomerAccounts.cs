using System.Transactions;

namespace BankSystemProject.Model
{
    public class CustomerAccount
    {
        public int CustomerAccountId { get; set; }
        public string UserId { get; set; }
        public int AccountTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AccountNumber { get; set; }
        public bool IsDeleted { get; set; }
        public Users User { get; set; }
        public AccountType AccountType { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Loan> Loans { get; set; }
        public ICollection<CreditCard> CreditCards { get; set; }
        public ICollection<LoanApplication> LoanApplications { get; set; }
    }

}
