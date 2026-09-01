using System;
using System.Collections.Generic;

namespace CoffeeShop.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Message { get; set; } = "";

        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public bool IsReplied { get; set; } = false;
        public DateTime? RepliedAt { get; set; }

        public List<ContactReply> Replies { get; set; } = new();
    }
}