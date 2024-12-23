using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface IAccountType
    {
        Task<List<Res_AccountTypeDto>> GetAllAccountTypesAsync();
        Task<Res_AccountTypeDto> GetAccountTypeByIdAsync(int accountTypeId);
        Task<Res_AccountTypeDto> CreateAccountTypeAsync(Req_AccountTypeDto accountTypeDto);
        Task<Res_AccountTypeDto> UpdateAccountTypeAsync(int accountTypeId, Req_AccountTypeDto accountTypeDto);
        Task<bool> DeleteAccountTypeAsync(int accountTypeId);
    }
}
