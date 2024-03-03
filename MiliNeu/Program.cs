using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using MiliNeu.Utility;
using SendGrid.Extensions.DependencyInjection;
namespace WebApp
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            /*builder.Services.AddRazorPages();*/
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                //Add role func
                .AddRoles<IdentityRole>()

                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            //Email
            builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGridSettings"));
            builder.Services.AddSendGrid(option =>
            {
                option.ApiKey = builder.Configuration.GetSection("SendGridSettings").GetValue<string>("ApiKey");
            });
            builder.Services.AddScoped<IEmailSender, EmailSenderService>();



            var app = builder.Build();

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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            using (var scope = app.Services.CreateScope())
            {

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Customer" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var adminUserConfig = builder.Configuration.GetSection("AdminUser");
                var email = adminUserConfig["Email"];
                var password = adminUserConfig["Password"];

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser();
                    user.Email = email;
                    user.UserName = email;
                    // Get the ApplicationDbContext from the service provider
                    /*var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();*/

                    Cart cart = new Cart(); // Create a new cart

                    cart.User = user;
                    cart.ApplicationUserId = user.Id;
                    user.Cart = cart;
                    user.CartId = cart.Id;

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Admin");

                }


            }

            app.Run();
        }
    }
}