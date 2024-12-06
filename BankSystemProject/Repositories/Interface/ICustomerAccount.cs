using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface ICustomerAccount
    {
        Task<List<Res_CustomersAccounts>> GetCustomersAccountsInfoAsync(bool includeDeleted);
        Task<Res_UpdateAccTypeInCustomerAcc> UpdateAccountInfo(Req_UpdateAccTypeInCustomerAcc req_UpdateAccTypeIn, int customerAccountID);
        Task<Res_UpdateAccTypeInCustomerAcc> GetAccountTypeNameBycustomerAccountID( int customerAccountID);
        Task<Res_CustomersAccounts> GetAccountInfoByAccountNum(string AccountNum);
        Task<bool> UpdateCustomerInfoAsync(Req_UpdateCustomerInfoDto updateInfo,int CustomerAccountID);
        Task<bool> DeleteCustomerAccountAsync(int CustomerAccountID);
    }
}
