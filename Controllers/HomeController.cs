using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CoffeeShop.Data;
using CoffeeShop.Models;
using CoffeeShop.ViewModels;

namespace CoffeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                FeaturedProducts = _context.Products.Take(4).ToList(),
                Services = _context.Services.Take(3).ToList(),
                RecentBlogs = _context.BlogPosts.OrderByDescending(b => b.CreatedAt).Take(3).ToList()
            };
            return View(model);
        }

        public IActionResult Menu()
        {
            var categoriesWithProducts = _context.Categories.Include(c => c.Products).ToList();
            return View(categoriesWithProducts);
        }

        public IActionResult Services()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        public IActionResult About() => View();

        public IActionResult Blog()
        {
            var blogs = _context.BlogPosts.OrderByDescending(b => b.CreatedAt).ToList();
            return View(blogs);
        }

        public IActionResult BlogSingle(int id)
        {
            var blog = _context.BlogPosts.FirstOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }

        public IActionResult ProductSingle(int id)
        {
            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View(new ContactMessage());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactMessage message)
        {
            // binder boş gətirirsə formdan götür
            message.Name = string.IsNullOrWhiteSpace(message.Name) ? Request.Form["Name"].ToString() : message.Name;
            message.Email = string.IsNullOrWhiteSpace(message.Email) ? Request.Form["Email"].ToString() : message.Email;
            message.Subject = string.IsNullOrWhiteSpace(message.Subject) ? Request.Form["Subject"].ToString() : message.Subject;
            message.Message = string.IsNullOrWhiteSpace(message.Message) ? Request.Form["Message"].ToString() : message.Message;

            // hələ də boşdursa – deməli form göndərmir (nested form/JS)
            if (string.IsNullOrWhiteSpace(message.Email) ||
                string.IsNullOrWhiteSpace(message.Name) ||
                string.IsNullOrWhiteSpace(message.Subject) ||
                string.IsNullOrWhiteSpace(message.Message))
            {
                TempData["SuccessMessage"] = null;
                ModelState.AddModelError("", "Form məlumatları serverə gəlmir. Layout-da iç-içə <form> və ya JS reset problemi var.");
                return View(message);
            }

            message.SentAt = DateTime.Now;
            _context.ContactMessages.Add(message);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Mesajınız göndərildi!";
            return RedirectToAction(nameof(Contact));
        }

        [HttpGet]
        public IActionResult Checkout() => View();

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                TempData["Success"] = "Sifarişiniz qəbul olundu!";
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
    }
}