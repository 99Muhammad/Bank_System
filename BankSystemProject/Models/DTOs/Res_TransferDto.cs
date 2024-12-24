namespace BankSystemProject.Models.DTOs
{
    public class Res_TransferDto
    {
        public int TransferInfoId { get; set; }
        public int CustomerAccountID { get; set; }
        public string AccountNumTransferFrom { get; set; }
        public string AccountNumTransferTo { get; set; }
        public double BalanceFromBeforeTransfer { get; set; }
        public double BalanceToBeforeTransfer { get; set; }
        public DateTime DateTimeTransfer { get; set; }
        public double BalanceFromAfterTransfer { get; set; }
        public double BalanceToAfterTransfer { get; set; }
    }
}
