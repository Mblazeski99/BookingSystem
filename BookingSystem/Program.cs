using BookingSystem.DataAccess;
using BookingSystem.Extensions;
using BookingSystem.Services;
using BookingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IDataStore, BookingSystemDataStore>();
            builder.Services.AddSingleton<IManagerService, ManagerService>();
            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();

            string key = "bookingSystemKey1234";
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    
                    //
                    ValidateIssuer = false,
                    //

                    ValidateAudience = false,
                };
            });

            builder.Services.AddSingleton<JWTAuthenticationManager>(new JWTAuthenticationManager(key));

            var app = builder.Build();

            app.ConfigureExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Index}");

            app.Run();
        }
    }
}