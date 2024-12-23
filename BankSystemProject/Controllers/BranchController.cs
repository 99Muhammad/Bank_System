using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
   
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
        [Authorize(Roles = "SystemAdministrator,BranchManager")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var branch = await _branchService.GetBranchByIdAsync(id);
            if (branch == null)
            {
                return NotFound(); 
            }
            return Ok(branch);
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdministrator,BranchManager")]
        public async Task<IActionResult> CreateBranch([FromBody] Res_BranchDto branch)
        {
          

            var createdBranch = await _branchService.CreateBranchAsync(branch);
            if(createdBranch==null)
            {
                return BadRequest("Branch name is already Exist!");
            }
            return Ok("Create branch done successfully");

        }


        [HttpPut("{id}")]
        [Authorize(Roles = "SystemAdministrator,BranchManager")]
        public async Task<IActionResult> UpdateBranchByID(int id, [FromBody] Res_BranchDto branch)
        {
            if (branch == null )
            {
                return BadRequest(); 
            }

            var updatedBranch = await _branchService.UpdateBranchAsync(id, branch);
            if (updatedBranch == null)
            {
                return NotFound("Branch with this ID NOT FOUND!"); 
            }

            return Ok("Updated done successfully");
        }

       
        [HttpDelete("{id}")]
        [Authorize(Roles = "SystemAdministrator,BranchManager")]
        public async Task<IActionResult> DeleteBranchById(int id)
        {
            var deleted = await _branchService.DeleteBranchAsync(id);
            if (!deleted)
            {
                return NotFound("Branch with this ID NOT FOUND!");
            }

            return Ok("Deleted done successfully"); 
        }
    }
}
