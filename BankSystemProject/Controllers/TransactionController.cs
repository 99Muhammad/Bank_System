using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using BankSystemProject.Shared.Enums;
using Mailjet.Client.Resources.SMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction transaction;
        public TransactionController(ITransaction transaction)
        {
            this.transaction = transaction;
        }
       
        [HttpPost]
        public async Task<IActionResult> ImplementTransaction([FromForm] Req_ImplementTransactionDto transactionDto)
        {
            var result = await transaction.ImplementTransaction(transactionDto);
            string transactionType=transactionDto.transactionType.ToString();
            
            if (result!=-1)
            {
                var transactionRecord = new Res_ImplementTransactionDto
                {

                    Message = $"{transactionType} successful! Your new balance is $"+
                    result,
                    Amount = transactionDto.transactionAmount,
                    //Fee = transactioninfo.customerAccount,
                    Description = $"The amount of {transactionDto.transactionAmount} has been credited to your account."
                };

                return Ok(transactionRecord);
            }
               
            return BadRequest(new { Message = $"Failed to {transactionType}" });
        }

        
        //[HttpPost]
        //public async Task<IActionResult> Withdraw([FromForm] Req_ImplementTransactionDto transactionDto)
        //{
        //    var result = await transaction.ImplementTransaction(transactionDto);
        //    if (result)
        //        return Ok(new { Message = "Withdrawal successful" });
        //    return BadRequest(new { Message = "Failed to withdraw" });
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await transaction.GetAllTransactionsAsync();

            if (transactions == null || !transactions.Any())
                return NotFound("No transactions found.");

            return Ok(transactions);
        }

        [HttpGet("{accountNumber}")]
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
