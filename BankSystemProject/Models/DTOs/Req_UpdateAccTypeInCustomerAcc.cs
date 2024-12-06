using BankSystemProject.Shared.Enums;
using System.Runtime.Serialization;

namespace BankSystemProject.Models.DTOs
{
    public class Req_UpdateAccTypeInCustomerAcc
    {
       // public int customerAccountID { get; set; }
        public enAccountType AccountTypeName { get; set; }
        public string OldAccountNumber { get; set; }
        public string NewAccountNumber { get; set; }
        public string ConfirmNewAccountNumber { get; set; }

    }
}
