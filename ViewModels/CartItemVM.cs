namespace CoffeeShop.ViewModels
{
    public class CartItemVM
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Subtotal => Price * Quantity;
    }
}
