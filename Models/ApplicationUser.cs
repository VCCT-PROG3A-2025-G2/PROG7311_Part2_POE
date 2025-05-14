using Microsoft.AspNetCore.Identity;

namespace PROG6212_New_POE.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
  
    }
}
