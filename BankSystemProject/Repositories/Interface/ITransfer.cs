using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface ITransfer
    {
        Task<List<Res_TransferDto>> GetAllTransfersAsync();
        Task<Res_TransferDto> GetTransferByIdAsync(int id);
        Task CreateTransferAsync(Req_TransferInfoDto transferInfoDto);
    }
}
