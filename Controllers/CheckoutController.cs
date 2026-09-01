using CoffeeShop.Data;
using CoffeeShop.Models;
using CoffeeShop.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;
        private const string CartKey = "CART_V1";

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

      
        public IActionResult Index()
        {
            var items = BuildCartItems();
            if (!items.Any())
                return RedirectToAction("Index", "Cart");

            var vm = new CheckoutVM
            {
                Items = items
            };

           
            return View(vm);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CheckoutVM vm)
        {
            var items = BuildCartItems();
            vm.Items = items;

            if (!items.Any())
            {
                ModelState.AddModelError("", "Your cart is empty.");
                return View(vm);
            }

            if (!ModelState.IsValid)
                return View(vm);

            // Order yarat
            var order = new Order
            {
                FullName = vm.FullName,
                Email = vm.Email,
                Phone = vm.Phone,
                Address = vm.Address,
                Notes = vm.Notes,
                PaymentMethod = vm.PaymentMethod,
                TotalAmount = items.Sum(x => x.Subtotal),
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var ci in items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = ci.ProductId,
                    ProductNameSnapshot = ci.Name,
                    UnitPrice = ci.Price,
                    Quantity = ci.Quantity
                });
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

           
            HttpContext.Session.Remove(CartKey);

            return RedirectToAction(nameof(Success), new { id = order.Id });
        }

      
        public IActionResult Success(int id)
        {
            var order = _context.Orders
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    o.Id,
                    o.FullName,
                    o.TotalAmount,
                    o.CreatedAt
                })
                .FirstOrDefault();

            if (order == null) return NotFound();

            ViewBag.OrderId = order.Id;
            ViewBag.FullName = order.FullName;
            ViewBag.Total = order.TotalAmount;
            ViewBag.CreatedAt = order.CreatedAt;

            return View();
        }

        private List<CartItemVM> BuildCartItems()
        {
            var cart = HttpContext.Session.GetObject<Dictionary<int, int>>(CartKey)
                       ?? new Dictionary<int, int>();

            if (!cart.Any()) return new List<CartItemVM>();

            var ids = cart.Keys.ToList();

            var products = _context.Products
                .Where(p => ids.Contains(p.Id))
                .Select(p => new { p.Id, p.Name, p.Price, p.ImageUrl })
                .ToList();

            var items = products.Select(p => new CartItemVM
            {
                ProductId = p.Id,
                Name = p.Name,
                ImageUrl = string.IsNullOrWhiteSpace(p.ImageUrl) ? "/images/menu-1.jpg" : p.ImageUrl,
                Price = p.Price,
                Quantity = cart[p.Id]
            }).ToList();

            return items;
        }
    }
}