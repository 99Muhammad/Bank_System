using BankSystemProject.Model;
using Mailjet.Client.Resources;

namespace BankSystemProject.Models.DTOs
{
    public class Res_UpdateAccTypeInCustomerAcc  
    {
        public string FullName { get; set; }
        public string  AccountTypeName { get; set; }
        public string Message { get; set; }
        public string AccountNumber { get; set; }
        //public Users user { get; set; }
        //public AccountType accountType { get; set;}
    }
}
