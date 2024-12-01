using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface ITransaction
    {
        Task<bool> ImplementTransaction(Req_ImplementTransactionDto transactionDto);
        Task<List<Res_TransactionDetailsDto>> GetAllTransactionsAsync();
        Task<List<Res_TransactionDetailsDto>> GetTransactionsByAccountNumberAsync(string accountNumber);
    }
}
