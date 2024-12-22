using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Service;

namespace BankSystemProject.Repositories.Interface
{
    public interface ITrackingLoggedInUsers
    {
        //Task<List<Res_LoggedInUsersDto>> GetFilteredUsers(string UserName );
       Task<List<Res_LoggedInUsersDto>> GetAllLoggedInUsersAsync();
       // Task<TrackingLoggedInUsersService> GetAllLoggedInUsersAsync();
    }
}
