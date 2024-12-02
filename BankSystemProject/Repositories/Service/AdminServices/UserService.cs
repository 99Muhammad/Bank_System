using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using MailKit.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace BankSystemProject.Repositories.Service.AdminServices
{
    public class UserService : IUser
    {

        private readonly UserManager<Users> _userManager;
        private readonly Bank_DbContext _context;
        public UserService(UserManager<Users> userManager, Bank_DbContext _context)
        {
            _userManager = userManager;
            this._context = _context;
        }

        //For Admin Role
        public async Task<List<Res_UsersInfo>> FilterUsersByRoleAsync(string roleName)
        {

            //  return await _userManager.GetUsersInRoleAsync(roleName) as List<Users>;
            try
            {  var usersInRole = await _userManager
                    .GetUsersInRoleAsync(roleName) ;

                var userDtos = usersInRole.Where(u =>!u.IsDeleted)
                    .Select(u => new Res_UsersInfo
                {
                    //Id= u.Id,
                    FullName = u.FullName,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber=u.PhoneNumber,
                    Address=u.Address,
                    DateOfBirth=u.DateOfBirth,
                  //PersonalImage=u.PersonalImage,
                    IsDeleted=u.IsDeleted,
                    Gender=u.Gender,
                   

                }).ToList();

                return userDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching users by role", ex);
            }
           // throw new NotImplementedException();
        }
        //For Admin Role
        public async Task<List<Res_UsersInfo>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userManager.Users
                    .Where(u => !u.IsDeleted)
                    .ToListAsync();

                var usersResponse = users.Select(u => new Res_UsersInfo
                {

                    FullName = u.FullName,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    DateOfBirth = u.DateOfBirth,
                    //PersonalImage=u.PersonalImage,
                    IsDeleted = u.IsDeleted,
                    Gender = u.Gender,

                }).ToList();
                return usersResponse;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching users by role", ex);
            }
           
        }

        //For Admin Role
        public async Task<Res_UsersInfo> SearchUserByFullNameOrUserNameAsync(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return null;

            var user = await _userManager.Users
                .Where(u=>!u.IsDeleted)
            .FirstOrDefaultAsync(u => u.UserName.Contains(Name) || u.FullName.Contains(Name))
            ;

            if (user == null)
                return null; 

            return new Res_UsersInfo
            {
               FullName = user.FullName,
               UserName = user.UserName,
               Email = user.Email,
               PhoneNumber = user.PhoneNumber,
               Address = user.Address,
               DateOfBirth = user.DateOfBirth,
               //PersonalImage=user.PersonalImage,
               IsDeleted = user.IsDeleted,
               Gender = user.Gender,
            };
        }
        //For Admin Role
        public async Task<Res_UsersInfo> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null; 

            var user = await _userManager.Users.Where(u => !u.IsDeleted).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null; 

            return new Res_UsersInfo
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                //PersonalImage=user.PersonalImage,
                IsDeleted = user.IsDeleted,
                Gender = user.Gender,
            };
        }

        /* public async Task<bool> DeleteUserAsync(string UserID)
         {
             var user = await _userManager.FindByIdAsync(UserID);

             //We can't use my function(GetUserByIdAsync) because it return an 
             //object of type Res_UsersInfo not of type Users.
             //  {{        var user = await GetUserByIdAsync(UserID);     }}
             if (user == null) return false;


             var result = await _userManager.DeleteAsync(user);
             return result.Succeeded;
         }*/

        //For Admin Role
        public async Task<bool> SoftDeleteCustomerUserAsync(string userId)
        {
            // Fetch the user
            var user = await _context.Users
                .Include(u => u.CustomerAccounts) 
                    .ThenInclude(ca => ca.CreditCards) 
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return false;

            
            user.IsDeleted = true;
            //user.DeletedAt = DateTime.UtcNow;

            
            foreach (var account in user.CustomerAccounts)
            {
                account.IsDeleted = true;
                //account.DeletedAt = DateTime.UtcNow;

                foreach (var card in account.CreditCards)
                {
                    card.IsDeleted = true;
                   // card.DeletedAt = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        //For Admin Role
        public async Task<bool> UpdateUserAsync(string userID, Req_UpdateUserInfo updateUserDto)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null||user.IsDeleted) return false;

            
            string FName = string.Join(" ", updateUserDto.FirstName, updateUserDto.SecondName, updateUserDto.ThirdName, updateUserDto.LastName).Trim();
            user.FullName = FName;
            user.Email = updateUserDto.Email;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.Address = updateUserDto.Address;
            user.DateOfBirth = updateUserDto.DateOfBirth;
            user.Role = updateUserDto.UserRole.ToString();
            //user.PersonalImage = updateUserDto.PersonalImage;
            user.IsDeleted = updateUserDto.IsDeleted;
            user.Gender = updateUserDto.Gender.ToString();


            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }


    }
}
