using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Shared.Enums;
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

        [HttpGet("GetAllApplications")]
        public async Task<IActionResult> GetAllApplications()
        {
            var applications = await _loanApplicationService.GetAllApplicationsAsync();
            return Ok(applications);
        }

        [HttpGet("GetApplicationById/{ApplicationID}")]
        public async Task<IActionResult> GetApplicationById(int ApplicationID)
        {
            var application = await _loanApplicationService
                .GetApplicationByIdAsync(ApplicationID);
            if (application == null) return NotFound();
            return Ok(application);
        }

        [HttpPost("SubmitLoanApplication")]
        public async Task<IActionResult> SubmitLoanApplication([FromForm] Req_LoanApplicationDto loanApplication)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdApplication = await _loanApplicationService.SubmitLoanApplicationAsync(loanApplication);
            return Ok(createdApplication);
        }

        [HttpPut("UpdateLoanApplication/{loanApplicationID}")]
        public async Task<IActionResult> UpdateLoanApplication(int loanApplicationID, [FromForm] Req_UpdateLoanApplicationDto loanApplication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _loanApplicationService.UpdateLoanApplicationAsync(loanApplicationID, loanApplication);

            if (!result)
            {
                return NotFound(new { message = "Loan application not found." });
            }

            return Ok(new { message = "Loan application updated successfully." });
        }

        [HttpDelete("DeleteLoanAppliation/{AppliationID}")]
        public async Task<IActionResult> DeleteLoanAppliation(int AppliationID)
        {
            var result = await _loanApplicationService.DeleteAsync(AppliationID);
            if (!result) return NotFound("Application NOT found");
            return Ok("Delete done successfully.");
        }

        [HttpGet("GetApplicationsByStatus")]
        public async Task<IActionResult> GetApplicationsByStatus([FromQuery] enLoanAndApplicationStatus status)
        {
            if (string.IsNullOrWhiteSpace(status.ToString()))
            {
                return BadRequest(new { message = "Status is required." });
            }

            var applications = await _loanApplicationService.GetApplicationsByStatusAsync(status.ToString());

            if (applications == null || !applications.Any())
            {
                return NotFound(new { message = $"No loan applications found with status '{status.ToString()}'." });
            }

            return Ok(applications);
        }

        
        [HttpPost("CalculateStandaloneEmi")]
        public async Task<IActionResult> CalculateStandaloneEmi([FromForm] Req_StandaloneEmiDto emiRequest)
        {
            if (emiRequest.LoanAmount <= 0 || emiRequest.AnnualInterestRate <= 0 || emiRequest.LoanTermMonths <= 0)
            {
                return BadRequest(new { message = "All input values must be greater than zero." });
            }

            var emiResult = await _loanApplicationService.CalculateStandaloneEmiAsync(emiRequest);
            return Ok(emiResult);
        }

        [HttpGet("CalculateEmiForApplication/{loanApplicationId}")]
        public async Task<IActionResult> CalculateEmiForApplication(int loanApplicationId)
        {
            try
            {
                var emiResult = await _loanApplicationService.CalculateEmiForApplicationAsync(loanApplicationId);
                return Ok(emiResult);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("ApproveLoanApplication/{loanApplicationId}")]
        public async Task<IActionResult> ApproveLoanApplication(int loanApplicationId)
        {
            bool approvalResult = await _loanApplicationService.ApproveLoanApplicationAsync(loanApplicationId);

            if (!approvalResult)
            {
                return NotFound(new { message = "Loan application not found or already approved." });
            }

            return Ok(new { message = "Loan application approved successfully ,check your balance." });
        }

    }
}
