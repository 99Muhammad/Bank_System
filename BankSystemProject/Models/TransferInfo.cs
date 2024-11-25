namespace BankSystemProject.Model
{
    public class TransferInfo
    {
        public int TransferInfoId { get; set; }  // Primary Key
        public string UserId { get; set; }          // Foreign Key to Users
        public string AccountNumTransferFrom { get; set; }
        public string AccountNumTransferTo { get; set; }
        public decimal BalanceFromBeforeTransfer { get; set; }
        public decimal BalanceToBeforeTransfer { get; set; }
        public DateTime DateTimeTransfer { get; set; }
        public decimal BalanceFromAfterTransfer { get; set; }
        public decimal BalanceToAfterTransfer { get; set; }

        public Users User { get; set; }           // Navigation property to Users
    }

}
