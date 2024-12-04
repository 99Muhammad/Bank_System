using BankSystemProject.Model;
using Driving_License_Management_DVLD_.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankSystemProject.Repositories.Service
{
    public class JWTService
    {
       // private readonly UserManager<Users> _userManager;
        private readonly IConfiguration _configuration;
       // private readonly RoleManager<IdentityRole> roleManager;


        public JWTService(IConfiguration configuration)
        {
            //_userManager = userManager;
            _configuration = configuration;
           // this.roleManager = roleManager;
        }
        //public async Task<string>AddRoleAsync(AddRoleModel model)
        //{
        //    var user = await _userManager.FindByIdAsync(model.UserId);

        //    if (user is null || !await roleManager.RoleExistsAsync(model.Role))
        //        return "Invalid user ID or Role";

        //    if (await _userManager.IsInRoleAsync(user, model.Role))
        //        return "User already assigned to this role";

        //    var result = await _userManager.AddToRoleAsync(user, model.Role);

        //    return result.Succeeded ? string.Empty : "Sonething went wrong";
        //}
        //private async Task<JwtSecurityToken> CreateJwtToken(Users user)
        //{
        //    var userClaims = await _userManager.GetClaimsAsync(user);
        //    var roles = await _userManager.GetRolesAsync(user);
        //    var roleClaims = new List<Claim>();

        //    foreach (var role in roles)
        //        roleClaims.Add(new Claim("roles", role));

        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.UserName, user.UserName),
        //       // new Claim(JwtRegisteredClaimNames.Exp,
        //             //     ((int)DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:DurationInDays"])).Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString()),
        //    }
        //    .Union(userClaims)
        //    .Union(roleClaims);

        //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        //    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        //    var jwtSecurityToken = new JwtSecurityToken(
        //        issuer: _configuration["JWT:Issuer"],
        //        audience: _configuration["JWT:Audience"],
        //        claims: claims,
        //       //expires: DateTime.Now.AddDays(int.Parse(_configuration["JWT:DurationInDays"])),
        //        signingCredentials: signingCredentials);

        //    return jwtSecurityToken;
        //}

        //public async Task<string> GenerateToken(Users user)
        //{
        //    var token = await CreateJwtToken(user);
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        public string GenerateJwtToken(Users user)
        {
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                 new Claim(ClaimTypes.Role, user.Role),
                 new Claim(ClaimTypes.Name, user.UserName),
       
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireDurationInMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}