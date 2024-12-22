using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypeController : ControllerBase
    {
        private readonly IAccountType _accountTypeService;

        public AccountTypeController(IAccountType accountTypeService)
        {
            _accountTypeService = accountTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccountTypes()
        {
            var result = await _accountTypeService.GetAllAccountTypesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountTypeById(int id)
        {
            var result = await _accountTypeService.GetAccountTypeByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountType([FromBody] Req_AccountTypeDto accountTypeDto)
        {
            var result = await _accountTypeService.CreateAccountTypeAsync(accountTypeDto);
            return CreatedAtAction(nameof(GetAccountTypeById), new { id = result.AccountTypeId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountType(int id, [FromBody] Req_AccountTypeDto accountTypeDto)
        {
            var result = await _accountTypeService.UpdateAccountTypeAsync(id, accountTypeDto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountType(int id)
        {
            var result = await _accountTypeService.DeleteAccountTypeAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
