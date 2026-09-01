using CoffeeShop.Models;

namespace CoffeeShop.ViewModels
{
    public class BlogDetailsViewModel
    {
        public BlogPost Blog { get; set; } = new BlogPost();
        public List<BlogComment> Comments { get; set; } = new();
        public BlogComment NewComment { get; set; } = new BlogComment();
    }
}
