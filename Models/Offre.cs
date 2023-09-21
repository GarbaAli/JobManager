using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace AnnonceManager.Models
{
    public class Offre
    {
        [Key]
        public int OffreId { get; set; }
        [MaxLength(255)]
        public string IntitulePoste { get; set; } = string.Empty;
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
        public string Lieu { get; set; } = string.Empty;

        public int Salaire { get; set; }
        public DateTime DateLine { get; set; }
        public DateTime CreatedAt { get; set; }

        //les cle etrangeres
        public string? EntId { get; set; }
        public Entreprise Entreprise { get; set; } = new Entreprise();

        //Candidat - Offre
        public ICollection<Candidature> CandidatLink { get; set; } = new List<Candidature>();
    }
}
