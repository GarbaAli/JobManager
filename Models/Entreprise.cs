using System.ComponentModel.DataAnnotations;

namespace AnnonceManager.Models
{
    public class Entreprise:ApplicationUser
    {
        [Required]
        [MaxLength(255)]
        public string LibelleEntreprise { get; set; } = string.Empty;
        [MaxLength(100)]
        public string AdresseEnt { get; set; } = string.Empty;
        [MinLength(10)]
        public string Description { get; set; } = string.Empty;
        public IEnumerable<Offre> OffreCreer { get; set; } = new List<Offre>();
    }
}
