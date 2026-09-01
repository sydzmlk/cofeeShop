using System.Collections.Generic;
using CoffeeShop.Models;

namespace CoffeeShop.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> FeaturedProducts { get; set; }
        public List<ServiceItem> Services { get; set; }
        public List<BlogPost> RecentBlogs { get; set; }
    }
}