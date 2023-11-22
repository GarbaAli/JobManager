using AnnonceManager.Models;
using System.ComponentModel.DataAnnotations;

namespace AnnonceManager.ViewModels.Offres
{
    public class CandidatureViewModel
    {
        [Required]
        public string? CandidatId { get; set; }
        [Required]
        public int OffreId { get; set; }
        public DateTime CandidatureDate { get; set; }
        [Required]
        public bool DejaCandidat { get; set; }
        public int statusCandidature { get; set; }
        public Offre OffreDetail { get; set; } = new Offre();

    }
}
