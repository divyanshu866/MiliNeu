using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
/*namespace WebApp
{
class Program
{
public static async Task Main(string[] args)
{*/
var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                //Add role func
                .AddRoles<IdentityRole>()

                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Collections}/{action=Index}/{id?}");
            app.MapRazorPages();

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
                var email = "dev866@admin";
                var password = "Dev866$";
                
    if (await userManager.FindByEmailAsync(email)==null)
    {
        var user = new ApplicationUser();
        user.Email = email;
        user.UserName = email;
        // Get the ApplicationDbContext from the service provider
        /*var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();*/


        

        Cart cart = new Cart(); // Create a new cart
       
        {
            cart.User = user;
            cart.ApplictaionUserId = user.Id;
            
        };
        user.Cart = cart;
        user.CartId = cart.Id;
       
        

        await userManager.CreateAsync(user,password);
        await userManager.AddToRoleAsync(user, "Admin");

    }

                
            }

app.Run();
      /*  }
    }
}*/