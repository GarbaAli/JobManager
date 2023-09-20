namespace AnnonceManager.Models
{
    public class Candidat:ApplicationUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Experience { get; set; }

        //Candidat - Offre
        public ICollection<Candidature> OffreLink { get; set; } = new List<Candidature>();
    }
}
