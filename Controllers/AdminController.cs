using CoffeeShop.Data;
using CoffeeShop.Models;
using CoffeeShop.Models.CoffeeShop.Models;
using CoffeeShop.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CoffeeShop.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private static readonly string[] AllowedRoles = { "Admin", "AssistantAdmin", "Editor" };

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login() => View();

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.AdminUsers
                .FirstOrDefaultAsync(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Username və ya password səhvdir";
                return View();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(
            string username,
            [FromServices] EmailSender emailSender,
            [FromServices] IConfiguration config)
        {
            var user = await _context.AdminUsers.FirstOrDefaultAsync(x => x.Username == username);

            if (user != null)
            {
                var code = Generate6DigitCode();
                var minutes = int.TryParse(config["ResetSettings:CodeExpiresMinutes"], out var m) ? m : 10;

                var req = new PasswordResetRequest
                {
                    Username = username,
                    Code = code,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(minutes),
                    Used = false
                };

                _context.PasswordResetRequests.Add(req);
                await _context.SaveChangesAsync();

                var adminEmail = config["EmailSettings:AdminEmail"];
                emailSender.Send(
                    adminEmail,
                    "CoffeeShop Reset Code",
                    $"Username: {username}\nKod: {code}\nVaxt: {minutes} dəqiqə"
                );
            }

            ViewBag.Message = "Əgər belə istifadəçi varsa, reset kodu admin emailinə göndərildi.";
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword() => View();

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string username, string code, string newPassword)
        {
            var req = await _context.PasswordResetRequests
                .Where(r => r.Username == username && r.Code == code && !r.Used)
                .OrderByDescending(r => r.Id)
                .FirstOrDefaultAsync();

            if (req == null || req.ExpiresAt < DateTime.UtcNow)
            {
                ViewBag.Error = "Kod yanlışdır və ya vaxtı bitib.";
                return View();
            }

            var user = await _context.AdminUsers.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                ViewBag.Error = "İstifadəçi tapılmadı.";
                return View();
            }

            user.Password = newPassword;
            req.Used = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
        }
        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        public IActionResult Index()
        {
            ViewBag.ProductCount = _context.Products.Count();
            ViewBag.MessageCount = _context.ContactMessages.Count(m => !m.IsDeleted);
            ViewBag.OrderCount = _context.Orders.Count();
            return View();
        }

        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        [HttpGet]
        public IActionResult Orders()
        {
            var orders = _context.Orders
                .AsNoTracking()
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return View(orders);
        }

        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        [HttpGet]
        public IActionResult OrderDetails(int id)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .AsNoTracking()
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }

        [Authorize(Roles = "Admin,AssistantAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderStatus(int id, OrderStatus status)
        {
            var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            order.Status = status;
            _context.SaveChanges();

            return RedirectToAction(nameof(OrderDetails), new { id });
        }


        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        public IActionResult Products()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        [HttpGet]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(Product product)
        {
            ModelState.Remove("Category");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Products));
        }

        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Product product)
        {
            ModelState.Remove("Category");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Products));
        }

        [Authorize(Roles = "Admin,AssistantAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Products));
        }

     
        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        public IActionResult Messages()
        {
            var messages = _context.ContactMessages
                .Where(m => !m.IsDeleted)
                .OrderByDescending(m => m.SentAt)
                .ToList();

            return View(messages);
        }

     
        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            var users = _context.AdminUsers.OrderBy(u => u.Username).ToList();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewBag.Roles = AllowedRoles;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(AdminUser user)
        {
            if (string.IsNullOrWhiteSpace(user.Role) || !AllowedRoles.Contains(user.Role))
                user.Role = "Editor";

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = AllowedRoles;
                return View(user);
            }

            var exists = _context.AdminUsers.Any(x => x.Username == user.Username);
            if (exists)
            {
                ViewBag.Error = "Bu username artıq mövcuddur";
                ViewBag.Roles = AllowedRoles;
                return View(user);
            }

            _context.AdminUsers.Add(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Users));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditUserRole(int id)
        {
            var user = _context.AdminUsers.Find(id);
            if (user == null) return NotFound();

            ViewBag.Roles = AllowedRoles;
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUserRole(int id, string role)
        {
            var user = _context.AdminUsers.Find(id);
            if (user == null) return NotFound();

            if (!AllowedRoles.Contains(role))
                return BadRequest();

            user.Role = role;
            _context.SaveChanges();
            return RedirectToAction(nameof(Users));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            var currentUsername = User.Identity?.Name;

            var user = _context.AdminUsers.Find(id);
            if (user == null) return NotFound();

            if (user.Username == currentUsername)
                return BadRequest("Öz hesabını silə bilməzsən.");

            _context.AdminUsers.Remove(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Users));
        }

        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMessage(int id)
        {
            
            if (!User.IsInRole("Admin"))
            {
                TempData["Error"] = "Mesaj silmək üçün icazəniz yoxdur.";
                return RedirectToAction(nameof(Messages));
            }

            var msg = _context.ContactMessages.FirstOrDefault(x => x.Id == id);
            if (msg == null)
            {
                TempData["Error"] = "Mesaj tapılmadı.";
                return RedirectToAction(nameof(Messages));
            }

            msg.IsDeleted = true;
            msg.DeletedAt = DateTime.Now;

            _context.SaveChanges();

            TempData["Success"] = "Mesaj silindi.";
            return RedirectToAction(nameof(Messages));
        }

        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        [HttpGet]
        public IActionResult ReplyMessage(int id)
        {
            var msg = _context.ContactMessages
                .Include(m => m.Replies)
                .FirstOrDefault(m => m.Id == id && !m.IsDeleted);

            if (msg == null) return NotFound();

            return View(msg);
        }
        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReply(int id, int messageId)
        {
           
            if (!User.IsInRole("Admin"))
            {
                TempData["Error"] = "Bu əməliyyat üçün icazəniz yoxdur.";
                return RedirectToAction(nameof(ReplyMessage), new { id = messageId });
            }

            var reply = _context.ContactReplies.FirstOrDefault(r => r.Id == id && r.ContactMessageId == messageId);
            if (reply == null)
            {
                TempData["Error"] = "Reply tapılmadı.";
                return RedirectToAction(nameof(ReplyMessage), new { id = messageId });
            }

            _context.ContactReplies.Remove(reply);
            _context.SaveChanges();

            TempData["Success"] = "Cavab silindi.";
            return RedirectToAction(nameof(ReplyMessage), new { id = messageId });
        }
        [Authorize(Roles = "Admin,AssistantAdmin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReplyMessage(int id, string subject, string body, [FromServices] EmailSender emailSender)
        {
            var msg = _context.ContactMessages.FirstOrDefault(m => m.Id == id && !m.IsDeleted);
            if (msg == null) return NotFound();

            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
            {
                TempData["Error"] = "Subject və Body boş ola bilməz.";
                return RedirectToAction(nameof(ReplyMessage), new { id });
            }
            if (string.IsNullOrWhiteSpace(msg.Email))
            {
                TempData["Error"] = "Bu mesajda Email yoxdur. Cavab göndərmək mümkün deyil.";
                return RedirectToAction(nameof(ReplyMessage), new { id });
            }

            emailSender.Send(msg.Email, subject, body);

            _context.ContactReplies.Add(new ContactReply
            {
                ContactMessageId = msg.Id,
                Subject = subject,
                Body = body,
                SentAt = DateTime.Now,
                SentBy = User.Identity?.Name
            });

            msg.IsReplied = true;
            msg.RepliedAt = DateTime.Now;

            _context.SaveChanges();

            TempData["Success"] = "Cavab göndərildi.";
            return RedirectToAction(nameof(ReplyMessage), new { id });
        }


        private static string Generate6DigitCode()
        {
      
            var value = RandomNumberGenerator.GetInt32(100000, 1000000);
            return value.ToString();
        }
    }
}