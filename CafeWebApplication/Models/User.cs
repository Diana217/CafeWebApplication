using Microsoft.AspNetCore.Identity;
namespace CafeWebApplication.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
