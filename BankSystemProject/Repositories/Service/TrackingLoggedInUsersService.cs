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

        public async Task<List<Res_LoggedInUsersDto>> GetLoggedInUsersByUserNameAsync(string userName)
        {
            var loggedInUsers = await _dbContext.TrackingLoggedInUsers
                .Include(tlu => tlu.users)
                .Where(tlu => tlu.users.UserName.Contains(userName))
                .OrderByDescending(tlu => tlu.LoginTime)
                .Select(tlu => new Res_LoggedInUsersDto
                {
                    Username = tlu.users.UserName,
                    Email = tlu.users.Email,
                    Role = tlu.users.Role,
                    LoginTime = tlu.LoginTime
                })
                .ToListAsync();

            return loggedInUsers;
        }

        public async Task<List<Res_LoggedInUsersDto>> GetLoggedInUsersByDateAsync(DateTime loginDate)
        {
            var loggedInUsers = await _dbContext.TrackingLoggedInUsers
                .Include(tlu => tlu.users)
                .Where(tlu => tlu.LoginTime.Date == loginDate.Date)
                .OrderByDescending(tlu => tlu.LoginTime)
                .Select(tlu => new Res_LoggedInUsersDto
                {
                    Username = tlu.users.UserName,
                    Email = tlu.users.Email,
                    Role = tlu.users.Role,
                    LoginTime = tlu.LoginTime
                })
                .ToListAsync();

            return loggedInUsers;
        }

        public async Task<List<Res_LoggedInUsersDto>> GetLoggedInUsersByRoleAsync(string role)
        {
            var loggedInUsers = await _dbContext.TrackingLoggedInUsers
                .Include(tlu => tlu.users)
                .Where(tlu => tlu.users.Role.Contains(role))
                .OrderByDescending(tlu => tlu.LoginTime)
                .Select(tlu => new Res_LoggedInUsersDto
                {
                    Username = tlu.users.UserName,
                    Email = tlu.users.Email,
                    Role = tlu.users.Role,
                    LoginTime = tlu.LoginTime
                })
                .ToListAsync();

            return loggedInUsers;
        }

        public async Task<List<Res_LoggedInUsersDto>> GetLoggedInUsersLastMonthAsync()
        {
            var firstDayOfLastMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
            var lastDayOfLastMonth = firstDayOfLastMonth.AddMonths(1).AddDays(-1);

            var loggedInUsers = await _dbContext.TrackingLoggedInUsers
                .Include(tlu => tlu.users)
                .Where(tlu => tlu.LoginTime >= firstDayOfLastMonth && tlu.LoginTime <= lastDayOfLastMonth)
                .OrderByDescending(tlu => tlu.LoginTime)
                .Select(tlu => new Res_LoggedInUsersDto
                {
                    Username = tlu.users.UserName,
                    Email = tlu.users.Email,
                    Role = tlu.users.Role,
                    LoginTime = tlu.LoginTime
                })
                .ToListAsync();

            return loggedInUsers;
        }
    }
    
    
}
