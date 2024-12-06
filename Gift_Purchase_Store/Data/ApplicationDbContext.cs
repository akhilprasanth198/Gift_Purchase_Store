using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gift_Purchase_Store.Models;

namespace Gift_Purchase_Store.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define composite key and relationships for ProductIngredient
            modelBuilder.Entity<ProductIngredient>()
                .HasKey(pi => new { pi.ProductId, pi.IngredientId });

            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);

            //Seed Data
            modelBuilder.Entity<Category>().HasData(
               new Category { CategoryId = 1, Name = "Watch" },
               new Category { CategoryId = 2, Name = "Soft Toys" },
               new Category { CategoryId = 3, Name = "Eductional Toys" },
               new Category { CategoryId = 4, Name = "Decoration Items" },
               new Category { CategoryId = 5, Name = "Hats" }
           );

            modelBuilder.Entity<Ingredient>().HasData(
              //add mexican restaurant ingredients here
              new Ingredient { IngredientId = 1, Name = "High-quality plastic" },
              new Ingredient { IngredientId = 2, Name = "Treated Wood" },
              new Ingredient { IngredientId = 3, Name = "Child-safe" },
              new Ingredient { IngredientId = 4, Name = "Stainless steel" },
              new Ingredient { IngredientId = 5, Name = "Baloons" },
              new Ingredient { IngredientId = 6, Name = "Candles" }
          );

            modelBuilder.Entity<Product>().HasData(

                //Add mexican restaurant food entries here
                new Product
                {
                    ProductId = 1,
                    Name = "Teddy Bear Soft Toy",
                    Description = "Fluffy and cuddly teddy bear made with non-toxic, child-safe materials.",
                    Price = 699,
                    Stock = 13500,
                    CategoryId = 2
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Wooden Alphabet Puzzle",
                    Description = "Brightly colored wooden puzzle to teach children letters and improve motor skills.",
                    Price = 349,
                    Stock = 25,
                    CategoryId = 3
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Party Balloons Set ",
                    Description = " Set of 50 balloons in assorted colors, made from biodegradable latex.",
                    Price = 399,
                    Stock = 100,
                    CategoryId = 4
                },
                
            new Product
            {
                ProductId = 4,
                Name = "Happy Birthday Banner",
                Description = "High-quality, reusable Happy Birthday banner made from durable paper and foil.",
                Price = 299,
                Stock = 50,
                CategoryId = 4
            }
                );

            modelBuilder.Entity<ProductIngredient>().HasData(
                new ProductIngredient { ProductId = 1, IngredientId = 3 },
                new ProductIngredient { ProductId = 2, IngredientId = 2 },
                new ProductIngredient { ProductId = 3, IngredientId = 5 },
                new ProductIngredient { ProductId = 3, IngredientId = 3 },
                new ProductIngredient { ProductId = 4, IngredientId = 1 },
                new ProductIngredient { ProductId = 4, IngredientId = 3 }
                );
        }
    
}
}
