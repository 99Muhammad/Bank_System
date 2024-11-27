using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface IAccount
    {
        Task<Res_Registration> RegisterUserAsync(Req_Registration registerDto);

        Task<string> LoginAsync(Req_Login loginDto);
    }
}
