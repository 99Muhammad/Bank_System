namespace BankSystemProject.Model
{
    public class AccountType
    {
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string Description { get; set; }

        public ICollection<CustomerAccount> CustomerAccounts { get; set; }
    }

}
