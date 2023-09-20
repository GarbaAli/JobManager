namespace AnnonceManager.Models
{
    public class Offre
    {
        public int OffreId { get; set; }
        public string IntitulePoste { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Lieu { get; set; } = string.Empty;
        public decimal Salaire { get; set; }
        public DateTime DateLine { get; set; }
        public DateTime CreatedAt { get; set; }

        //les cle etrangeres
        public string? EntId { get; set; }
        public Entreprise Entreprise { get; set; } = new Entreprise();

        //Candidat - Offre
        public ICollection<Candidature> CandidatLink { get; set; } = new List<Candidature>();
    }
}
