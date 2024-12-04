using BankSystemProject.Data;
using BankSystemProject.Model;
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
        public async Task<List<TransferInfo>> GetAllTransfersAsync()
        {
            return await _context.TransferInfo
                .Include(t => t.customerAccount)  // Optionally include related data
                .ToListAsync();
        }

        // Retrieve a specific transfer record by ID
        public async Task<TransferInfo> GetTransferByIdAsync(int id)
        {
            return await _context.TransferInfo
                .Include(t => t.customerAccount)
                .FirstOrDefaultAsync(t => t.TransferInfoId == id);
        }

        // Create a new transfer record
        public async Task CreateTransferAsync(TransferInfo transferInfo)
        {
            var fromAccount = await _context.CustomersAccounts
                .FirstOrDefaultAsync(c => c.AccountNumber == transferInfo.AccountNumTransferFrom);
            var toAccount = await _context.CustomersAccounts
                .FirstOrDefaultAsync(c => c.AccountNumber == transferInfo.AccountNumTransferTo);

            if (fromAccount == null || toAccount == null)
                throw new ArgumentException("Invalid account numbers.");

            if (fromAccount.Balance < transferInfo.BalanceFromBeforeTransfer)
                throw new InvalidOperationException("Insufficient funds in the source account.");

            // Update balances
            fromAccount.Balance -= transferInfo.BalanceFromAfterTransfer;
            toAccount.Balance += transferInfo.BalanceToAfterTransfer;

            // Add transfer info to the database
            _context.TransferInfo.Add(transferInfo);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }
    }
}
