namespace BankSystemProject.Models.DTOs
{
    public class Req_TransferInfoDto
    {
        public string EnterYourPincCode { get; set; }
        public string AccountNumTransferFrom { get; set; }
        public string AccountNumTransferTo { get; set; }
        public double Amount { get; set; }
        //public double BalanceFromBeforeTransfer { get; set; }
        //public double BalanceFromAfterTransfer { get; set; }
        //public double BalanceToAfterTransfer { get; set; }
    }
}
