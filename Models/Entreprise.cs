namespace AnnonceManager.Models
{
    public class Entreprise:ApplicationUser
    {
        public string LibelleEntreprise { get; set; } = string.Empty;
        public string AdresseEnt { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IEnumerable<Offre> OffreCreer { get; set; } = new List<Offre>();
    }
}
