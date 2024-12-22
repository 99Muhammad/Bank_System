using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface IBranch
    {
        Task<List<Res_Branch>> GetAllBranchesAsync(); 
        Task<Res_Branch> GetBranchByIdAsync(int branchId); 
        Task<Res_Branch> CreateBranchAsync(Res_Branch branch); 
        Task<Res_Branch> UpdateBranchAsync(int branchId, Res_Branch branch); 
        Task<bool> DeleteBranchAsync(int branchId); 
    }
}
