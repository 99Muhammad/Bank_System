using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankSystemProject.Repositories.Service
{
    public class EmployeeService:IEmployee
    {
        private readonly Bank_DbContext _context;
        private readonly UserManager<Users> userManager;
        public EmployeeService(Bank_DbContext _context,UserManager<Users> userManager)
        {
            this._context = _context;   
            this.userManager = userManager;
        }
        
        public async Task<Res_EmployeeInfoDto> GetEmployeeInfoByUsernameAsync(string username,bool includeDeleted)
        {
            var employeeInfo = await _context.Employee
                .Include(e => e.User)
                .Include(e => e.BranchEmployee)
                .Where(e => includeDeleted ? e.IsDeleted : !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.User.UserName == username);

            if (employeeInfo == null)
            {
                return null; 
            }

            return new Res_EmployeeInfoDto
            {

                FullName = employeeInfo.User.FullName,
                Email = employeeInfo.User.Email,
                PhoneNumber = employeeInfo.User.PhoneNumber,
                Address = employeeInfo.User.Address,
                Position = employeeInfo.User.Role.ToString(),
                Salary = employeeInfo.EmployeeSalary,
                HireDate = employeeInfo.HireDate,
                PersonalImage=employeeInfo.User.PersonalImage,
                BranchName = employeeInfo.BranchEmployee.BranchName,
                BranchLocation = employeeInfo.BranchEmployee.BranchLocation,

            };
        }

       
        public async Task<bool> UpdateEmployeeInfoByUserNameAsync(string username, Req_UpdateEmployeeInfoDto updateDto)
        {
           
            var updateEmployee = await _context.Employee
                .Include(e=>e.User)
                .Include(e=>e.BranchEmployee)
                .FirstOrDefaultAsync(e => e.User.UserName ==username);
            if (updateEmployee == null)
            {
                return false;      
            }

            //var user = await userManager.FindByNameAsync(updateEmployee.User.Id);
            
            updateEmployee.User.Email = updateDto.Email;
            updateEmployee.User.PhoneNumber = updateDto.PhoneNumber ;
            updateEmployee.User.Address = updateDto.Address ;
            updateEmployee.User.PasswordHash=updateDto.Password;
            updateEmployee.User.PersonalImage = updateDto.ImageUrl;
            updateEmployee.User.UserName=updateDto.UserName;
            
            // Save changes
            _context.Employee.Update(updateEmployee);
            //_context.Users.UpdateLoanApplication(user); 
            await _context.SaveChangesAsync();

            return true;
        }

       
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employee
                                 .Include(e => e.User)  
                                 .ToListAsync();
        }

        public async Task<bool> SoftDeleteEmployeeAsync(string UserID)
        {
            var employee = await _context.Employee
                                         .Include(e => e.User)  
                                         .FirstOrDefaultAsync(e => e.UserId == UserID);

            if (employee == null)
            {
                return false; 
            }

            employee.IsDeleted = true;            

            if (employee.User != null)
            {
                employee.User.IsDeleted = true;
            }

            await _context.SaveChangesAsync();  

            return true;
        }

        public async Task<bool> UpdateEmployeeInfoByIDAsync(int EmployeeID, Req_UpdateEmployeeInfoByAdminDto updateDto)
        {

            var updateEmployee = await _context.Employee
                .Include(e => e.User)
                .Include(e => e.BranchEmployee)
                .FirstOrDefaultAsync(e => e.EmployeeId == EmployeeID);
            if (updateEmployee == null)
            {
                return false;
            }

            //var user = await userManager.FindByNameAsync(updateEmployee.User.Id);

            updateEmployee.User.Role = updateDto.UserRole.ToString();
            updateEmployee.EmployeeSalary = updateDto.Salary;
            updateEmployee.BranchID = (int)updateDto.BrnachName;
            updateEmployee.IsDeleted = updateDto.IsDeleted;
  
            // Save changes
            _context.Employee.Update(updateEmployee);
            //_context.Users.UpdateLoanApplication(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
