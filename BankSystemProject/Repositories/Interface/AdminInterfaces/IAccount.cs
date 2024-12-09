using BankSystemProject.Model;
using BankSystemProject.Models;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface.AdminInterfaces
{
    public interface IAccount
    {
        Task<Res_Registration> RegisterUserAsync(Req_Registration registerDto);
         Task<Res_TokenDto> RefreshTokensAsync(string refreshToken)
        Task<Res_TokenDto> LoginAsync(Req_Login loginDto);
    }
}
