using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface IBranch
    {
        Task<List<Res_BranchDto>> GetAllBranchesAsync(); 
        Task<Res_BranchDto> GetBranchByIdAsync(int branchId); 
        Task<Res_BranchDto> CreateBranchAsync(Res_BranchDto branch); 
        Task<Res_BranchDto> UpdateBranchAsync(int branchId, Res_BranchDto branch); 
        Task<bool> DeleteBranchAsync(int branchId); 
    }
}
