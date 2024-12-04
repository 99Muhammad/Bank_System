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
            //using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //var account = await _context.CustomersAccounts.FindAsync(transactionDto.UserName);

                var transactioninfo = await _context.Transactions.Include(u => u.customerAccount)
                    .FirstOrDefaultAsync(u => u.customerAccount.AccountNumber == transactionDto.AccountNumber);

                if (transactioninfo == null || (transactionDto.transactionType == enTransactionType.Withdraw && transactioninfo.customerAccount.Balance < transactionDto.transactionAmount))
                    return false;

                transactioninfo.customerAccount.Balance += (transactionDto.transactionType == enTransactionType.Withdraw ? -transactionDto.transactionAmount : transactionDto.transactionAmount);

                await _context.CustomersAccounts.AddAsync(transactioninfo.customerAccount);

                var newTransaction = new TransactionsDepWi
                {
                    CustomerAccountId = transactioninfo.customerAccount.CustomerAccountId,
                    TransactionType = transactionDto.transactionType.ToString(),
                    Amount = transactionDto.transactionAmount,
                    TransactionDate = DateTime.UtcNow,
                };

                await _context.Transactions.AddAsync(newTransaction);
                await _context.SaveChangesAsync();

                if (transactionDto.transactionType.ToString() == "Deposit")
                {
                    var transactionRecord = new Res_ImplementTransactionDto
                    {
                        Message = "Deposit successful! Your new balance is $" +
                        transactioninfo.customerAccount.Balance,
                        Amount = transactionDto.transactionAmount,
                        //Fee = transactioninfo.customerAccount,

                        Description = $"The amount of {transactionDto.transactionAmount} has been credited to your account."
                    };
                }
                else
                {
                    var transactionRecord = new Res_ImplementTransactionDto
                    {
                        Message = "Withdrwal successful! Your new balance is $" +
                                            transactioninfo.customerAccount.Balance,
                        Amount = transactionDto.transactionAmount,

                        //Fee = transactioninfo.customerAccount.BankFee,

                        Description = $"${transactionDto.transactionAmount} has been debited from your account",
                    };
                }

                //await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                //await transaction.RollbackAsync();

                return false;
            }
        }

        public async Task<List<Res_TransactionDetailsDto>> GetAllTransactionsAsync()
        {
            var transactions = await _context.Transactions
                .Include(t => t.customerAccount)
                .Select(t => new Res_TransactionDetailsDto
                {
                    TransactionId = t.TransactionId,
                    CustomerAccountName = t.customerAccount.User.FullName,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                })
                .ToListAsync();

            return transactions;
        }

        public async Task<List<Res_TransactionDetailsDto>> GetTransactionsByAccountNumberAsync(string accountNumber)
        {
            var transactions = await _context.Transactions
                .Include(t => t.customerAccount)
                .Where(t => t.customerAccount.AccountNumber == accountNumber)
                .Select(t => new Res_TransactionDetailsDto
                {
                    TransactionId = t.TransactionId,
                    CustomerAccountName = t.customerAccount.User.FullName,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                })
                .ToListAsync();

            return transactions;
        }

    }
}
