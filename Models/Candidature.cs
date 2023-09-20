namespace AnnonceManager.Models
{
    public class Candidature
    {
        public string? CandidatId { get; set; }
        public int OffreId { get; set; }
        public DateTime CandDate { get; set; }
        public int Status { get; set; }



        //Candidat - Offre
        public Offre Offre { get; set; } = new Offre();
        public Candidat Candidat { get; set; } = new Candidat();
    }
}
