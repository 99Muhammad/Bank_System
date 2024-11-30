using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using BankSystemProject.Repositories.Service.AdminServices;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers.AdminControllers
{
    public class UserController:ControllerBase
    {
        private readonly IUser _IUser;

        public UserController(IUser _IUser)
        {
            this._IUser = _IUser;
        }


        [HttpGet("FilterUsersByRole/{roleName}")]
        public async Task<IActionResult> FilterUsersByRoleAsync(string roleName)
        {
            var users = await _IUser.FilterUsersByRoleAsync(roleName);
            if (users == null || !users.Any())
            {
                return NotFound($"No users found with role {roleName}");
            }
            return Ok(users);
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _IUser.GetAllUsersAsync();

            if (users == null || !users.Any())
                return NotFound("No users found.");

            return Ok(users);
        }

        [HttpGet("SearchUserByFullNameOrUserNameAsync/{name}")]
        public async Task<IActionResult> SearchUserByFullNameOrUserNameAsync(string name)
        {
            var user = await _IUser.SearchUserByFullNameOrUserNameAsync(name);
            if (user==null)
            {
                return NotFound($"No users found with the name: {name}");
            }
            return Ok(user);
        }

        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID cannot be null or empty.");

            var user = await _IUser.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpDelete("DeleteUser/{userID}")]
        public async Task<IActionResult> DeleteUser(string userID)
        {
            var result = await _IUser.SoftDeleteUserAsync(userID);
            if (!result)
            {
                return NotFound($"User with ID: {userID} not found");
            }
       
            return Ok("Deleted done successfully");
            
        }

        [HttpPut("UpdateUserAsync/{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, [FromForm] Req_UpdateUserInfo updateUserDto)
        {
            var result = await _IUser.UpdateUserAsync(id, updateUserDto);
            if (!result)
            {
                return NotFound($"User with ID: {id} not found");
            }
            return Ok("User updated successfully");
        }
    }
}
