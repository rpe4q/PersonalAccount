using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Mappers;
using PersonalAccount.Models;
using PersonalAccount.Repositories;
using PersonalAccount.Services.Account;
using PersonalAccount.Services.Cabinet;
using PersonalAccount.Services.Confirmation;
using PersonalAccount.Services.Db;

namespace PersonalAccount
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    // options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ExpireTimeSpan =
                        TimeSpan.FromDays(int.Parse(builder.Configuration["Auth:ExpireTimeInDays"]!));
                    options.SlidingExpiration = true;
                });
            builder.Services.AddAuthorization();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDefaultConnection")));

            // Options
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
            builder.Services.Configure<DbBootstrapSettings>(builder.Configuration.GetSection("DbBootstrap"));

            // Services
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IConfirmationTokenService, ConfirmationTokenService>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            if (builder.Environment.IsDevelopment())
                builder.Services.AddScoped<DbBootstrap>();

            // Cabinet Services
            builder.Services.AddScoped<IStudentCabinetService, StudentCabinetService>();
            builder.Services.AddScoped<IAdminCabinetService, AdminCabinetService>();

            // Repositories
            builder.Services.AddScoped<IAccountRepo, AccountRepo>();
            builder.Services.AddScoped<IStudentProfileRepo, StudentProfileRepo>();
            builder.Services.AddScoped<IConfirmationTokenRepo, ConfirmationTokenRepo>();

            // Mappers
            builder.Services.AddSingleton<IMapper<AccountEntity, AccountModel>, AccountMapper>();
            builder.Services.AddSingleton<IMapper<StudentProfileEntity, StudentProfileModel>, StudentProfileMapper>();
            builder.Services
                .AddSingleton<IMapper<ConfirmationTokenEntity, ConfirmationTokenModel>, ConfirmationTokenMapper>();

            // Others
            builder.Services.AddSingleton<IPasswordHasher<AccountModel>, PasswordHasher<AccountModel>>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var seeder = scope.ServiceProvider.GetRequiredService<DbBootstrap>();
                await seeder.SeedAsync();
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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            await app.RunAsync();
        }
    }
}