using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using MiliNeu.Models.Services.Implementations;
using MiliNeu.Models.Services.Interfaces;
using MiliNeu.Utility;
using PaymentGateway;
using UserTracking;

namespace MiliNeu
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Register EmailSender service
            builder.Services.AddTransient<ISendGridService, SendGridService>();

            //Register RazorPay service
            builder.Services.AddTransient<IRazorpayPaymentService, RazorpayPaymentService>();

            //Register OrderService service
            builder.Services.AddScoped<IOrderService, OrderService>();

            //Register OrderService service
            builder.Services.AddScoped<ICartService, CartService>();

            //Register CartService service
            builder.Services.AddScoped<IProductService, ProductService>();

            //Register CollectionService service
            builder.Services.AddScoped<ICollectionService, CollectionService>();

            /*
                        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                        builder.Services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseSqlServer(connectionString));
                        */
            // Add services to the container.
            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)

                 .AddRoles<IdentityRole>()/*Added Role Func*/

                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();





            // ** Add session services here **
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30); // Set the session timeout
                options.Cookie.HttpOnly = true; // Prevent client-side access
                options.Cookie.IsEssential = true; // Make the session cookie essential
            });





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
            app.UseSession(); // Ensure session is enabled before the tracking middleware
            app.UseMiddleware<UserTrackingMiddleware>(); // Add the user tracking middleware

            app.UseAuthentication();/*Added*/
            app.UseAuthorization();

            /* Define routing for areas */
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // Define custom area routing

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            /*Custom Code....................................................*/
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
                    user.Id = "fddc1dd1-9bfb-45e6-971f-67a18db045e3";
                    user.Email = email;
                    user.UserName = email;
                    user.EmailConfirmed = true;
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

            app.MapRazorPages();

            app.Run();
        }
    }
}
