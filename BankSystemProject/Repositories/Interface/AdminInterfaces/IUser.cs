using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface.AdminInterfaces
{
    public interface IUser
    {
        Task<List<Res_UsersInfo>> FilterUsersByRoleAsync(string roleName);
        Task<List<Res_UsersInfo>>GetAllUsersAsync();
        Task<Res_UsersInfo> SearchUserByFullNameOrUserNameAsync(string name);
        Task<Res_UsersInfo> GetUserByIdAsync(string userId);
        //Task<bool> DeleteUserAsync(string userID);
        Task<bool> SoftDeleteUserAsync(string userId);
        Task<bool> UpdateUserAsync(string userID, Req_UpdateUserInfo updateUserDto);




    }
}
