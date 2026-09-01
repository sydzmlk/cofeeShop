using System.ComponentModel.DataAnnotations;
using CoffeeShop.Models;

namespace CoffeeShop.ViewModels
{
    public class CheckoutVM
    {
        
        [Required, MaxLength(120)]
        public string FullName { get; set; } = "";

        [Required, EmailAddress, MaxLength(120)]
        public string Email { get; set; } = "";

        [Required, MaxLength(30)]
        public string Phone { get; set; } = "";

        [Required, MaxLength(300)]
        public string Address { get; set; } = "";

        [MaxLength(500)]
        public string? Notes { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CashOnDelivery;

     
        public List<CartItemVM> Items { get; set; } = new();
        public decimal Total => Items.Sum(x => x.Subtotal);
    }
}