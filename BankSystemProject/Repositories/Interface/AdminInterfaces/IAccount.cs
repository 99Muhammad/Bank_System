using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface.AdminInterfaces
{
    public interface IAccount
    {
        Task<Res_Registration> RegisterUserAsync(Req_Registration registerDto);

        Task<Users> LoginAsync(Req_Login loginDto);
    }
}
