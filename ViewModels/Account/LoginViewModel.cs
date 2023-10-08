using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AnnonceManager.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adress Email")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Remember Me")]
        public Boolean Remember { get; set; }
    }
}
