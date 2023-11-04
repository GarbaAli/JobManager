using AnnonceManager.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnonceManager.Repositories
{
    public class OffreRepository:IOfferRepository
    {
        private readonly AppDbContext _dbContext;
        public OffreRepository(AppDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task CreateOffre(Offre offre)
        {
            _dbContext.Offres.Add(offre);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOffre(int id)
        {
            var offre = await _dbContext.Offres.FindAsync(id);
            if (offre != null)
            {
                _dbContext.Offres.Remove(offre);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Offre> Get(int id)
        {
            return await _dbContext.Offres
                .Include(o => o.CandidatLink)
                .ThenInclude(c => c.Candidat)
                .SingleOrDefaultAsync(o => o.OffreId == id);
        }

        //

        public async Task<IEnumerable<Offre>> GetAll()
        {
            return await _dbContext.Offres.Include(o => o.Entreprise).ToListAsync();
        }

        //

        public async Task<IEnumerable<Offre>> GetAllByUserId(string id)
        {
            return await _dbContext.Offres.Include(o => o.CandidatLink).Where(offre => offre.EntId == id).ToListAsync();
        }

        //RECUPERER POUR UN CANDIDAT TOUTES LES OFFRES AUX QUEL ILS A POSTULER

        public async Task<IEnumerable<Offre>> GetOffreForCandidat(string CandId)
        {
            return await _dbContext.Offres.Include(o => o.CandidatLink)
                .ThenInclude(c => c.Candidat)
                .Where(o => o.CandidatLink.Any(c => c.CandidatId == CandId)).ToListAsync();
        }

        //VERIFIE SI UNE OFFRE DEFINI ES LIER A UN CANDIDAT DEFINI

        public bool IsLink(int OffreId, string userId)
        {
            var result = _dbContext.Offres.Where(o => o.OffreId == OffreId)
                .Include(o => o.CandidatLink)
                .Where(o => o.CandidatLink.Any(c => c.CandidatId == userId))
                .FirstOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<Offre> UpdateOffre(int id)
        {
            throw new NotImplementedException();
        }
    }
}
