using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using BankSystemProject.Repositories.Service.AdminServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomersAccountsController : ControllerBase
    {

        private readonly ICustomerAccount _IcustomerAccount;

        public CustomersAccountsController(ICustomerAccount _IcustomerAccount)
        {
            this._IcustomerAccount = _IcustomerAccount;
        }

        [Authorize(Roles = "SystemAdministrator,BranchManager")]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomerAccounts()
        {
            var accounts = await _IcustomerAccount.GetCustomersAccountsInfoAsync(false);
            if (!accounts.Any())
            {
                return NotFound("No customer accounts found.");
            }
            return Ok(accounts);
        }

        [Authorize(Roles = "SystemAdministrator,BranchManager")]
        [HttpGet("GetAllDeletedCustomersAccounts")]
        public async Task<IActionResult> GetAllDeletedCustomersAccounts()
        {
            var accounts = await _IcustomerAccount.GetCustomersAccountsInfoAsync(true);
            if (!accounts.Any())
            {
                return NotFound("No customer accounts found.");
            }
            return Ok(accounts);
        }

        [Authorize("Customer")]
        [HttpPut("Customer/{customerAccountID}")]
        public async Task<IActionResult> UpdateAccountInfo([FromForm] Req_UpdateAccTypeInCustomerAcc req_UpdateAccTypeIn,int customerAccountID)
        {
            try
            {
                var response = await _IcustomerAccount.UpdateAccountInfo(req_UpdateAccTypeIn,customerAccountID);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [Authorize(Roles = "SystemAdministrator,BranchManager,Customer")]
        [HttpGet("{customerAccountID}")]
        public async Task<IActionResult> GetAccountTypeNameByCustomerAccountID( int customerAccountID)
        {
            try
            {
                var response = await _IcustomerAccount.GetAccountTypeNameBycustomerAccountID( customerAccountID);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "SystemAdministrator,BranchManager,Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAccountInfoByAccountNumber(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Account number cannot be null or empty."
                });
            }

            try
            {
                var accountInfo = await _IcustomerAccount.GetAccountInfoByAccountNum(accountNumber);

                if (accountInfo == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "No account information found for the given account number."
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Account information retrieved successfully.",
                    Data = accountInfo
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Log the exception here using a logging framework like Serilog, NLog, etc.
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An internal server error occurred. Please try again later.",
                    Details = ex.Message // Remove this in production for security reasons.
                });
            }
        }

        [Authorize(Roles = "SystemAdministrator,BranchManager,Customer")]
        [HttpPut("Customer/{customerAccountID}")]
        public async Task<IActionResult> UpdateCustomerInfo([FromForm] Req_UpdateCustomerInfoDto request, int customerAccountID)
        {
            try
            {
                var result = await _IcustomerAccount.UpdateCustomerInfoAsync(request, customerAccountID);
                if (result)
                {
                    return Ok(new { Message = "User information updated successfully." });
                }

                return BadRequest(new { Message = "Failed to update user information." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating user information.", Details = ex.Message });
            }
        }

        [Authorize(Roles = "SystemAdministrator,BranchManager")]
        [HttpDelete("{customerAccountID}")]
        public async Task<IActionResult> DeleteCustomerAccountById(int customerAccountID)
        {
            try
            {
                var result = await _IcustomerAccount.DeleteCustomerAccountAsync(customerAccountID);
                if (result)
                {
                    return Ok(new { Message = "Customer account deleted successfully." });
                }

                return BadRequest(new { Message = "Failed to delete customer account." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the customer account.", Details = ex.Message });
            }
        }
    }
}
