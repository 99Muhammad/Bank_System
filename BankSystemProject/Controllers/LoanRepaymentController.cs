using BankSystemProject.Model;
using BankSystemProject.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanRepaymentController : ControllerBase
    {
        private readonly IRepaymentLoan _repaymentService;

        public LoanRepaymentController(IRepaymentLoan repaymentService)
        {
            _repaymentService = repaymentService;
        }

        [HttpGet("loan/{loanId}")]
        public async Task<IActionResult> GetRepaymentsByLoanId(int loanId)
        {
            var repayments = await _repaymentService.GetRepaymentsByLoanIdAsync(loanId);
            return Ok(repayments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var repayment = await _repaymentService.GetRepaymentByIdAsync(id);
            if (repayment == null) return NotFound("Repayment not found.");
            return Ok(repayment);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LoanRepayment repayment)
        {
            await _repaymentService.CreateLoanRepaymentAsync(repayment);
            return CreatedAtAction(nameof(GetById), new { id = repayment.LoanRepaymentId }, repayment);
        }
    }
}
