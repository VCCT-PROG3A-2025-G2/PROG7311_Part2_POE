using Microsoft.AspNetCore.Mvc;
using PROG6212_New_POE.Data;
using PROG6212_New_POE.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace PROG6212_New_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Farmer")]
        public IActionResult AddProduct()
        {
            return View(new ProductInputModel());
        }

        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> AddProduct(ProductInputModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    var product = new Product
                    {
                        Name = input.Name,
                        Category = input.Category,
                        Price = input.Price,
                        Description = input.Description,
                        ProductionDate = input.ProductionDate,
                        ApplicationUserId = user.Id
                    };
                    _context.Products.Add(product);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        TempData["Success"] = "Product added successfully!";
                        return RedirectToAction(nameof(ViewProducts));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to save the product.");
                    }
                }
                return View(input);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in AddProduct: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while saving the product.");
                return View(input);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Farmer,Employee")]
        public async Task<IActionResult> ViewProducts()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                if (userRole == "Farmer")
                {
                    var products = await _context.Products
                        .Where(p => p.ApplicationUserId == user.Id)
                        .ToListAsync();
                    
                    // Debug information
                    Console.WriteLine($"Found {products.Count} products for farmer {user.Id}");
                    foreach (var product in products)
                    {
                        Console.WriteLine($"Product: {product.Name}, ID: {product.ProductId}, UserID: {product.ApplicationUserId}");
                    }
                    
                    return View(products);
                }
                else if (userRole == "Employee")
                {
                    var products = await _context.Products
                        .Include(p => p.User)
                        .ToListAsync();
                    
                    // Debug information
                    Console.WriteLine($"Found {products.Count} products for employee view");
                    
                    return View(products);
                }
                
                return View(new List<Product>());
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in ViewProducts: {ex.Message}");
                return View(new List<Product>());
            }
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> FilterProducts(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Products.Include(p => p.User).AsQueryable();
            if (startDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate.HasValue && p.ProductionDate.Value >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate.HasValue && p.ProductionDate.Value <= endDate.Value);
            }
            var products = await query.ToListAsync();
            return View(nameof(ViewProducts), products);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public IActionResult AddFarmer()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AddFarmer(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                var result = await _userManager.CreateAsync(user, "Default123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Farmer");
                    TempData["Success"] = "Farmer added successfully!";
                    return RedirectToAction("ViewProducts");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
    }
}

 