using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface ITrackingLoggedInUsers
    {
        //Task<List<Res_LoggedInUsersDto>> GetFilteredUsers(string UserName );
        Task<List<Res_LoggedInUsersDto>> GetAllLoggedInUsersAsync();

    }
}
