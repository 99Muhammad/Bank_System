using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoanRepaymentController : ControllerBase
    {
        private readonly IRepaymentLoan _repaymentService;

        public LoanRepaymentController(IRepaymentLoan repaymentService)
        {
            _repaymentService = repaymentService;
        }

        [HttpGet("{loanId}")]
        [Authorize(Roles = "SystemAdministrator,LoanOfficer")]

        public async Task<IActionResult> GetRepaymentsByLoanId(int loanId)
        {
            var repayments = await _repaymentService.GetRepaymentsByLoanIdAsync(loanId);
            return Ok(repayments);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SystemAdministrator,LoanOfficer")]

        public async Task<IActionResult> GetRepaymentById(int id)
        {
            var repayment = await _repaymentService.GetRepaymentByIdAsync(id);
            if (repayment == null) return NotFound("Repayment not found.");
            return Ok(repayment);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateLoanRepayment(LoanRepayment repayment)
        //{
        //    await _repaymentService.CreateLoanRepaymentAsync(repayment);
        //    return CreatedAtAction(nameof(GetRepaymentById), new { id = repayment.LoanRepaymentId }, repayment);
        //}
        [HttpGet("GetAllRepayments")]
        [Authorize(Roles = "SystemAdministrator,LoanOfficer")]
        public async Task<ActionResult<List<Req_LoanRepaymentForGetAllDto>>> GetAllRepayments()
        {
            try
            {
                var repayments = await _repaymentService.GetAllRepaymentsAsync();
                return Ok(repayments);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching loan repayments: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ProcessLoanRepayment([FromBody] Req_LoanRepaymentDto repaymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _repaymentService.ProcessLoanRepaymentAsync(repaymentDto);

            if (result.Message.Contains("not found") || result.Message.Contains("exceeds"))
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpGet("{accountNumber}")]
       [Authorize(Roles = "SystemAdministrator,LoanOfficer,Customer")]
        public async Task<ActionResult<List<Req_LoanRepaymentForGetAllDto>>> GetRepaymentsByAccountNumber(string accountNumber)
        {
            var repayments = await _repaymentService.GetRepaymentsByAccountNumberAsync(accountNumber);

            if (repayments == null || repayments.Count == 0)
            {
                return NotFound(new { Message = "No repayments found for the specified account number." });
            }

            return Ok(repayments);
        }
    }
}
