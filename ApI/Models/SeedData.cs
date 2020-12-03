using ApI.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApI.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new APIContext
                (serviceProvider.GetRequiredService<DbContextOptions<APIContext>>()))
            {
                if (context.Pages.Any())
                {
                    return;
                }

                context.Pages.AddRange(
                    new Page
                    {
                        Name = "Home",
                        Slug = "home",
                        Content = "Home page"
                    },
                    new Page
                    {
                        Name = "About Us",
                        Slug = "about-us",
                        Content = "About us page"
                    },
                    new Page
                    {
                        Name = "Services",
                        Slug = "services",
                        Content = "Services page"
                    },
                    new Page
                    {
                        Name = "Contact",
                        Slug = "contact",
                        Content = "Contact page"
                    }
                );

                context.Categories.AddRange(
                    new Category
                    {
                        Name = "Fruits",
                        Slug = "fruits"
                    },
                    new Category
                    {
                        Name = "T-shirts",
                        Slug = "t-shirts"
                    }
                );

                context.Products.AddRange(
                    new Product
                    {
                        Name = "White Shirt",
                        CategoryId = 2,
                        Description = "A white t-shirt",
                        Image = "white_shirt.jpg",
                        Price = 2.99M
                    },
                    new Product
                    {
                        Name = "Blue Shirt",
                        CategoryId = 2,
                        Description = "A blue t-shirt",
                        Image = "blue_shirt.jpg",
                        Price = 3.99M
                    },
                    new Product
                    {
                        Name = "Apples",
                        CategoryId = 1,
                        Description = "Nice apples",
                        Image = "apples.jpg",
                        Price = 1.50M
                    },
                    new Product
                    {
                        Name = "Bananas",
                        CategoryId = 1,
                        Description = "Nice bananas",
                        Image = "bananas.jpg",
                        Price = 2.99M
                    },
                    new Product
                    {
                        Name = "Black Shirt",
                        CategoryId = 2,
                        Description = "A black t-shirt",
                        Image = "black_shirt.jpg",
                        Price = 2.99M
                    },
                    new Product
                    {
                        Name = "Grey Shirt",
                        CategoryId = 2,
                        Description = "A grey t-shirt",
                        Image = "grey_shirt.jpg",
                        Price = 3.99M
                    },
                    new Product
                    {
                        Name = "Grapefruit",
                        CategoryId = 1,
                        Description = "Juicy grapefruit",
                        Image = "grapefruit.jpg",
                        Price = 2.50M
                    },
                    new Product
                    {
                        Name = "Grapes",
                        CategoryId = 1,
                        Description = "Nice grapes",
                        Image = "grapes.jpg",
                        Price = 2M
                    },
                    new Product
                    {
                        Name = "Pink Shirt",
                        CategoryId = 2,
                        Description = "A pink t-shirt",
                        Image = "pink_shirt.jpg",
                        Price = 5.99M
                    },
                    new Product
                    {
                        Name = "Yellow Shirt",
                        CategoryId = 2,
                        Description = "A yellow t-shirt",
                        Image = "yellow_shirt.jpg",
                        Price = 6.99M
                    },
                    new Product
                    {
                        Name = "Kiwi",
                        CategoryId = 1,
                        Description = "Fresh kiwi",
                        Image = "kiwi.jpg",
                        Price = 3M
                    },
                    new Product
                    {
                        Name = "Watermelon",
                        CategoryId = 1,
                        Description = "Delicious watermelon",
                        Image = "watermelon.jpg",
                        Price = .50M
                    }
               );

                context.SaveChanges();
            }
        }
    }
}
