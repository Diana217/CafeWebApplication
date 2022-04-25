using Microsoft.AspNetCore.Identity;
namespace CafeWebApplication.Models
{
    public class Role: IdentityRole
    {
        public string Name { get; set; }
    }
}
