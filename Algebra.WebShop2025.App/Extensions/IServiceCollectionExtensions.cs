using Algebra.WebShop2025.App.Data;
using Algebra.WebShop2025.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Algebra.WebShop2025.App.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, 
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(1);
            options.Cookie.Name = ".Algebra.WebShop2025.Session";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        var inContainer = bool.TryParse(configuration["DOTNET_RUNNING_IN_CONTAINER"], out bool value) && value;

        if (environment.IsDevelopment() && !inContainer)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTIONSTRING")));
        }

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<ApplicationUser>(options => {
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminWithCredit", policy => policy
                .RequireAuthenticatedUser()
                .RequireRole("Admin")
            ///
            );
        });

        services.AddControllersWithViews();

        services.AddScoped<DataSeeder>();

        return services;
    }
}
