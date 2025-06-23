using PROG6212_New_POE.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PROG6212_New_POE.Services
{
    public interface IProductService
    {
        Task AddProductAsync(ProductInputModel input, string userId);
        Task<List<Product>> GetProductsForUserAsync(string userId, bool isFarmer);
        Task<List<Product>> FilterProductsAsync(DateTime? startDate, DateTime? endDate, string category, string farmerEmail);
    }
} 