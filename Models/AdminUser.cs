using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class AdminUser
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        
        [Required]
        public string Role { get; set; } = "Editor";
    }
}