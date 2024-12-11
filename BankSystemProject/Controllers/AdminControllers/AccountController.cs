using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using BankSystemProject.Repositories.Service;
using BankSystemProject.Repositories.Service.AdminServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BankSystemProject.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _IAccount;
        private readonly JWTService _jwtService;
        private readonly UserManager<Users> _userManager;
        private readonly IEmail _Email;
        private readonly IUrlHelper _urlHelper;
        private readonly  string ?_appUrl;
        private readonly IConfiguration _configuration;

        public AccountController(
            IAccount _IAccount,
            JWTService _jwtService,
            UserManager<Users> _userManager,
            IEmail _Email,
            IUrlHelperFactory urlHelperFactory,
            IHttpContextAccessor httpContextAccessor,IConfiguration _configuration)
        {
            this._IAccount = _IAccount;
            this._jwtService = _jwtService;
            this._userManager = _userManager;
            this._Email = _Email;
            this._configuration = _configuration;
            _appUrl= _appUrl = _configuration["App:Url"];

            // Ensure IHttpContextAccessor is properly registered and not null
            if (httpContextAccessor.HttpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            }

            _urlHelper = urlHelperFactory.GetUrlHelper(new ActionContext(
                httpContextAccessor.HttpContext,
                httpContextAccessor.HttpContext.GetRouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()));
        }
    


    [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromForm]Req_Login loginDto)
        {

            var loginResult = await _IAccount.LoginAsync(loginDto);

            if (loginResult == null)
            {
                return Unauthorized("Invalid UserName or Password");
            }

           // var token = _jwtService.GenerateJwtToken(loginResult);
            //var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { Token = loginResult });
        }




        [HttpPost("register")]
        //[Consumes("multipart/form-data")] // Ensure the method consumes multipart form data
        public async Task<IActionResult> Register([FromForm] Req_Registration registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid registration data");

            var result = await _IAccount.RegisterUser(registerDto);

            if (result.UserName == null)
                return BadRequest(result.Message);

            
            return Ok(new
            {
                message = "Registration successful! , please check your email to Confirm Email ",
                //accountNumber = result.AccountNmber,
                result.UserName,
                result.Success,
                result.AccessToken,
                result.confirmLink,
                
            });
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var tokens = await _IAccount.RefreshTokensAsync(refreshToken);
            if (tokens == null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }
            return Ok(tokens);
        }



        //[HttpGet("confirm-email")]
        //public async Task<IActionResult> ConfirmEmail(string email, string token)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null) return BadRequest("Invalid email.");

        //    var result = await _userManager.ConfirmEmailAsync(user, token);
        //    if (result.Succeeded) return Ok("Email confirmed successfully.");

        //    return BadRequest("Email confirmation failed.");
        //}

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _IAccount.ConfirmEmailAsync(userId, token);

            if (!result)
            {
                return BadRequest("Email confirmation failed.");
            }

            return Ok("Email confirmed successfully.");
        }

        //    [HttpPost("RequestPasswordReset")]
        //    public async Task<IActionResult> RequestPasswordReset([FromBody] Req_ResetPasswordDto request)
        //    {
        //        // Validate if the email exists in the system
        //        var user = await _userManager.FindByEmailAsync(request.Email);
        //        if (user == null)
        //        {
        //            return NotFound(new { message = "User with the specified email does not exist." });
        //        }

        //        // Generate password reset token
        //        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        //        // Create the reset password URL
        //        var resetUrl = _urlHelper.Action(
        //            action: "ResetPassword",
        //            controller: "Account",
        //            values: new { email = user.Email, token = resetToken },
        //            protocol: "https"
        //        );

        //        // Send the reset URL to the user's email
        //        string emailContent = $@"
        //<html>
        //<body>
        //    <p>You requested a password reset. Click the link below to reset your password:</p>
        //    <p><a href='{resetUrl}'>Reset Password</a></p>
        //</body>
        //</html>";
        //        await _Email.SendEmailAsync(user.Email, "Reset Password", emailContent, emailContent);

        //        return Ok(new { message = "Password reset link sent to your email." });
        //    }

        //    [HttpPost("ResetPassword")]
        //    public async Task<IActionResult> ResetPassword([FromBody] Request_ResetPassword model)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(new { message = "Invalid data.", errors = ModelState });
        //        }

        //        // Find the user
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user == null)
        //        {
        //            return NotFound(new { message = "User not found." });
        //        }

        //        // Reset the password
        //        var resetResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        //        if (!resetResult.Succeeded)
        //        {
        //            return BadRequest(new { message = "Password reset failed.", errors = resetResult.Errors });
        //        }

        //        return Ok(new { message = "Password has been reset successfully." });
        //    }


        [HttpPost("reset-password-request")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] ResetPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new { Message = "User not found" });
            }

            // Generate the reset password token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Generate a password reset link
           // var resetLink = _urlHelper.Action("ResetPassword", "Account", new { token, email = user.Email }, protocol: HttpContext.Request.Scheme);
            var resetLink = $"{_appUrl}/api/Account/ResetPassword?email={user.Email}&token={token}";
            // HTML email body with a clickable link
            //var emailBody = $"<p>Please reset your password by clicking the following link: <a href=\"{resetLink}\">Reset Password</a></p>";

            var emailBody = $@"
             <html>
                 <body>
                     <p>Please reset your password by clicking the following link:</p>
                     <p><a href='{ resetLink}'>Reset Password</a></p>
                 </ body >
             </ html > ";


         
            // Send the reset link to the user's email as HTML
            await _Email.SendEmailAsync(user.Email, "Password Reset", emailBody,emailBody);

            return Ok(new { Message = "Password reset link has been sent to your email." });
        }


        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordDto resetPasswordDto)
        {
            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
            {
                return BadRequest(new { Message = "Passwords do not match" });
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return BadRequest(new { Message = "User not found" });
            }

            // Reset the password using the token
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = string.Join(", ", result.Errors.Select(e => e.Description)) });
            }

            return Ok(new { Message = "Password has been successfully reset." });
        }


        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _IAccount.Logout(User);
            Response.Cookies.Delete("AuthToken");
            return Ok(new { message = "Successfully logged out" });
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordReqDTO forgotPasswordDto)
        {
            var result = await _IAccount.ForgotPasswordAsync(forgotPasswordDto);
            if (!result)
            {
                return BadRequest("Failed to send reset email.");
            }

            return Ok("Password reset link sent.");
        }

    }
}
