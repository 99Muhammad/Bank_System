using BankSystemProject.Data;
using BankSystemProject.Helpers;
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

                var transactioninfo = await _context.CustomersAccounts.
                    Include(u => u.CreditCards)
                    //.ThenInclude(c=>c.CreditCards)
                    .FirstOrDefaultAsync(u => u.AccountNumber 
                    == Base64Helper.Encode(transactionDto.AccountNumber) );

                if (transactioninfo == null || (transactionDto.transactionType 
                    == enTransactionType.Withdraw && transactioninfo.Balance < transactionDto.transactionAmount))
                    return false;


               var creditCard= transactioninfo.CreditCards
                .FirstOrDefault(c => c.PinCode == Base64Helper.Encode(transactionDto.PinCode)); 
                   
                if(creditCard== null) return false;


               transactioninfo.Balance += (transactionDto.transactionType == enTransactionType.Withdraw ? -transactionDto.transactionAmount : transactionDto.transactionAmount);

                //await _context.CustomersAccounts.AddAsync(transactioninfo);
                _context.CustomersAccounts.Update(transactioninfo);
                var newTransaction = new TransactionsDepWi
                {
                    CustomerAccountId = transactioninfo.CustomerAccountId,
                    TransactionType = transactionDto.transactionType.ToString(),
                    Amount = transactionDto.transactionAmount,
                    TransactionDate = DateTime.UtcNow,
                };

                await _context.Transactions.AddAsync(newTransaction);
                await _context.SaveChangesAsync();

                if (transactionDto.transactionType.ToString() == enTransactionType.Deposit.ToString())
                {
                    var transactionRecord = new Res_ImplementTransactionDto
                    {
                        Message = "Deposit successful! Your new balance is $" +
                        transactioninfo.Balance,
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
                                            transactioninfo.Balance,
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
