﻿using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BankSystemProject.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAccount _IAccount;
        private readonly JWTService _jwtService;
        //private readonly UserManager<Users> _userManager;
        public AccountController(IAccount _IAccount,
            JWTService _jwtService)
        {
            this._IAccount = _IAccount;
            this._jwtService = _jwtService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(Req_Login loginDto)
        {

            var loginResult = await _IAccount.LoginAsync(loginDto);

            if (loginResult == null)
            {
                return Unauthorized("Invalid Email or Password");
            }

            var token = _jwtService.GenerateToken(loginResult);
            //var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { Token = token });
        }




        [HttpPost("register")]
        //[Consumes("multipart/form-data")] // Ensure the method consumes multipart form data
        public async Task<IActionResult> Register([FromForm] Req_Registration registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid registration data");

            var result = await _IAccount.RegisterUserAsync(registerDto);

            if (result.UserName == null)
                return BadRequest(result.Message);



            return Ok(new
            {
                message = "Registration successful! , please check your email to Confirm Email ",
                // userId = result.userId,
                accountNumber = result.AccountNmber
            });
        }




    }
}