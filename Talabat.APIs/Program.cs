using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Security.Cryptography;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helper;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Interfaces;
using Talabat.Repository.Data.Context;
using Talabat.Repository.Repositories;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services Works With DI
            // Add services to the container.

            builder.Services.AddControllers();


            //----------------------------------------------------------------------------------------------------
            //------------------------ Databases -------------------------------

            //Connection DbContext
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            //Connection Redis 
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(connection);
            });


            //Connection Identity
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            //----------------------------------------------------------------------------------------------------



            //-----------------------------------------------------------------------------
            //------------- Services Application (Extension Services) -----------------------

            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddSwaggerServices();

            //----------------------------------------------------------------------------------------------------


            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("MyPolicy", options =>
            //    {
            //        options.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FronEndUrl"]);
            //    });
            //});



            #endregion



            var app = builder.Build();




            #region Update-Database inside main
            
            //StoreDbContext storeDbContext = new StoreDbContext();
            //await storeDbContext.Database.MigrateAsync(); //Update-database

            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider; // DI => Dependaces Injection
            //LoggerFactory
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = services.GetRequiredService<StoreDbContext>();
                await dbContext.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(dbContext);

                var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityDbContext.Database.MigrateAsync();

                var UserManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex.Message, "An Error Occured during apply the Migration");
            } 
            #endregion


            #region Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWares();
            }


            app.UseMiddleware<ExeptionMiddleware>();
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseCors("MyPolicy");

            app.MapControllers(); 
            #endregion

            app.Run();
        }

    }
}
