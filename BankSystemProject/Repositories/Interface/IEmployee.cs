using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface IEmployee
    {
        Task<Res_EmployeeInfoDto> GetEmployeeInfoByUsernameAsync(string username);
        Task<bool> UpdateEmployeeInfoByUserNameAsync(string username, Req_UpdateEmployeeInfoDto updateDto);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<bool> UpdateEmployeeInfoByIDAsync(string UserID, Req_UpdateEmployeeInfoByAdminDto updateDto);
         Task<bool> SoftDeleteEmployeeAsync(string UserID);
    }
}
