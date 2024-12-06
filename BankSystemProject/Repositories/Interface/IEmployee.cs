using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface IEmployee
    {
        Task<Res_EmployeeInfoDto> GetEmployeeInfoByUsernameAsync(string username, bool includeDeleted);
        Task<bool> UpdateEmployeeInfoByUserNameAsync(string username, Req_UpdateEmployeeInfoDto updateDto);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<bool> UpdateEmployeeInfoByIDAsync(int EmployeeID, Req_UpdateEmployeeInfoByAdminDto updateDto);
        Task<bool> SoftDeleteEmployeeAsync(string UserID);
    }
}
