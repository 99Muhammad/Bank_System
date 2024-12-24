using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoanTypeController : ControllerBase
    {
        private readonly ILoanType _loanTypeService;

        public LoanTypeController(ILoanType loanTypeService)
        {
            _loanTypeService = loanTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLoanTypes()
        {
            var result = await _loanTypeService.GetAllLoanTypesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanTypeById(int id)
        {
            var result = await _loanTypeService.GetLoanTypeByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoanType([FromBody] Req_LoanTypeDto loanTypeDto)
        {
            var result = await _loanTypeService.CreateLoanTypeAsync(loanTypeDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoanType(int id, [FromBody] Req_LoanTypeDto loanTypeDto)
        {
            var result = await _loanTypeService.UpdateLoanTypeAsync(id, loanTypeDto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoanType(int id)
        {
            var result = await _loanTypeService.DeleteLoanTypeAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
