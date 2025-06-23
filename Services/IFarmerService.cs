using PROG6212_New_POE.Models;
using System.Threading.Tasks;

namespace PROG6212_New_POE.Services
{
    public interface IFarmerService
    {
        Task<bool> AddFarmerAsync(ApplicationUser model);
    }
} 