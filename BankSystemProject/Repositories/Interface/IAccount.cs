using BankSystemProject.Model;
using BankSystemProject.Models;
using BankSystemProject.Models.DTOs;
using System.Security.Claims;

namespace BankSystemProject.Repositories.Interface
{
    public interface IAccount
    {
        Task<Res_Registration> RegisterUser(Req_Registration registerDto);
        Task<Res_TokenDto> RefreshTokensAsync(string refreshToken);
        Task<Res_TokenDto> LoginAsync(Req_Login loginDto);
        Task<bool> ConfirmEmailAsync(string userId, string token);
        //Task<bool> ResetPasswordAsync(ResetPasswordReqDTO resetPasswordDto);
        public Task Logout(ClaimsPrincipal userPrincipal);
        Task<bool> ForgotPasswordAsync(ForgotPasswordReqDTO forgotPasswordDto);
    }
}
