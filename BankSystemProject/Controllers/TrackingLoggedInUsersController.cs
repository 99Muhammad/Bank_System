using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service.AdminServices;
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

        [HttpGet("GetAllLoggedInUsersAsync")]
        public async Task<IActionResult> GetAllLoggedInUsersAsync()
        {
            var users = await _trackingUsers.GetAllLoggedInUsersAsync();
            return Ok(users);
        }
    }
}
