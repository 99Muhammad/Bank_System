using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;

namespace BankSystemProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


        

            // Add services to the container.

            // Register DbContext with SQL Server
            builder.Services.AddDbContext<Bank_DbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity services
            //builder.Services.AddIdentity<Users, IdentityRole>(options =>
            //{

            //})
            //.AddEntityFrameworkStores<Bank_DbContext>()
            //.AddDefaultTokenProviders();
            builder.Services.AddIdentity<Users, IdentityRole>()
            .AddEntityFrameworkStores<Bank_DbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddControllers()
             .AddJsonOptions(options =>
              {
                  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
              });

            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations(); //onvert all enums to strings in Swagger
            });

            builder.Services.AddEndpointsApiExplorer();
           // builder.Services.AddSwaggerGen();

            builder.Services.AddEndpointsApiExplorer();
            


            builder.Services.AddScoped<IAccount, AccountService>();

            var app = builder.Build();

           
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
