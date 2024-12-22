using BankSystemProject.Data;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using Mailjet.Client.Resources;
using Microsoft.EntityFrameworkCore;
using System;

namespace BankSystemProject.Repositories.Service
{
    public class TrackingLoggedInUsersService : ITrackingLoggedInUsers
    {
        private readonly Bank_DbContext _dbContext; 
        public TrackingLoggedInUsersService(Bank_DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task<Res_LoggedInUsersDto>GetFilteredUsers(string UserName)
        //{

        //    //var query = _dbContext.TrackingLoggedInUsers.AsQueryable();

        //    var User = _dbContext.TrackingLoggedInUsers
        //        .Include(u => u.users)
        //        .Where(u => u.users.UserName == UserName);

        //    if (User == null)
        //        return null!;

        //    //// Filter by role
        //    //if (!string.IsNullOrEmpty(filterRequest.Role))
        //    //{
        //    //    User = User.Where(u => u.users.Role.Equals(filterRequest.Role, StringComparison.OrdinalIgnoreCase));
        //    //}

        //    ////// Filter by login time
        //    ////if (filterRequest.LoginAfter!=null)
        //    ////{
        //    ////    query = query.Where(u => u.LoginTime >= filterRequest.LoginAfter.Value);
        //    ////}

        //    //// Search by username or email
        //    //if (!string.IsNullOrEmpty(filterRequest.SearchTerm))
        //    //{
        //    //    query = query.Where(u =>
        //    //        u.users.UserName.Contains(filterRequest.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
        //    //        u.users.Email.Contains(filterRequest.SearchTerm, StringComparison.OrdinalIgnoreCase));
        //    //}

        //    // Fetch the filtered data
        //    var users =  User
        //        .Select(u => new Res_LoggedInUsersDto
        //        {
        //            Username = u.users.UserName,
        //            Email = u.users.Email,
        //            Role = u.users.Role,
        //            //LoginTime = u.LoginTime
        //        });
               

        //    return users;
        //}



        // This method will get all logged-in users from the TrackingUsers table
        public async Task<List<Res_LoggedInUsersDto>> GetAllLoggedInUsersAsync()
        {
            // Querying all users from the TrackingUsers table
            var users = await _dbContext.TrackingLoggedInUsers
                .Select(u => new Res_LoggedInUsersDto
                {
                    Username = u.users.UserName,
                    Email = u.users.Email,
                    Role = u.users.Role,
                    //LoginTime = u.LoginTime
                })
                .ToListAsync();

            return users;
        }
    }
    
}
