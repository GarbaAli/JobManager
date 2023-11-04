using AnnonceManager.Models;
using System.ComponentModel.DataAnnotations;

namespace AnnonceManager.ViewModels.Account
{
    public class EntrepriseEditViewModel:Entreprise
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmation do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
