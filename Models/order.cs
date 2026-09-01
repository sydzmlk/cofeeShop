using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; }

        [Required, MaxLength(120)]
        public string FullName { get; set; } = "";

        [Required, MaxLength(120)]
        public string Email { get; set; } = "";

        [Required, MaxLength(30)]
        public string Phone { get; set; } = "";

        [Required, MaxLength(300)]
        public string Address { get; set; } = "";

        [MaxLength(500)]
        public string? Notes { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CashOnDelivery;

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<OrderItem> Items { get; set; } = new();
    }
}