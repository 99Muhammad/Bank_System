using BankSystemProject.Model;
using BankSystemProject.Repositories.Interface;
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

        // POST: api/TransferInfo
        [HttpPost]
        public async Task<IActionResult> CreateTransfer([FromBody] TransferInfo transferInfo)
        {
            try
            {
                await _transferInfoService.CreateTransferAsync(transferInfo);
                return CreatedAtAction(nameof(GetTransferById), new { id = transferInfo.TransferInfoId }, transferInfo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Return invalid account error
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return insufficient funds error
            }
        }
    }
}
