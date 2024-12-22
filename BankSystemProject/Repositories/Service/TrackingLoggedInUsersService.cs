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


       public async Task<List<Res_LoggedInUsersDto>> GetAllLoggedInUsersAsync()
        {
            var loggedInUsers = _dbContext.TrackingLoggedInUsers
              .Include(tlu => tlu.users) 
              .Select(tlu => new Res_LoggedInUsersDto
              {
                  Username = tlu.users.UserName,
                  Email = tlu.users.Email,
                  Role = tlu.users.Role,
                  LoginTime = tlu.LoginTime
              }).ToList();

            return loggedInUsers;
        }
    }
    
    
}
