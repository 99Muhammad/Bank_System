using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAccount _IAccount;
        public AccountController(IAccount _IAccount)
        {
            this._IAccount=_IAccount;
        }
        [HttpPost("register")]
        //[Consumes("multipart/form-data")] // Ensure the method consumes multipart form data
        public async Task<IActionResult> Register([FromForm] Req_Registration registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid registration data");

            var result = await _IAccount.RegisterUserAsync(registerDto);

            if (result.UserName == null )
                return BadRequest(result.Message);



            return Ok(new
            {
                message = "Registration successful! , please check your email to Confirm Email ",
                // userId = result.userId,
                accountNumber = result.AccountNmber
            });
        }



        [HttpPost("login")]

        public async Task<IActionResult> Login(Req_Login loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid name or password");
            }
            var result = await _IAccount.LoginAsync(loginDto);
            if (result == "Invalid credentials."||result =="")
                return Unauthorized("Invalid credentials.");

            //else if (result == "Your Email Not Confirm please confirm your email")
            //{
            //    return BadRequest(result);
            //}
            //else if (result == "Faild")
            //{
            //    return BadRequest($"Unable to login {result}");
            //}

            return Ok(new { Message = "Login done successfully." });
        }
    }
}
