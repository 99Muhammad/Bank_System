//using BankSystemProject.Data;
//using BankSystemProject.Model;
//using BankSystemProject.Repositories.Interface;
//using BankSystemProject.Repositories.Service;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Text.Json.Serialization;

//namespace BankSystemProject
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);




//            // Add services to the container.

//            // Register DbContext with SQL Server
//            builder.Services.AddDbContext<Bank_DbContext>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//            // Add Identity services
//            //builder.Services.AddIdentity<Users, IdentityRole>(options =>
//            //{

//            //})
//            //.AddEntityFrameworkStores<Bank_DbContext>()
//            //.AddDefaultTokenProviders();
//            builder.Services.AddIdentity<Users, IdentityRole>()
//            .AddEntityFrameworkStores<Bank_DbContext>()
//            .AddDefaultTokenProviders();

//            builder.Services.AddControllers()
//             .AddJsonOptions(options =>
//              {
//                  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//              });

//            builder.Services.AddSwaggerGen(c =>
//            {
//                c.EnableAnnotations(); //onvert all enums to strings in Swagger
//            });

//            builder.Services.AddEndpointsApiExplorer();
//           // builder.Services.AddSwaggerGen();

//            builder.Services.AddEndpointsApiExplorer();



//            builder.Services.AddScoped<IAccount, AccountService>();

//            var app = builder.Build();


//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();


//            app.UseAuthentication(); 
//            app.UseAuthorization();

//            app.MapControllers();

//            app.Run();
//        }
//    }
//}


using AutoMapper;
using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using BankSystemProject.Repositories.Service;
using BankSystemProject.Repositories.Service.AdminServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace BankSystemProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddScoped<IAccount,AccountService>();
            



            // Register DbContext with SQL Server
            builder.Services.AddDbContext<Bank_DbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity services
            builder.Services.AddIdentity<Users, IdentityRole>()
                .AddEntityFrameworkStores<Bank_DbContext>()
                .AddDefaultTokenProviders();

            // Configure JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Disable for local testing
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            // Add JSON options
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddLogging();
            // Add Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations(); // Convert enums to strings in Swagger
            });

            builder.Services.AddEndpointsApiExplorer();

            // Register custom services
            builder.Services.AddScoped<IAccount, AccountService>();
            builder.Services.AddScoped<JWTService>();
            builder.Services.AddScoped<IUser, UserService>();
            builder.Services.AddScoped<ICustomerAccount, CustomerAccountsService>();
            builder.Services.AddScoped<ICreditCard,CreditCardService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Add Authentication and Authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
      

    }
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Req_CreditCardDto, CreditCard>();
        }
    }
}



