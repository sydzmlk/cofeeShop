using CoffeeShop.Data;
using CoffeeShop.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Controllers
{
    public class CartController : Controller
    {
        private const string CartKey = "CART_V1";
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = GetCart();

            var productIds = cart.Keys.ToList();

            var products = _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToList();

            var items = products.Select(p => new CartItemVM
            {
                ProductId = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Quantity = cart[p.Id]
            }).ToList();

            return View(items);
        }


        [HttpGet]
        public IActionResult Count()
        {
            var cart = GetCart();
            var count = cart.Values.Sum();
            return Json(new { count });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int productId)
        {
            var cart = GetCart();

            if (cart.ContainsKey(productId))
                cart.Remove(productId);

            SaveCart(cart);

            var count = cart.Values.Sum();
            return Json(new { success = true, count });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeQty(int productId, int delta)
        {
            var cart = GetCart();

            if (!cart.ContainsKey(productId))
                return Json(new { success = true, count = cart.Values.Sum() });

            cart[productId] += delta;

            if (cart[productId] <= 0)
                cart.Remove(productId);

            SaveCart(cart);

            var count = cart.Values.Sum();
            return Json(new { success = true, count });
        }  


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int productId, int qty = 1)
        {
            
            var exists = _context.Products.Any(p => p.Id == productId);
            if (!exists) return NotFound();

            if (qty < 1) qty = 1;

            var cart = GetCart();
            cart.TryGetValue(productId, out var current);
            cart[productId] = current + qty;

            SaveCart(cart);

            var count = cart.Values.Sum();
            return Json(new { success = true, count });
        }

        private Dictionary<int, int> GetCart()
        {
            return HttpContext.Session.GetObject<Dictionary<int, int>>(CartKey)
                   ?? new Dictionary<int, int>();
        }

        private void SaveCart(Dictionary<int, int> cart)
        {
            HttpContext.Session.SetObject(CartKey, cart);
        }
    }
}