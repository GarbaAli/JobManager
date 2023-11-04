using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AnnonceManager.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(10)]
        [MinLength(3)]
        public string Pseudo { get; set; } = string.Empty;
    }
}
