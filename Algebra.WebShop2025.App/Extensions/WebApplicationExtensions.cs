namespace Algebra.WebShop2025.App.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication Configure(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.MapRazorPages()
           .WithStaticAssets();

        //using var scope = app.Services.CreateScope();
        //var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        //dataSeeder.Seed();

        //using var scope = app.Services.CreateScope();
        //var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //dbContext.Database.Migrate();

        return app;
    }
}
