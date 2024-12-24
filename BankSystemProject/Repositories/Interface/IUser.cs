using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Shared.Enums;

namespace BankSystemProject.Repositories.Interface
{
    public interface IUser
    {
        Task<List<Res_UsersInfo>> FilterUsersByRoleAsync(enUserRole roleName);
        Task<List<Res_UsersInfo>> GetAllUsersAsync();
        Task<Res_UsersInfo> SearchUserByFullNameOrUserNameAsync(string name);
        Task<Res_UsersInfo> GetUserByIdAsync(string userId);
        //Task<bool> DeleteUserAsync(string userID);
        Task<bool> SoftDeleteCustomerUserAsync(string userId);
        Task<bool> UpdateUserAsync(string userID, Req_UpdateUserInfo updateUserDto);




    }
}
