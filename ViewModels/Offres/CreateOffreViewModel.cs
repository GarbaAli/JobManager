using AnnonceManager.Models;
using System.ComponentModel.DataAnnotations;

namespace AnnonceManager.ViewModels.Offres
{
    public class CreateOffreViewModel
    {
        [MaxLength(255)]
        [Display(Name ="Intituler du poste")]
        public string IntitulePoste { get; set; } = string.Empty;
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Lieu { get; set; } = string.Empty;
        [Required]
        public int Salaire { get; set; }
        [Required]
        public DateTime DateLine { get; set; }
    }
}
