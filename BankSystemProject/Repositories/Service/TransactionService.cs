using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace BankSystemProject.Repositories.Service
{
    public class TransactionService : ITransaction
    {
        private readonly Bank_DbContext _context;
        public TransactionService(Bank_DbContext _context)
        {
            this._context = _context;
        }
        public async Task<bool> ImplementTransaction(Req_ImplementTransactionDto transactionDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //var account = await _context.CustomersAccounts.FindAsync(transactionDto.AccountNumber);

                var transactioninfo = await _context.Transactions.Include(u => u.CustomerAccount)
                    .FirstOrDefaultAsync(u => u.CustomerAccount.AccountNumber == transactionDto.AccountNumber);
                
                if (transactioninfo == null || (transactionDto.transactionType == enTransactionType.Withdraw && transactioninfo.CustomerAccount.Balance < transactionDto.transactionAmount))
                    return false;

                transactioninfo.CustomerAccount.Balance += (transactionDto.transactionType == enTransactionType.Withdraw ? -transactionDto.transactionAmount : transactionDto.transactionAmount);

                
                await _context.CustomersAccounts.AddAsync(transactioninfo.CustomerAccount.Balance);
                
                var newTransaction = await new Transactions
                {
                    CustomerAccountId=transactioninfo.CustomerAccount.AccountId,
                    TransactionType = transactionDto.transactionType.ToString(),
                    Amount = transactionDto.transactionAmount,
                    TransactionDate = DateTime.UtcNow,
                };

                await _context.Transactions.AddAsync(newTransaction);
                await _context.SaveChangesAsync();

                if(transactionDto.transactionType.ToString()=="Deposit")
                {
                    var transactionRecord = new Res_ImplementTransactionDto
                    {
                        Message = "Deposit successful! Your new balance is $" + 
                        transactioninfo.CustomerAccount.Balance,
                        Amount = transactionDto.transactionAmount,
                        Fee = transactioninfo.CustomerAccount.BankFee,
                        
                        Description = $"The amount of {transactionDto.transactionAmount} has been credited to your account."
                    };
                }
                else
                {
                    var transactionRecord = new Res_ImplementTransactionDto
                    {
                        Message = "Withdrwal successful! Your new balance is $" +
                                            transactioninfo.CustomerAccount.Balance,
                        Amount = transactionDto.transactionAmount,
                        
                        Fee = transactioninfo.CustomerAccount.BankFee,

                        Description = $"${transactionDto.transactionAmount} has been debited from your account",
                    };
                }
               
                await transaction.CommitAsync();
               
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
               
                return false;
            }
        }
    }
}
