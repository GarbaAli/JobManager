using AnnonceManager.Models;

namespace AnnonceManager.Repositories
{
    public interface IOfferRepository
    {
        Task<IEnumerable<Offre>> GetAll();
        Task<Offre> Get(int id);
         Task CreateOffre(Offre offre);
        Task DeleteOffre(int id);
        Task<Offre> UpdateOffre(int id);
        Task<IEnumerable<Offre>> GetAllByUserId(string id);
        bool IsLink(int OffreId, string userId);
        Task<IEnumerable<Offre>> GetOffreForCandidat(string CandId);

    }
}
