namespace CoffeeShop.ViewModels
{
    public class EmailSettings
    {
        public string AdminEmail { get; set; } = "";
        public string FromEmail { get; set; } = "";
        public string FromName { get; set; } = "";
        public string SmtpHost { get; set; } = "";
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; } = "";
        public string SmtpAppPassword { get; set; } = "";
    }
}