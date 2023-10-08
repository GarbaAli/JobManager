using AnnonceManager.Models;

namespace AnnonceManager.ViewModels.Account
{
    public class AccountViewModel:ApplicationUser
    {
        public ApplicationUser UserAccount { get; set; } = new ApplicationUser();
        public Candidat CandidatAccount { get; set; } = new Candidat();
        public Entreprise EntrepriseAccount { get; set; } = new Entreprise();
    }
}
