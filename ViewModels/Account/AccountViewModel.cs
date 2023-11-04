using AnnonceManager.Models;
using System.ComponentModel.DataAnnotations;

namespace AnnonceManager.ViewModels.Account
{
    public class AccountViewModel:ApplicationUser
    {
        public ApplicationUser UserAccount { get; set; } = new ApplicationUser();
        public string roleName { get; set; } = string.Empty;

        //Candidat
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Experience { get; set; }
        public IEnumerable<Offre>? OffreCandidat { get; set; } 

        //Entreprise
        public string LibelleEntreprise { get; set; } = string.Empty;
        public string AdresseEnt { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
