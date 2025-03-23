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

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<SpendingManagementContext>();
            builder.Services.AddScoped<WalletDAO>();
            builder.Services.AddSingleton<WalletRepository>();
            builder.Services.AddSingleton<WalletService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
