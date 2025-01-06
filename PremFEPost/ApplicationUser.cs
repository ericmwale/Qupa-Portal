using Microsoft.AspNetCore.Identity;

namespace PremFEPost
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty ;
        public DateTime CreatedAt { get; set; }  
    }
}
