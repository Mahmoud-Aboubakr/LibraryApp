using Application.Interfaces.IIdentityService;
using Infrastructure.IdentityServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection IdentityService(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                           .AddEntityFrameworkStores<LibraryDbContext>()
                           .AddDefaultTokenProviders();

            services.AddTransient<IAuthenticateServices, AuthenticateServices>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = config["Token:Audience"],
                    ValidIssuer = config["Token:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]))
                };
            });
            
            return services;
        }
     }
}
