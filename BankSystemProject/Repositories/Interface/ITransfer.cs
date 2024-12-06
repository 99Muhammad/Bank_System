using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface ITransfer
    {
        Task<List<TransferInfo>> GetAllTransfersAsync();
        Task<TransferInfo> GetTransferByIdAsync(int id);
        Task CreateTransferAsync(Req_TransferInfoDto transferInfoDto);
    }
}
