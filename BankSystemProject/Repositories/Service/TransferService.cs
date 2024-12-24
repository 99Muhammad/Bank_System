using BankSystemProject.Data;
using BankSystemProject.Helpers;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BankSystemProject.Repositories.Service
{
    public class TransferService:ITransfer
    {
        private readonly Bank_DbContext _context;

        public TransferService(Bank_DbContext context)
        {
            _context = context;
        }

        // Retrieve all transfer records
        public async Task<List<Res_TransferDto>> GetAllTransfersAsync()
        {
            var transfers = await _context.TransferInfo
             .Include(t => t.customerAccount) // Ensure navigation properties are included, if needed
             .ToListAsync();

            // Manually map the TransferInfo entities to TransferInfoDto
            var transferDtos = transfers.Select(t => new Res_TransferDto
            {
                TransferInfoId = t.TransferInfoId,
                CustomerAccountID = t.CustomerAccountID,
                AccountNumTransferFrom = t.AccountNumTransferFrom,
                AccountNumTransferTo = t.AccountNumTransferTo,
                BalanceFromBeforeTransfer = t.BalanceFromBeforeTransfer,
                BalanceToBeforeTransfer = t.BalanceToBeforeTransfer,
                DateTimeTransfer = t.DateTimeTransfer,
                BalanceFromAfterTransfer = t.BalanceFromAfterTransfer,
                BalanceToAfterTransfer = t.BalanceToAfterTransfer
            }).ToList();

            return transferDtos;
        }
        

        // Retrieve a specific transfer record by ID
        public async Task<Res_TransferDto> GetTransferByIdAsync(int id)
        {
            //return await _context.TransferInfo
            //    .Include(t => t.customerAccount)
            //    .FirstOrDefaultAsync(t => t.TransferInfoId == id);

            var transfers = await _context.TransferInfo
             .Include(t => t.customerAccount) // Ensure navigation properties are included, if needed
              .FirstOrDefaultAsync(t => t.TransferInfoId == id);

            if (transfers == null)
                return null!;
            // Manually map the TransferInfo entities to TransferInfoDto
            var transferDtos = new Res_TransferDto
            {
                TransferInfoId = transfers.TransferInfoId,
                CustomerAccountID = transfers.CustomerAccountID,
                AccountNumTransferFrom = transfers.AccountNumTransferFrom,
                AccountNumTransferTo = transfers.AccountNumTransferTo,
                BalanceFromBeforeTransfer = transfers.BalanceFromBeforeTransfer,
                BalanceToBeforeTransfer = transfers.BalanceToBeforeTransfer,
                DateTimeTransfer = transfers.DateTimeTransfer,
                BalanceFromAfterTransfer = transfers.BalanceFromAfterTransfer,
                BalanceToAfterTransfer = transfers.BalanceToAfterTransfer
            };

            return transferDtos;
        }

        // SubmitLoanApplicationAsync a new transfer record
        public async Task CreateTransferAsync(Req_TransferInfoDto transferInfoDto)
        {
            var fromAccount = await _context.CustomersAccounts
                .FirstOrDefaultAsync(c => c.AccountNumber == Base64Helper.Encode(transferInfoDto.AccountNumTransferFrom));
            var toAccount = await _context.CustomersAccounts
                .FirstOrDefaultAsync(c => c.AccountNumber == Base64Helper.Encode(transferInfoDto.AccountNumTransferTo));

            if (fromAccount == null || toAccount == null)
                throw new ArgumentException("Invalid account numbers.");

            if(transferInfoDto.AccountNumTransferFrom==transferInfoDto.AccountNumTransferTo)
                throw new ArgumentException("Invalid account numbers.");
            
            if (fromAccount.Balance < transferInfoDto.Amount)
                throw new InvalidOperationException("Insufficient funds in the source account.");
            
            var BalanceFromBefore = fromAccount.Balance;
            var BalanceToBefore = toAccount.Balance;
            // Update balances
            fromAccount.Balance -= transferInfoDto.Amount;
            toAccount.Balance += transferInfoDto.Amount;

            // CreateLoanRepayment a transfer record
            var transferInfo = new TransferInfo
            {
                CustomerAccountID=fromAccount.CustomerAccountId,
                AccountNumTransferFrom = transferInfoDto.AccountNumTransferFrom,
                AccountNumTransferTo = transferInfoDto.AccountNumTransferTo,
                BalanceFromBeforeTransfer = BalanceFromBefore,
                BalanceFromAfterTransfer = fromAccount.Balance,
                BalanceToAfterTransfer = toAccount.Balance,
                DateTimeTransfer = DateTime.UtcNow
            };

            // Add transfer info to the database
            _context.TransferInfo.Add(transferInfo);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }
    }
}
