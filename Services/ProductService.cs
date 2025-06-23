using Microsoft.EntityFrameworkCore;
using PROG6212_New_POE.Data;
using PROG6212_New_POE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG6212_New_POE.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(ProductInputModel input, string userId)
        {
            var product = new Product
            {
                Name = input.Name,
                Category = input.Category,
                Price = input.Price,
                Description = input.Description,
                ProductionDate = input.ProductionDate,
                ApplicationUserId = userId
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProductsForUserAsync(string userId, bool isFarmer)
        {
            if (isFarmer)
            {
                return await _context.Products.Where(p => p.ApplicationUserId == userId).ToListAsync();
            }
            else
            {
                return await _context.Products.Include(p => p.User).ToListAsync();
            }
        }

        public async Task<List<Product>> FilterProductsAsync(DateTime? startDate, DateTime? endDate, string category, string farmerEmail)
        {
            var query = _context.Products.Include(p => p.User).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(p => p.ProductionDate.HasValue && p.ProductionDate.Value >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.ProductionDate.HasValue && p.ProductionDate.Value <= endDate.Value);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Category.ToLower() == category.Trim().ToLower());

            if (!string.IsNullOrWhiteSpace(farmerEmail))
                query = query.Where(p => p.User.Email.ToLower().Contains(farmerEmail.Trim().ToLower()));

            return await query.ToListAsync();
        }
    }
} 