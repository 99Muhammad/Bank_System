using BankSystemProject.Data;
using BankSystemProject.Helpers;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
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
        public async Task<double> ImplementTransaction(Req_ImplementTransactionDto transactionDto)
        {
            
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
                    return -1;

                var encodedPinCode = Base64Helper.Encode(transactionDto.PinCode);

                var creditCard = transactioninfo.CreditCards
                .FirstOrDefault(c => c.PinCode== encodedPinCode);
                
                if (creditCard== null) return -1;


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

                
                    return transactioninfo.Balance;
            }
            catch (Exception ex)
            {
                //await transaction.RollbackAsync();

                return -1;
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
                .Where(t => t.customerAccount.AccountNumber == Base64Helper.Encode(accountNumber))
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
