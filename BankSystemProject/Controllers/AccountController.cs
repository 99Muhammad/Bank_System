using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using BankSystemProject.Repositories.Service;
using BankSystemProject.Repositories.Service.AdminServices;
using BankSystemProject.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BankSystemProject.Controllers
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
        private readonly string? _appUrl;
        private readonly IConfiguration _configuration;

        public AccountController(
            IAccount _IAccount,
            JWTService _jwtService,
            UserManager<Users> _userManager,
            IEmail _Email,
            IUrlHelperFactory urlHelperFactory,
            IHttpContextAccessor httpContextAccessor, IConfiguration _configuration)
        {
            this._IAccount = _IAccount;
            this._jwtService = _jwtService;
            this._userManager = _userManager;
            this._Email = _Email;
            this._configuration = _configuration;
            _appUrl = _appUrl = _configuration["App:Url"];


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
        public async Task<IActionResult> LoginAsync([FromForm] Req_Login loginDto)
        {

            var loginResult = await _IAccount.LoginAsync(loginDto);

            if (loginResult == null)
            {
                return Unauthorized("Invalid UserName or Password");
            }

            return Ok(new { Token = loginResult });
        }

        [Authorize(Roles = "SystemAdministrator")]
        [HttpPost("Register")]
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
                result.UserName,
                result.Success,
                //result.AccessToken,
                //result.confirmLink,

            });
        }

        [Authorize(Roles = "SystemAdministrator")]
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

        [Authorize(Roles = "SystemAdministrator")]
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

        [Authorize]
        [HttpPost("reset-password-request")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] ResetPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new { Message = "User not found" });
            }


            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);


            var resetLink = $"{_appUrl}/api/Account/ResetPassword?email={user.Email}&token={resetToken}";
            // HTML email body with a clickable link
            var emailBody = $@"
             <html>
                 <body>
                     <p>Please reset your password by clicking the following link:</p>
                     <p><a href='{resetLink}'>Reset Password</a></p>
                 </ body >
             </ html > ";

            // Send the reset link to the user's email as HTML
            await _Email.SendEmailAsync(user.Email, "Password Reset", emailBody, emailBody);

            return Ok(new { Message = "Password reset link has been sent to your email." });
        }

        [Authorize]
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

            // Reset the password using the resetToken
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = string.Join(", ", result.Errors.Select(e => e.Description)) });
            }

            return Ok(new { Message = "Password has been successfully reset." });
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _IAccount.Logout(User);
            Response.Cookies.Delete("AuthToken");
            return Ok(new { message = "Successfully logged out" });
        }

        [Authorize]
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
