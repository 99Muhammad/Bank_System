using BankSystemProject.Model;
using BankSystemProject.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationController : ControllerBase
    {
        private readonly ILoanApplication _loanApplicationService;

        public LoanApplicationController(ILoanApplication loanApplicationService)
        {
            _loanApplicationService = loanApplicationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var applications = await _loanApplicationService.GetAllAsync();
            return Ok(applications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var application = await _loanApplicationService.GetByIdAsync(id);
            if (application == null) return NotFound();
            return Ok(application);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] LoanApplication loanApplication)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdApplication = await _loanApplicationService.CreateAsync(loanApplication);
            return CreatedAtAction(nameof(GetById), new { id = createdApplication.LoanApplicationId }, createdApplication);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] LoanApplication loanApplication)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedApplication = await _loanApplicationService.UpdateAsync(id, loanApplication);
            if (updatedApplication == null) return NotFound();
            return Ok(updatedApplication);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loanApplicationService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
