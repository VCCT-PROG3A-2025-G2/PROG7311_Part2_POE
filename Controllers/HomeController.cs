using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_New_POE.Data;
using PROG6212_New_POE.Models;
using Microsoft.AspNetCore.Identity;
using PROG6212_New_POE.Services;

namespace PROG6212_New_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductService _productService;
        private readonly IFarmerService _farmerService;

        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IProductService productService,
            IFarmerService farmerService)
        {
            _context = context;
            _userManager = userManager;
            _productService = productService;
            _farmerService = farmerService;
        }

        public IActionResult Index() => View();

        [HttpGet]
        [Authorize(Roles = "Farmer")]
        public IActionResult AddProduct() =>
            View(new ProductInputModel());

        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> AddProduct(ProductInputModel input)
        {
            if (!ModelState.IsValid)
                return View(input);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            await _productService.AddProductAsync(input, user.Id);

            TempData["Success"] = "Product added successfully!";
            return RedirectToAction(nameof(ViewProducts));
        }

        [HttpGet]
        [Authorize(Roles = "Farmer,Employee")]
        public async Task<IActionResult> ViewProducts()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var roles = await _userManager.GetRolesAsync(user);
            bool isFarmer = roles.Contains("Farmer");
            var products = await _productService.GetProductsForUserAsync(user.Id, isFarmer);
            return View(products);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> FilterProducts(
            DateTime? startDate,
            DateTime? endDate,
            string productCategory,
            string farmerEmail)
        {
            var filteredList = await _productService.FilterProductsAsync(startDate, endDate, productCategory, farmerEmail);
            return View(nameof(ViewProducts), filteredList);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public IActionResult AddFarmer() => View();

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AddFarmer(ApplicationUser model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _farmerService.AddFarmerAsync(model);
            if (result)
            {
                TempData["Success"] = "Farmer added successfully!";
                return RedirectToAction(nameof(ViewProducts));
            }

            ModelState.AddModelError(string.Empty, "Failed to add farmer.");
            return View(model);
        }
    }
}
