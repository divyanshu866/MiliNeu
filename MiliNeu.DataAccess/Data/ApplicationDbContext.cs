using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiliNeu.Models;
namespace MiliNeu.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product>? Product { get; set; }
        public DbSet<Collection>? Collection { get; set; }
        public DbSet<Cart>? Cart { get; set; }
        public DbSet<Order>? Order { get; set; }

        //Seed Initial Data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<ApplicationUser>(u => u.CartId);

            //Seed Collections
            modelBuilder.Entity<Collection>().HasData(
                new Models.Collection
                {
                    Id = 1,
                    Name = "Bloom & Breeze",
                    Category = "Summer",
                    Description = "Description",
                    Price = (decimal)50000.34765,
                    Path = ""

                },
                new Models.Collection
                {
                    Id = 2,
                    Name = "Tribal Terra",
                    Category = "Winter",
                    Description = "Description",
                    Price = (decimal)65000.7894,
                    Path = ""
                }
                );

            //Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Asymmetric Hem Slip Dress",
                    Category = "Summer",
                    Description = "Description",
                    Price = (decimal)12000.6789,
                    Path = "11.JPG",
                    CollectionId = 1,
                    /*CollectionName = "Bloom & Breeze"*/

                },
                new Product
                {
                    Id = 2,
                    Name = "Draped Jersey Dress",
                    Category = "Summer",
                    Description = "Description",
                    Price = (decimal)13000.98765,
                    Path = "12.JPG",
                    CollectionId = 1,
                    /*CollectionName = "Bloom & Breeze"*/
                },
                new Product
                {
                    Id = 3,
                    Name = "Blazer",
                    Category = "Summer",
                    Description = "Description",
                    Price = (decimal)24000.5678,
                    Path = "13.JPG",
                    CollectionId = 1,
                    //CollectionName = "Bloom & Breeze"
                },
                new Product
                {
                    Id = 4,
                    Name = "Lace Top",
                    Category = "Summer",
                    Description = "Description",
                    Price = (decimal)16000.6543,
                    Path = "14.JPG",
                    CollectionId = 1,
                    //CollectionName = "Bloom & Breeze"
                },
                new Product
                {
                    Id = 5,
                    Name = "Suite",
                    Category = "Summer",
                    Description = "Description",
                    Price = (decimal)34000.87654,
                    Path = "15.JPG",
                    CollectionId = 1,
                    //CollectionName = "Bloom & Breeze"
                },
                new Product
                {
                    Id = 6,
                    Name = "Suite",
                    Category = "Winter",
                    Description = "Description",
                    Price = (decimal)34000.76543,
                    Path = "1.JPG",
                    CollectionId = 2,
                    //CollectionName = "Tribal Terra"
                },
                new Product
                {
                    Id = 7,
                    Name = "Lace Top",
                    Category = "Winter",
                    Description = "Description",
                    Price = (decimal)16000.9876,
                    Path = "2.JPG",
                    CollectionId = 2,
                    //CollectionName = "Tribal Terra"
                },
                new Product
                {
                    Id = 8,
                    Name = "Blazer",
                    Category = "Winter",
                    Description = "Description",
                    Price = (decimal)24000.87654,
                    Path = "3.JPG",
                    CollectionId = 2,
                    //CollectionName = "Tribal Terra"
                },
                new Product
                {
                    Id = 9,
                    Name = "Draped Jersey Dress",
                    Category = "Winter",
                    Description = "Description",
                    Price = (decimal)13000.9349,
                    Path = "4.JPG",
                    CollectionId = 2,
                    //CollectionName = "Tribal Terra"
                },
                new Product
                {
                    Id = 10,
                    Name = "Asymmetric Hem Slip Dress",
                    Category = "Winter",
                    Description = "Description",
                    Price = (decimal)12000.6723,
                    Path = "1.JPG",
                    CollectionId = 2,
                    //CollectionName = "Tribal Terra"
                }
                );
        }


    }

}