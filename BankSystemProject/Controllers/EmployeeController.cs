using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _IEmployee;
        public EmployeeController(IEmployee _IEmployee)
        {
            this._IEmployee= _IEmployee;
        }

        //for specific Employee Role
        [HttpGet("GetDeletedEmployeeInfoByUsername/{username}")]
        public async Task<IActionResult> GetDeletedEmployeeInfoByUsername(string username)
        {
            var employeeInfo = await _IEmployee.GetEmployeeInfoByUsernameAsync(username,true);

            if (employeeInfo == null)
            {
                return NotFound(new { Message = "Employee not found." });
            }

            return Ok(employeeInfo);
        }
        [HttpGet("GetEmployeeInfo/{username}")]
        public async Task<IActionResult> GetEmployeeInfoByUsername(string username)
        {
            var employeeInfo = await _IEmployee.GetEmployeeInfoByUsernameAsync(username, false);

            if (employeeInfo == null)
            {
                return NotFound(new { Message = "Employee not found." });
            }

            return Ok(employeeInfo);
        }

        //for specific Employee Role
        [HttpPut("UpdateEmployeeByUserNameInfo/{UserName}")]
        public async Task<IActionResult> UpdateEmployeeByUserNameInfo(string UserName, [FromForm] Req_UpdateEmployeeInfoDto updateDto)
        {
            var isUpdated = await _IEmployee.UpdateEmployeeInfoByUserNameAsync(UserName, updateDto);

            if (!isUpdated)
            {
                return NotFound(new { Message = "Employee not found or update failed." });
            }

            return Ok(new { Message = "Employee information updated successfully." });
        }

        //for Employee Admin
        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            var employees = await _IEmployee.GetAllEmployeesAsync();

            if (employees == null || !employees.Any())
                return NotFound("No employees found.");

            return Ok(employees);
        }

        //for Employee Admin
        [HttpPut("UpdateEmployeeByIDInfo/{EmployeeID}")]
        public async Task<IActionResult> UpdateEmployeeByIDInfo(int EmployeeID, [FromForm] Req_UpdateEmployeeInfoByAdminDto updateDto)
        {
            var isUpdated = await _IEmployee.UpdateEmployeeInfoByIDAsync(EmployeeID, updateDto);

            if (!isUpdated)
            {
                return NotFound(new { Message = "Employee not found or update failed." });
            }

            return Ok(new { Message = "Employee information updated successfully." });
        }

        //for Employee Admin
        [HttpDelete("DeleteEmployee/{UserID}")]
        public async Task<IActionResult> SoftDeleteEmployeeAsync(string UserID)
        {
            var result = await _IEmployee.SoftDeleteEmployeeAsync(UserID);

            if (!result)
            {
                return NotFound($"Employee with ID {UserID} not found.");
            }

            return Ok("Employee and associated user deleted successfully (soft delete).");
        }

    }
}
