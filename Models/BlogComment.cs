using System;
using System.Collections.Generic;

namespace CoffeeShop.Models
{
    public class BlogComment
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost? BlogPost { get; set; }

        public int? ParentCommentId { get; set; }
        public BlogComment? ParentComment { get; set; }
        public List<BlogComment> Replies { get; set; } = new();

        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsAdminReply { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
