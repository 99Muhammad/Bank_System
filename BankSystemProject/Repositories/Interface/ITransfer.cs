﻿using BankSystemProject.Model;

namespace BankSystemProject.Repositories.Interface
{
    public interface ITransfer
    {
        Task<List<TransferInfo>> GetAllTransfersAsync();
        Task<TransferInfo> GetTransferByIdAsync(int id);
        Task CreateTransferAsync(TransferInfo transferInfo);
    }
}