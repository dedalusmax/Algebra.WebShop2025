using Algebra.WebShop2025.App.Data;
using Algebra.WebShop2025.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Algebra.WebShop2025.App.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(1);
            options.Cookie.Name = ".Algebra.WebShop2025.Session";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

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
