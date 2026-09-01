namespace CoffeeShop.Models
{
    using System;

    namespace CoffeeShop.Models
    {
        public class PasswordResetRequest
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Code { get; set; }
            public DateTime ExpiresAt { get; set; }
            public bool Used { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
    }
}
