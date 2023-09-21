using System.ComponentModel.DataAnnotations;

namespace AnnonceManager.Models
{
    public class Candidat:ApplicationUser
    {
        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string LastName { get; set; } = string.Empty;
        [Range(1, 20)]
        public int Experience { get; set; }

        //Candidat - Offre
        public ICollection<Candidature> OffreLink { get; set; } = new List<Candidature>();
    }
}
