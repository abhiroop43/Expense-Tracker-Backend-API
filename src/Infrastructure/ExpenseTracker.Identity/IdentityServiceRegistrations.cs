using System.Text;
using AspNetCore.Identity.MongoDbCore.Models;
using DotNetEnv;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Models.Identity;
using ExpenseTracker.Identity.Models;
using ExpenseTracker.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace ExpenseTracker.Identity;

public static class IdentityServiceRegistrations
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        Env.TraversePath().Load();

        var connectionString = configuration.GetConnectionString("ExpenseTrackerConnection");
        var databaseName = configuration.GetSection("DatabaseName").Value;

        services.AddIdentity<ApplicationUser, MongoIdentityRole<ObjectId>>()
            .AddMongoDbStores<ApplicationUser, MongoIdentityRole<ObjectId>, ObjectId>
            (
                connectionString, databaseName
            )
            .AddDefaultTokenProviders();

        services.AddTransient<IAuthService, AuthService>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Env.GetString("JWT_SECRET_KEY")))
                };
            });

        return services;
    }
}