using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction transaction;
        public TransactionController(ITransaction transaction)
        {
            this.transaction = transaction;
        }
       
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromForm] Req_ImplementTransactionDto transactionDto)
        {
            var result = await transaction.ImplementTransaction(transactionDto);
            if (result)
                return Ok(new { Message = "Deposit successful" });
            return BadRequest(new { Message = "Failed to deposit" });
        }

        
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromForm] Req_ImplementTransactionDto transactionDto)
        {
            var result = await transaction.ImplementTransaction(transactionDto);
            if (result)
                return Ok(new { Message = "Withdrawal successful" });
            return BadRequest(new { Message = "Failed to withdraw" });
        }

        [HttpGet("GetAllTransactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await transaction.GetAllTransactionsAsync();

            if (transactions == null || !transactions.Any())
                return NotFound("No transactions found.");

            return Ok(transactions);
        }

        [HttpGet("GetTransactionsByAccount/{accountNumber}")]
        public async Task<IActionResult> GetTransactionsByAccountNumber(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest("Account number cannot be null or empty.");

            var transactions = await transaction.GetTransactionsByAccountNumberAsync(accountNumber);

            if (transactions == null || !transactions.Any())
                return NotFound($"No transactions found for account number {accountNumber}.");

            return Ok(transactions);
        }

    }
}
