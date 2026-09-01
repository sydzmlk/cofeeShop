using CoffeeShop.Models;

namespace CoffeeShop.ViewModels
{
    public class ShopVM
    {
        public List<Category> Categories { get; set; } = new();
        public List<Product> Products { get; set; } = new();
    }
}
