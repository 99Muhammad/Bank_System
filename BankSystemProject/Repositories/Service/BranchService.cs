﻿using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BankSystemProject.Repositories.Service
{
    public class BranchService:IBranch
    {
        private readonly Bank_DbContext _dbContext;

        public BranchService(Bank_DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Res_BranchDto>> GetAllBranchesAsync()
        {
            var branches =await _dbContext.Branches
                .Select(b => new Res_BranchDto
                {
                    BranchName = b.BranchName,
                    BranchLocation = b.BranchLocation,
                }).ToListAsync();

            return  branches;
            
        }

        public async Task<Res_BranchDto> GetBranchByIdAsync(int branchId)
        {

            var branch =await _dbContext.Branches
                .FirstOrDefaultAsync(b => b.BranchId == branchId);
            
            if (branch == null)
                return null!;

            var resBranch = new Res_BranchDto
            {
                BranchName = branch.BranchName,
                BranchLocation = branch.BranchLocation,
            };
                

            return resBranch;
        }

        public async Task<Res_BranchDto> CreateBranchAsync(Res_BranchDto branch)
        {
            var existBranch = await _dbContext.Branches
                .FirstOrDefaultAsync(b=>b.BranchName==branch.BranchName);
           
            if (existBranch != null)
                return null!;
           
            var newBranch = new Branch
            {
                BranchName = branch.BranchName,
                BranchLocation = branch.BranchLocation,
            };
            _dbContext.Branches.Add(newBranch);
            await _dbContext.SaveChangesAsync();
            return branch; 
        }

        public async Task<Res_BranchDto> UpdateBranchAsync(int branchId, Res_BranchDto branch)
        {
            var existingBranch = await _dbContext.Branches
                .FirstOrDefaultAsync(b => b.BranchId == branchId);

            if (existingBranch == null)
            {
                return null!; 
            }

            
            existingBranch.BranchName = branch.BranchName;
            existingBranch.BranchLocation = branch.BranchLocation;
           
            var res = new Res_BranchDto
            {
                BranchName = existingBranch.BranchName,
                BranchLocation = existingBranch.BranchLocation,
            };
            await _dbContext.SaveChangesAsync();
            return res;
        }

        public async Task<bool> DeleteBranchAsync(int branchId)
        {
            var branch = await _dbContext.Branches
                .FirstOrDefaultAsync(b => b.BranchId == branchId);

            if (branch == null)
            {
                return false; 
            }

            _dbContext.Branches.Remove(branch);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
