using bowling_tournament_MVCPRoject.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bowling_tournament_MVCPRoject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //NEW ADDITION NECESSARY TO MAKE MVC WORK WITH OUR ARCHITECTURAL SETUP
            //  (Basically tell ASPNet "Hey, our views aren't where you'll expect them. Try the new UI folder!")
            builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/Ui/Views/{1}/{0}.cshtml");
                options.ViewLocationFormats.Add("/Ui/Views/Shared/{0}.cshtml");
            });

            // Adding DB
            builder.Services.AddDbContext<BowlingDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("bowling-tournament-db"))
            );
            builder.Services.AddDbContext<BowlingDbContextV2>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("bowling-tournament-db-v2"))
            );

            builder.Services.AddAuthentication("app-cookie")
            .AddCookie("app-cookie", options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/Denied";
            });
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
