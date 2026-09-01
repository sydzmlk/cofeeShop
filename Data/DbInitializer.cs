using CoffeeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.Migrate();

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Coffee" },
                    new Category { Name = "Main Dish" },
                    new Category { Name = "Drinks" },
                    new Category { Name = "Desserts" }
                );
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                var coffee = context.Categories.First(c => c.Name == "Coffee");
                var mainDish = context.Categories.First(c => c.Name == "Main Dish");
                var drinks = context.Categories.First(c => c.Name == "Drinks");
                var desserts = context.Categories.First(c => c.Name == "Desserts");

                context.Products.AddRange(
                    new Product { Name = "Cappuccino", Description = "Tasty cappuccino", Price = 5.90m, CategoryId = coffee.Id, ImageUrl = "/images/menu-1.jpg" },
                    new Product { Name = "Espresso", Description = "Strong espresso", Price = 4.50m, CategoryId = coffee.Id, ImageUrl = "/images/menu-2.jpg" },
                    new Product { Name = "Chicken Sandwich", Description = "Fresh sandwich", Price = 8.20m, CategoryId = mainDish.Id, ImageUrl = "/images/dish-1.jpg" },
                    new Product { Name = "Iced Coffee", Description = "Cold iced coffee", Price = 6.10m, CategoryId = drinks.Id, ImageUrl = "/images/drink-1.jpg" },
                    new Product { Name = "Cheesecake", Description = "Sweet cheesecake", Price = 7.00m, CategoryId = desserts.Id, ImageUrl = "/images/dessert-1.jpg" }
                );

                context.SaveChanges();
            }
        }
    }
}