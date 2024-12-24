using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using BankSystemProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingLoggedInUsersController : ControllerBase
    {
        private readonly ITrackingLoggedInUsers _trackingUsers;

        public TrackingLoggedInUsersController(ITrackingLoggedInUsers trackingUsers)
        {
            _trackingUsers = trackingUsers;
        }
        
        [Authorize(Roles = "SystemAdministrator")]
        [HttpGet("GetAllLoggedInUsersAsync")]
        public async Task<IActionResult> GetAllLoggedInUsersAsync()
        {
            var users = await _trackingUsers.GetAllLoggedInUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "SystemAdministrator")]
        [HttpGet("by-username")]
        public async Task<ActionResult<List<Res_LoggedInUsersDto>>> GetLoggedInUsersByUserName([FromQuery] string userName)
        {
            var result = await _trackingUsers.GetLoggedInUsersByUserNameAsync(userName);

            if (result == null || result.Count == 0)
            {
                return NotFound("No users found with the provided username.");
            }

            return Ok(result);
        }

        [Authorize(Roles = "SystemAdministrator")]
        [HttpGet("by-date")]
        public async Task<ActionResult<List<Res_LoggedInUsersDto>>> GetLoggedInUsersByDate([FromQuery] DateTime loginDate)
        {
            var result = await _trackingUsers.GetLoggedInUsersByDateAsync(loginDate);

            if (result == null || result.Count == 0)
            {
                return NotFound("No users found for the provided date.");
            }

            return Ok(result);
        }

        [Authorize(Roles = "SystemAdministrator")]
        [HttpGet("by-role")]
        public async Task<ActionResult<List<Res_LoggedInUsersDto>>> GetLoggedInUsersByRole([FromQuery] string role)
        {
            var result = await _trackingUsers.GetLoggedInUsersByRoleAsync(role);

            if (result == null || result.Count == 0)
            {
                return NotFound("No users found with the provided role.");
            }

            return Ok(result);
        }

        [Authorize(Roles = "SystemAdministrator")]
        [HttpGet("by-last-month")]
        public async Task<ActionResult<List<Res_LoggedInUsersDto>>> GetLoggedInUsersLastMonth()
        {
            var result = await _trackingUsers.GetLoggedInUsersLastMonthAsync();

            if (result == null || result.Count == 0)
            {
                return NotFound("No users found for the last month.");
            }

            return Ok(result);
        }

    }
}
