using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SystemAdministrator")]
    public class BranchController : ControllerBase
    {
        private readonly IBranch _branchService;

        public BranchController(IBranch branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBranches()
        {
            var branches = await _branchService.GetAllBranchesAsync();
            return Ok(branches);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var branch = await _branchService.GetBranchByIdAsync(id);
            if (branch == null)
            {
                return NotFound(); // Return 404 if branch not found
            }
            return Ok(branch);
        }

      
        [HttpPost]
        public async Task<IActionResult> CreateBranch([FromBody] Res_Branch branch)
        {
            if (branch == null)
            {
                return BadRequest(); // Return 400 if the branch is null
            }

            var createdBranch = await _branchService.CreateBranchAsync(branch);
            return Ok(createdBranch);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranch(int id, [FromBody] Res_Branch branch)
        {
            if (branch == null )
            {
                return BadRequest(); 
            }

            var updatedBranch = await _branchService.UpdateBranchAsync(id, branch);
            if (updatedBranch == null)
            {
                return NotFound(); 
            }

            return Ok(updatedBranch);
        }

        // DELETE: api/branches/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var deleted = await _branchService.DeleteBranchAsync(id);
            if (!deleted)
            {
                return NotFound(); 
            }

            return NoContent(); 
        }
    }
}
