using Gift_Purchase_Store.Data;
using Gift_Purchase_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;

namespace Gift_Purchase_Store.Controllers
{
    [Authorize(Roles ="Customer")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private Repository<Product> products;
        private Repository<Order> _orders;
        private Repository<Ingredient> ingredients;
        private Repository<Category> categories;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            products = new Repository<Product>(context);
            ingredients = new Repository<Ingredient>(context);
            categories = new Repository<Category>(context);
            _orders = new Repository<Order>(context);
        }

        //View Product 
        public async Task<IActionResult> ProductView()
        {
            return View(await products.GetAllAsync());
        }


        //View Types
        public async Task<IActionResult> TypeView()
        {
            return View(await ingredients.GetAllAsync());
        }

        //Create Order

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            //ViewBag.Products = await _products.GetAllAsync();

            //Retrieve or create an OrderViewModel from session or other state management
            var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await products.GetAllAsync()
            };


            return View(model);
        }

        //Order Functionalities
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> AddItem(int prodId, int prodQty)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var product = await _context.Products.FindAsync(prodId);
            if (product == null) return NotFound();

            // Retrieve or create user-specific OrderViewModel
            var model = HttpContext.Session.Get<OrderViewModel>(userId) ?? new OrderViewModel
            {
                OrderItems = new List<OrderItemViewModel>(),
                Products = await products.GetAllAsync()
            };

            // Add or update product in the order
            var existingItem = model.OrderItems.FirstOrDefault(item => item.ProductId == prodId);
            if (existingItem != null)
            {
                existingItem.Quantity += prodQty;
            }
            else
            {
                model.OrderItems.Add(new OrderItemViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Quantity = prodQty,
                    Price = product.Price
                });
            }

            // Update total amount
            model.TotalAmount = model.OrderItems.Sum(item => item.Price * item.Quantity);

            // Save back to session
            HttpContext.Session.Set(userId, model);

            return RedirectToAction("CreateOrder", model);
        }


        [HttpGet]
        [Authorize]
        [HttpGet]
        [Authorize]
        public IActionResult Cart()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var model = HttpContext.Session.Get<OrderViewModel>(userId);
            if (model == null || !model.OrderItems.Any())
            {
                return RedirectToAction("CreateOrder");
            }

            return View(model);
        }


        [HttpPost]
        [Authorize]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var model = HttpContext.Session.Get<OrderViewModel>(userId);
            if (model == null || !model.OrderItems.Any())
            {
                return RedirectToAction("CreateOrder");
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = model.TotalAmount,
                UserId = userId,
                OrderItems = model.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            await _orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // Clear user's cart
            HttpContext.Session.Remove(userId);

            return RedirectToAction("PaymentSuccess", new { orderId = order.OrderId });
        }

        [HttpGet]
        [Authorize]
        public IActionResult PaymentSuccess(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return View("Error", new ErrorViewModel { RequestId = "Order not found." });
            }

            var model = new OrderViewModel
            {
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList(),
                PaymentMethod = "Cash on Delivery"
            };

            return View(model);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewOrders()
        {
            var userId = _userManager.GetUserId(User);

            var userOrders = await _orders.GetAllByIdAsync(userId, "UserId", new QueryOptions<Order>
            {
                Includes = "OrderItems.Product"
            });

            return View(userOrders);
        }

        //PAyment functionality

        
       
    

        //view secific user

         [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            // Get the logged-in user's ID
            var userId = _userManager.GetUserId(User);

            // Retrieve orders specific to the logged-in user, including related data
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product) // Include product details
                .Where(o => o.UserId == userId) // Filter by user ID
                .ToListAsync();

            // Convert to a ViewModel for display
            var orderViewModels = orders.Select(o => new OrderViewModel
            {
                TotalAmount = o.TotalAmount,
                OrderItems = o.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            });

            return View(orderViewModels);
        }
    }

}


