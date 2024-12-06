using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransfer _transferInfoService;

        public TransferController(ITransfer transferInfoService)
        {
            _transferInfoService = transferInfoService;
        }

        // GET: api/TransferInfo
        [HttpGet]
        public async Task<IActionResult> GetAllTransfers()
        {
            var transfers = await _transferInfoService.GetAllTransfersAsync();
            return Ok(transfers);
        }

        // GET: api/TransferInfo/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransferById(int id)
        {
            var transfer = await _transferInfoService.GetTransferByIdAsync(id);
            if (transfer == null)
                return NotFound("Transfer not found.");

            return Ok(transfer);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> CreateTransferAsync([FromBody] Req_TransferInfoDto transferInfoDto)
        {
            try
            {
                await _transferInfoService.CreateTransferAsync(transferInfoDto);
                return Ok("Transfer successful.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
