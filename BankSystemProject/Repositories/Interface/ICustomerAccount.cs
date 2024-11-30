using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface ICustomerAccount
    {
        Task<List<Res_CustomersAccounts>> GetCustomersAccountsInfoAsync();
        Task<Res_UpdateAccTypeInCustomerAcc> UpdateAccTypeInCustomerAccByUserID(Req_UpdateAccTypeInCustomerAcc req_UpdateAccTypeIn,string UserID);
        Task<Res_UpdateAccTypeInCustomerAcc> GetAccountTypeNameByUserID( string UserID);
        Task<Res_CustomersAccounts> GetAccountInfoByAccountNum(string AccountNum);
    }
}
