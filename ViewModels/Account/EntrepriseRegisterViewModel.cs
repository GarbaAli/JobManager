using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AnnonceManager.ViewModels.Account
{
    public class EntrepriseRegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "CheckingExistingEMail", controller: "Account")] 
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmation do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Display(Name = "Libelle de l'Entreprise")]
        public string LibelleEntreprise { get; set; } = string.Empty;
        [MaxLength(100)]
        [Display(Name ="Adresse de l'Entreprise")]
        public string AdresseEnt { get; set; } = string.Empty;
        [MinLength(10)]
        public string Description { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        [MaxLength(10)]
        [MinLength(3)]
        public string Pseudo { get; set; } = string.Empty;
    }
}
