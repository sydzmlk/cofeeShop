using System;

namespace CoffeeShop.Models
{
    public class ContactReply
    {
        public int Id { get; set; }

        public int ContactMessageId { get; set; }
        public ContactMessage ContactMessage { get; set; } = null!;

        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";

        public DateTime SentAt { get; set; } = DateTime.Now;
        public string? SentBy { get; set; }
    }
}
