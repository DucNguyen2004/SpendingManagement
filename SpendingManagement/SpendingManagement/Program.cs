using SpendingManagement.DAOs;
using SpendingManagement.Models;
using SpendingManagement.Repositories;
using SpendingManagement.Services;

namespace SpendingManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();
            // Add services to the container.
            builder.Services.AddRazorPages();
            
            builder.Services.AddDbContext<SpendingManagementContext>();
            builder.Services.AddScoped<UserDAO>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<UserService>();

            //builder.Services.AddScoped<WalletDAO>();
            //builder.Services.AddScoped<WalletRepository>();
            //builder.Services.AddScoped<WalletService>();

            builder.Services.AddScoped<WalletDAO>();
            builder.Services.AddScoped<WalletRepository>();
            builder.Services.AddScoped<WalletService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
