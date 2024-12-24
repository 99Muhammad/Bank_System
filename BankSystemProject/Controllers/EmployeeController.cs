using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _IEmployee;
        public EmployeeController(IEmployee _IEmployee)
        {
            this._IEmployee= _IEmployee;
        }

        [Authorize(Roles = "SystemAdministrator,BranchManager")]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetDeletedEmployeeInfoByUsername(string username)
        {
            var employeeInfo = await _IEmployee.GetEmployeeInfoByUsernameAsync(username,true);

            if (employeeInfo == null)
            {
                return NotFound(new { Message = "Employee not found." });
            }

            return Ok(employeeInfo);
        }

        [Authorize(Roles = "SystemAdministrator,BranchManager")]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetEmployeeInfoByUsername(string username)
        {
            var employeeInfo = await _IEmployee.GetEmployeeInfoByUsernameAsync(username, false);

            if (employeeInfo == null)
            {
                return NotFound(new { Message = "Employee not found." });
            }

            return Ok(employeeInfo);
        }

        [Authorize(Roles = "SystemAdministrator,BranchManager,teller,CreditCardOfficer," +
            "LoanOfficer")]
        [HttpPut("employee/{UserName}")]
        public async Task<IActionResult> UpdateEmployeeInfoByUserName(string UserName, [FromForm] Req_UpdateEmployeeInfoDto updateDto)
        {
            var isUpdated = await _IEmployee.UpdateEmployeeInfoByUserNameAsync(UserName, updateDto);

            if (!isUpdated)
            {
                return NotFound(new { Message = "Employee not found or update failed." });
            }

            return Ok(new { Message = "Employee information updated successfully." });
        }

       [Authorize(Roles = "SystemAdministrator,BranchManager")]
        [HttpGet]
        public async Task<IActionResult> GetAllDeletedEmployeesAsync()
        {
            var employees = await _IEmployee.GetAllEmployeesAsync(true);

            if (employees == null || !employees.Any())
                return NotFound("No employees found.");

            return Ok(employees);
        }

         [Authorize(Roles = "SystemAdministrator,BranchManager")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            var employees = await _IEmployee.GetAllEmployeesAsync(false);

            if (employees == null || !employees.Any())
                return NotFound("No employees found.");

            return Ok(employees);
        }

        [Authorize(Roles = "SystemAdministrator,BranchManager")]
        [HttpPut("Admin/{EmployeeID}")]
        public async Task<IActionResult> UpdateEmployeeByIDInfo(int EmployeeID, [FromForm] Req_UpdateEmployeeInfoByAdminDto updateDto)
        {
            var isUpdated = await _IEmployee.UpdateEmployeeInfoByIDAsync(EmployeeID, updateDto);

            if (!isUpdated)
            {
                return NotFound(new { Message = "Employee not found or update failed." });
            }

            return Ok(new { Message = "Employee information updated successfully." });
        }

        [Authorize(Roles = "SystemAdministrator,BranchManager")]
        [HttpDelete("{UserID}")]
        public async Task<IActionResult> DeleteEmployeeAsync(string UserID)
        {
            var result = await _IEmployee.DeletedEmployeeAsync(UserID);

            if (!result)
            {
                return NotFound($"Employee with ID {UserID} not found.");
            }

            return Ok("Employee and associated user deleted successfully (soft delete).");
        }

    }
}
