using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_New_POE.Data;
using PROG6212_New_POE.Models;
using Microsoft.AspNetCore.Identity;

namespace PROG6212_New_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            await _context.SaveChangesAsync();

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
            if (roles.Contains("Farmer"))
            {
                var farmerProducts = await _context.Products
                    .Where(p => p.ApplicationUserId == user.Id)
                    .ToListAsync();
                return View(farmerProducts);
            }

            // Employee sees all by default
            var allProducts = await _context.Products
                .Include(p => p.User)
                .ToListAsync();
            return View(allProducts);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> FilterProducts(
            DateTime? startDate,
            DateTime? endDate,
            string productCategory,
            string farmerEmail)
        {
            var query = _context.Products
                .Include(p => p.User)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(p =>
                    p.ProductionDate.HasValue &&
                    p.ProductionDate.Value >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p =>
                    p.ProductionDate.HasValue &&
                    p.ProductionDate.Value <= endDate.Value);

            // case-insensitive category match
            if (!string.IsNullOrWhiteSpace(productCategory))
            {
                var catFilter = productCategory
                    .Trim()
                    .ToLower();
                query = query.Where(p =>
                    p.Category.ToLower() == catFilter);
            }

            // case-insensitive email contains
            if (!string.IsNullOrWhiteSpace(farmerEmail))
            {
                var emailFilter = farmerEmail
                    .Trim()
                    .ToLower();
                query = query.Where(p =>
                    p.User.Email.ToLower().Contains(emailFilter));
            }

            var filteredList = await query.ToListAsync();
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
                return RedirectToAction(nameof(ViewProducts));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }
    }
}
