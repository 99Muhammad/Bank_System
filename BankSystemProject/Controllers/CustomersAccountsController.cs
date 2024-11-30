using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersAccountsController : ControllerBase
    {

        private readonly ICustomerAccount _IcustomerAccount;

        public CustomersAccountsController(ICustomerAccount _IcustomerAccount)
        {
            this._IcustomerAccount = _IcustomerAccount;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerAccounts()
        {
            var accounts = await _IcustomerAccount.GetCustomersAccountsInfoAsync();
            if (!accounts.Any())
            {
                return NotFound("No customer accounts found.");
            }
            return Ok(accounts);
        }

        [HttpPut("UpdateAccTypeNameInCustomerAccByUserID/{UserID}")]
        public async Task<IActionResult> UpdateAccTypeNameInCustomerAccByUserID([FromForm] Req_UpdateAccTypeInCustomerAcc req_UpdateAccTypeIn,string UserID)
        {
            try
            {
                var response = await _IcustomerAccount.UpdateAccTypeInCustomerAccByUserID(req_UpdateAccTypeIn,UserID);
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

        [HttpGet("GetAccountTypeNameByUserID/{UserID}")]
        public async Task<IActionResult> GetAccountTypeNameByUserID( string UserID)
        {
            try
            {
                var response = await _IcustomerAccount.GetAccountTypeNameByUserID( UserID);
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

        [HttpGet("account-infoByAccountNumber")]
        //[Authorize]
        public async Task<IActionResult> GetAccountInfo(string accountNumber)
        {
            try
            {
                var accountInfo = await _IcustomerAccount.GetAccountInfoByAccountNum(accountNumber);
                return Ok(accountInfo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
