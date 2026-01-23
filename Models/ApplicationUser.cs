using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace lab4.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        
    }
}
