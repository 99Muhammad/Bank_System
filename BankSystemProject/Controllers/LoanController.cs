using BankSystemProject.Model;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using BankSystemProject.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoan _loanService;

        public LoanController(ILoan loanService)
        {
            _loanService = loanService;
        }

        [HttpGet("GetAllLoans")]
        public async Task<IActionResult> GetAllLoans()
        {
            var loans = await _loanService.GetAllLoansAsync();
            return Ok(loans);
        }

        [HttpGet("GetLoanByoanID/{LoanID}")]
        public async Task<IActionResult> GetLoanByoanID(int LoanID)
        {
            var loan = await _loanService.GetLoanByIdAsync(LoanID);
            if (loan == null) return NotFound("Loan not found.");
            return Ok(loan);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(Loan loan)
        //{
        //    await _loanService.CreateLoanAsync(loan);
        //    return CreatedAtAction(nameof(GetLoanByoanID), new { id = loan.LoanId }, loan);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, Loan loan)
        //{
        //    try
        //    {
        //        await _loanService.UpdateLoanAsync(id, loan);
        //        return NoContent();
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound("Loan not found.");
        //    }
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _loanService.DeleteLoanAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Loan not found.");
            }
        }


        [HttpGet("GetLoansByStatus")]
        public async Task<IActionResult> GetLoansByStatus([FromQuery] enLoanAndApplicationStatus status)
        {
            var loans = await _loanService.GetLoansByStatusAsync(status);

            if (loans == null || !loans.Any())
            {
                return NotFound(new { message = "No loans found with the given status." });
            }

            return Ok(loans);
        }
    }
}
