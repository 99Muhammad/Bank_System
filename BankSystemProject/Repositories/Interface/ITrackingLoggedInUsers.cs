using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Service;

namespace BankSystemProject.Repositories.Interface
{
    public interface ITrackingLoggedInUsers
    {
       Task<List<Res_LoggedInUsersDto>> GetAllLoggedInUsersAsync();
        Task<List<Res_LoggedInUsersDto>> GetLoggedInUsersByUserNameAsync(string userName);
        Task<List<Res_LoggedInUsersDto>> GetLoggedInUsersByDateAsync(DateTime loginDate);
        Task<List<Res_LoggedInUsersDto>> GetLoggedInUsersByRoleAsync(string role);
        Task<List<Res_LoggedInUsersDto>> GetLoggedInUsersLastMonthAsync();
    }
}
