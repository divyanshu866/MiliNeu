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
        public DbSet<Product>? Products { get; set; }
        public DbSet<HeroSection>? HeroSections { get; set; }
        public DbSet<HeroSectionImage>? HeroSectionImages { get; set; }

        public DbSet<VariantImage>? VariantImages { get; set; }
        public DbSet<ProductVariant>? ProductVariant { get; set; }
        public DbSet<Color>? Colors { get; set; }
        public DbSet<CollectionImage>? CollectionImages { get; set; }
        public DbSet<Collection>? Collections { get; set; }
        public DbSet<Cart>? Carts { get; set; }

        public DbSet<CartItem>? CartItems { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderIssue>? OrderIssues { get; set; }
        public DbSet<Address>? Address { get; set; }
        public DbSet<OrderAddress>? OrderAddress { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<VisitorLog> VisitorLogs { get; set; }
        public DbSet<PaymentStatusAudit> PaymentStatusAudits { get; set; }

        //Seed Initial Data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            /*            // OrderIssue and Order relationship
                        modelBuilder.Entity<OrderIssue>()
                            .HasOne(oi => oi.Order)
                            .WithMany(o => o.OrderIssues) // Add a navigation property in the Order model if needed
                            .HasForeignKey(oi => oi.OrderId)
                            .OnDelete(DeleteBehavior.Cascade);

                        // OrderIssue and User relationship
                        modelBuilder.Entity<OrderIssue>()
                            .HasOne(oi => oi.User)
                            .WithMany(u => u.OrderIssues) // Add a navigation property in the User model if needed
                            .HasForeignKey(oi => oi.UserId)
                            .OnDelete(DeleteBehavior.SetNull);*/


            modelBuilder.Entity<PaymentStatusAudit>()
                .HasOne(a => a.Order) // Navigation property
                .WithMany(o => o.PaymentStatusAudits) // Optional: collection in Order
                .HasForeignKey(a => a.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascading delete



            /*................................................................*/
            // Configure cascading delete for OrderItems
            modelBuilder.Entity<Order>()                        //Delete all OrderItems when Order is deleted
                .HasMany(o => o.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);



            // Configure cascading delete for ShippingAddress
            modelBuilder.Entity<Order>()
             .HasOne(o => o.ShippingAddress)
             .WithMany() // No navigation back to Order in OrderAddress
             .HasForeignKey(o => o.ShippingAddressId)
             .OnDelete(DeleteBehavior.Cascade); // Optional: avoid cascading deletes

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillingAddress)
                .WithMany() // No navigation back to Order in OrderAddress
                .HasForeignKey(o => o.BillingAddressId)
                .OnDelete(DeleteBehavior.NoAction); // Optional: avoid cascading deletes
            /*................................................................*/




            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<ApplicationUser>(u => u.CartId);
            // Define relationships for ProductVariant and CartItem

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.ProductVariant)
                .WithMany()
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<OrderItem>()
                .HasOne(ci => ci.ProductVariant)
                .WithMany()
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderItem>()
            .HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.NoAction);


            //Seed Collections
            modelBuilder.Entity<Collection>().HasData(
                new Models.Collection
                {
                    Id = 1,
                    Name = "Bloom & Breeze",
                    Category = "Summer",
                    Description = "Description",
                    Price = (decimal)50000.34765,
                    ReleaseDate = DateTime.Now

                },
                new Models.Collection
                {
                    Id = 2,
                    Name = "Tribal Terra",
                    Category = "Winter",
                    Description = "Description",
                    Price = (decimal)65000.7894,
                    ReleaseDate = DateTime.Now


                }


                );
            // Seeding colors with basic details
            modelBuilder.Entity<Color>().HasData(
                new Color { Id = 1, Name = "Red", HexCode = "#FF0000" },
                new Color { Id = 2, Name = "Green", HexCode = "#00FF00" },
                new Color { Id = 3, Name = "Blue", HexCode = "#0000FF" },
                new Color { Id = 4, Name = "Black", HexCode = "#000000" },
                new Color { Id = 5, Name = "White", HexCode = "#FFFFFF" },
                new Color { Id = 6, Name = "Yellow", HexCode = "#FFFF00" },
                new Color { Id = 7, Name = "Purple", HexCode = "#800080" },
                new Color { Id = 8, Name = "Orange", HexCode = "#FFA500" },
                new Color { Id = 9, Name = "Pink", HexCode = "#FFC0CB" },
                new Color { Id = 10, Name = "Brown", HexCode = "#A52A2A" }
            );  // Seed ProductVariants
            modelBuilder.Entity<ProductVariant>().HasQueryFilter(d => !d.IsDiscontinued).HasData(
                new ProductVariant { Id = 1, ColorId = 1, ProductId = 1, IsDiscontinued = false, isMain = true },
                new ProductVariant { Id = 2, ColorId = 2, ProductId = 1, IsDiscontinued = false, isMain = false },
                new ProductVariant { Id = 3, ColorId = 3, ProductId = 2, IsDiscontinued = false, isMain = true },
                new ProductVariant { Id = 4, ColorId = 4, ProductId = 3, IsDiscontinued = false, isMain = true },
                new ProductVariant { Id = 5, ColorId = 5, ProductId = 4, IsDiscontinued = false, isMain = true },
                new ProductVariant { Id = 6, ColorId = 6, ProductId = 4, IsDiscontinued = false, isMain = false },
                new ProductVariant { Id = 7, ColorId = 7, ProductId = 5, IsDiscontinued = false, isMain = true },
                new ProductVariant { Id = 8, ColorId = 8, ProductId = 6, IsDiscontinued = false, isMain = true },
                new ProductVariant { Id = 9, ColorId = 9, ProductId = 7, IsDiscontinued = false, isMain = true },
                new ProductVariant { Id = 10, ColorId = 10, ProductId = 7, IsDiscontinued = false, isMain = false },
                new ProductVariant { Id = 11, ColorId = 1, ProductId = 8, IsDiscontinued = false, isMain = true },
                new ProductVariant { Id = 12, ColorId = 2, ProductId = 9, IsDiscontinued = false, isMain = true },
                new ProductVariant { Id = 13, ColorId = 3, ProductId = 10, IsDiscontinued = false, isMain = true }

            );
            //Seed Collection Image
            modelBuilder.Entity<CollectionImage>().HasData(
               new CollectionImage { Id = 1, Path = "BloomB.jpg", CollectionId = 1, IsMain = true },
               new CollectionImage { Id = 2, Path = "TribalT.jpg", CollectionId = 2, IsMain = true }


            );

            //Seed Products
            //Seed Products
            modelBuilder.Entity<Product>().HasQueryFilter(d => !d.IsDiscontinued).HasData(

                new Product { Id = 1, Name = "A-line Dress", Category = "Casual Wear", Price = 5000.00m, DiscountedPrice = 3900.00m, SizeChartPath = "SizeChart1.JPG", ReleaseDate = DateTime.Now, Description = "Floral Midi Dress – A breezy, mid-length dress featuring a vibrant floral print, perfect for casual outings or summer events.", CollectionId = 1, SalesCount = 0, IsDiscontinued = false },
                new Product { Id = 2, Name = "Maxi Dress", Category = "Casual Wear", Price = 5000.00m, DiscountedPrice = 4800.00m, SizeChartPath = "SizeChart1.JPG", ReleaseDate = DateTime.Now, Description = "Lace Evening Gown – A sophisticated full-length gown with delicate lace details, ideal for formal occasions and black-tie events.", CollectionId = 2, SalesCount = 0, IsDiscontinued = false },
                new Product { Id = 3, Name = "Sheath Dress", Category = "Formal Wear", Price = 7000.00m, DiscountedPrice = 2700.00m, SizeChartPath = "SizeChart1.JPG", ReleaseDate = DateTime.Now, Description = "Bohemian Maxi Dress – Flowing and relaxed, this floor-length dress boasts boho-inspired patterns and an effortless silhouette.", CollectionId = 1, SalesCount = 0, IsDiscontinued = false },
                new Product { Id = 4, Name = "Shift Dress", Category = "Formal Wear", Price = 8000.00m, DiscountedPrice = 5600.00m, SizeChartPath = "SizeChart1.JPG", ReleaseDate = DateTime.Now, Description = "Shift Dress – A simple, straight-cut dress that falls loosely from the shoulders, great for a chic, minimalistic look.", CollectionId = 2, SalesCount = 0, IsDiscontinued = false },
                new Product { Id = 5, Name = "Wrap Dress", Category = "Evening Wear", Price = 4800.00m, DiscountedPrice = 4500.00m, SizeChartPath = "SizeChart1.JPG", ReleaseDate = DateTime.Now, Description = "Bodycon Dress – A form-fitting dress that hugs your curves, making it a sleek option for nights out or parties.", CollectionId = 1, SalesCount = 0, IsDiscontinued = false },
                new Product { Id = 6, Name = "Bodycon Dress", Category = "Evening Wear", Price = 4000.00m, DiscountedPrice = 3400.00m, SizeChartPath = "SizeChart1.JPG", ReleaseDate = DateTime.Now, Description = "A-Line Cocktail Dress – A flattering dress with a fitted bodice and a flared skirt, perfect for semi-formal occasions.", CollectionId = 2, SalesCount = 0, IsDiscontinued = false },
                new Product { Id = 7, Name = "Peplum Dress", Category = "SumAthleisuremer", Price = 2000.00m, DiscountedPrice = 3300.00m, SizeChartPath = "SizeChart1.JPG", ReleaseDate = DateTime.Now, Description = "Shirt Dress – A casual dress designed like a button-down shirt, offering both comfort and versatility.", CollectionId = 1, SalesCount = 0, IsDiscontinued = false },
                new Product { Id = 8, Name = "Empire Waist Dress", Category = "Athleisure", Price = 5000.00m, DiscountedPrice = 4100.00m, SizeChartPath = "SizeChart1.JPG", ReleaseDate = DateTime.Now, Description = "Wrap Dress – A classic design featuring a wrap-around style that cinches at the waist, providing a flattering fit.", CollectionId = 2, SalesCount = 0, IsDiscontinued = false },
                new Product { Id = 9, Name = "Fit and Flare Dress", Category = "Party Wear", Price = 2000.00m, DiscountedPrice = 2100.00m, SizeChartPath = "SizeChart1.JPG", ReleaseDate = DateTime.Now, Description = "Off-Shoulder Ruffle Dress – A playful dress with an off-the-shoulder neckline and ruffle details, great for a stylish yet fun look.", CollectionId = 1, SalesCount = 0, IsDiscontinued = false },
                new Product { Id = 10, Name = "Tunic Dress", Category = "Party Wear", Price = 9000.00m, DiscountedPrice = 2900.00m, SizeChartPath = "SizeChart1.JPG", ReleaseDate = DateTime.Now, Description = "Embroidered Tunic Dress – A relaxed tunic dress adorned with intricate embroidery, perfect for a bohemian or artistic vibe.", CollectionId = 2, SalesCount = 0, IsDiscontinued = false }

            );

            modelBuilder.Entity<VariantImage>().HasData(
               new VariantImage { Id = 1, Path = "1.JPG", ProductVariantId = 1, IsMain = true },
               new VariantImage { Id = 2, Path = "2.JPG", ProductVariantId = 2, IsMain = true },
               new VariantImage { Id = 3, Path = "3.WEBP", ProductVariantId = 3, IsMain = true },
               new VariantImage { Id = 4, Path = "4.JPG", ProductVariantId = 4, IsMain = true },
               new VariantImage { Id = 5, Path = "5.JPG", ProductVariantId = 5, IsMain = true },
               new VariantImage { Id = 6, Path = "6.JPG", ProductVariantId = 6, IsMain = true },
               new VariantImage { Id = 7, Path = "7.JPG", ProductVariantId = 7, IsMain = true },
               new VariantImage { Id = 8, Path = "8.JPG", ProductVariantId = 8, IsMain = true },
               new VariantImage { Id = 9, Path = "9.JPG", ProductVariantId = 9, IsMain = true },
               new VariantImage { Id = 10, Path = "10.WEBP", ProductVariantId = 10, IsMain = true },
               new VariantImage { Id = 11, Path = "11.JPG", ProductVariantId = 11, IsMain = true },
               new VariantImage { Id = 12, Path = "12.JPG", ProductVariantId = 12, IsMain = true },
               new VariantImage { Id = 13, Path = "13.JPG", ProductVariantId = 13, IsMain = true },
               new VariantImage { Id = 14, Path = "14.JPG", ProductVariantId = 1, IsMain = false },
               new VariantImage { Id = 15, Path = "15.JPG", ProductVariantId = 2, IsMain = false },
               new VariantImage { Id = 16, Path = "16.JPG", ProductVariantId = 3, IsMain = false },
               new VariantImage { Id = 17, Path = "17.JPG", ProductVariantId = 4, IsMain = false },
               new VariantImage { Id = 18, Path = "18.JPG", ProductVariantId = 5, IsMain = false },
               new VariantImage { Id = 19, Path = "19.JPG", ProductVariantId = 6, IsMain = false },
               new VariantImage { Id = 20, Path = "20.JPG", ProductVariantId = 7, IsMain = false },
               new VariantImage { Id = 21, Path = "21.JPG", ProductVariantId = 8, IsMain = false },
               new VariantImage { Id = 22, Path = "22.JPG", ProductVariantId = 9, IsMain = false },
               new VariantImage { Id = 23, Path = "23.JPG", ProductVariantId = 10, IsMain = false },
               new VariantImage { Id = 24, Path = "24.JPG", ProductVariantId = 11, IsMain = false }

            );

            //Seed HeroSection
            modelBuilder.Entity<HeroSection>().HasData(

                new HeroSection { Id = 1, Title = "BESTSELLERS", Link = "/Products/BestSellers", IsActive = true, HeroSectionImageId = 1 },
                new HeroSection { Id = 2, Title = "SALE", Link = "/Products/Sale", IsActive = true, HeroSectionImageId = 2 },
                new HeroSection { Id = 3, Title = "SHOP", Link = "/Products/Index", IsActive = true, HeroSectionImageId = 3 }



            );
            //Seed HeroSectionImage
            modelBuilder.Entity<HeroSectionImage>().HasData(

                new HeroSectionImage { Id = 1, Path = "sydney-sweeney.JPG" },
                new HeroSectionImage { Id = 2, Path = "sabrina-carpenter.JPG" },
                new HeroSectionImage { Id = 3, Path = "sydney-sweeney2.JPG" }



            );
            /*modelBuilder.Entity<Address>().HasData(
               new Address
               {
                   Id = 1,
                   UserId = "fddc1dd1-9bfb-45e6-971f-67a18db045e3",
                   FirstName = "John",
                   LastName = "Doe",
                   StreetAddress = "123 Main St",
                   ApartmentSuite = "4B",
                   City = "New York",
                   State = "NY",
                   PostalCode = "10001",
                   Country = "USA",
                   PhoneNumber = "123-456-7890",
                   Email = "johndoe@gmail.com",
                   IsPrimary = true
               },
               new Address
               {
                   Id = 2,
                   UserId = "fddc1dd1-9bfb-45e6-971f-67a18db045e3",
                   FirstName = "Jane",
                   LastName = "Smith",
                   StreetAddress = "456 Oak Avenue",
                   ApartmentSuite = "101",
                   City = "Los Angeles",
                   State = "CA",
                   PostalCode = "90001",
                   Country = "USA",
                   PhoneNumber = "987-654-3210",
                   Email = "janesmith@hotmail.com",
                   IsPrimary = false
               },
               new Address
               {
                   Id = 3,
                   UserId = "fddc1dd1-9bfb-45e6-971f-67a18db045e3",
                   FirstName = "Mark",
                   LastName = "Taylor",
                   StreetAddress = "789 Elm Street",
                   ApartmentSuite = "304",
                   City = "Chicago",
                   State = "IL",
                   PostalCode = "60601",
                   Country = "USA",
                   PhoneNumber = "642-987-8764",
                   Email = "RaneBrown@yahoo.com",
                   IsPrimary = false
               }
            );*/

        }


    }

}