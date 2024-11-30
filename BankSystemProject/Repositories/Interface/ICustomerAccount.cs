using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface ICustomerAccount
    {
        Task<List<Res_CustomersAccounts>> GetCustomerAccountsInfoAsync();
        Task<Res_UpdateAccTypeInCustomerAcc> UpdateAccTypeInCustomerAccByUserID(Req_UpdateAccTypeInCustomerAcc req_UpdateAccTypeIn,string UserID);
        Task<Res_UpdateAccTypeInCustomerAcc> GetAccountTypeNameByUserID( string UserID);

    }
}
