namespace BankSystemProject.Model
{
    public class TransferInfo
    {
        public int TransferInfoId { get; set; }  // Primary Key
        public string UserId { get; set; }          // Foreign Key to Users
        public string AccountNumTransferFrom { get; set; }
        public string AccountNumTransferTo { get; set; }
        public double BalanceFromBeforeTransfer { get; set; }
        public double BalanceToBeforeTransfer { get; set; }
        public DateTime DateTimeTransfer { get; set; }
        public double BalanceFromAfterTransfer { get; set; }
        public double BalanceToAfterTransfer { get; set; }

        public Users User { get; set; }           // Navigation property to Users
    }

}
