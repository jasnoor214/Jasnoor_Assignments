using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddAzureAppConfiguration("Endpoint=https://application-config-9170.azconfig.io;Id=O6Aq;Secret=BE69Lya3oxmNN92gUDPGd7fWrhGcUFrKwy7z5Q26rxwfEPvmTHrkJQQJ99CDACqBBLyI5rkiAAABAZAC3Aps");

            var dbPassword = builder.Configuration["Common:Settings:DbPassword"];

            // 3. Build the full connection string using that password
            //var connectionString =
            //    $"Server=tcp:nikhil-niksh-9170.database.windows.net,1433;" +
            //    $"Initial Catalog=NikshDB;" +
            //    $"Persist Security Info=False;" +
            //    $"User ID=nikhilnikshadmin;" +
            //    $"Password={dbPassword}" +
            //    $"MultipleActiveResultSets=False;" +
            //    $"Encrypt=True;" +
            //    $"TrustServerCertificate=False;" +
            //    $"Connection Timeout=30;";


            //var connectionString = builder.Configuration.GetConnectionString("AzureSqlConnection"); // here in appsetting u have to give this value //okay from statement 2 okay 
            //builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            //have to uncomment line 30

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
