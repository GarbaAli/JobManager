using AnnonceManager.Models;
using AnnonceManager.Repositories;
using AnnonceManager.ViewModels.Offres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AnnonceManager.Controllers
{
    
    public class OffreController : Controller
    {
        private readonly IOfferRepository _offerRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ISendMail _sendMail;

        public OffreController(IOfferRepository offerRepository, UserManager<ApplicationUser> userManager, AppDbContext context, ISendMail sendMail)
        {
            _offerRepository = offerRepository;
            _userManager = userManager;
            _context = context;
            _sendMail = sendMail;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var offres = await  _offerRepository.GetAll();
            return View(offres);
        }

        [HttpGet]
        [Authorize(Roles = "Entreprise")]
        public async Task<IActionResult> List()
        {
            //Recupere l'id de l'user connecter
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            var offres = await _offerRepository.GetAllByUserId(user.Id);
            if (offres.Count() == 0)
            {
                return NotFound();
            }
            
            return View(offres);
        }

        //

        [Authorize(Roles = "Entreprise")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            return View();
        }

        //
        [Authorize(Roles = "Entreprise")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOffreViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }
                Offre offre = new Offre()
                {
                    IntitulePoste = model.IntitulePoste,
                    Lieu = model.Lieu,
                    Salaire = model.Salaire,
                    DateLine = model.DateLine,
                    EntId = user.Id,
                    Entreprise = (Entreprise)user,
                    Description = model.Description,
                    CreatedAt = DateTime.Now
                };
                await _offerRepository.CreateOffre(offre);
                //Message flah et redirectionn
                TempData["message"] = $"Good! Nouvelle Offre Cree [{offre.IntitulePoste}]";
                TempData["type"] = "success";
                return RedirectToAction("List");
            }
            return View(model);
        }

        //

        [HttpGet]
        public async Task<IActionResult> Detail(int OffreId)
        {
            //On recupere l'offre via son Id
            var offre = await _offerRepository.Get(OffreId);
            // Identifier le candidat en ligne
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (offre != null && user != null)
            {
                CandidatureViewModel model = new CandidatureViewModel()
                {
                    OffreDetail = offre,
                    OffreId = OffreId,
                    DejaCandidat = false
                };                

                //Verifier si l'user en ligne a deja postuler a l'offre
                if (_offerRepository.IsLink(OffreId, user.Id))
                {
                    //POUR UNE OFFRE RECUPERER LE STATUS DE LA CANDIDATURE DE USER EN LIGNE
                    var status = offre.CandidatLink.Where(c => c.CandidatId == user.Id).FirstOrDefault().Status;
                    model.DejaCandidat = true;
                    model.statusCandidature = status;
                }
             
                return View(model);
            }

            return BadRequest();
        }

        //POSTULER OU ANNULER SA CANDIDATURE

        [HttpPost]
        public async Task<IActionResult> PostulerOuAnnuler(CandidatureViewModel model)
        {
            // Identifier le candidat en ligne
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            //On recupere l'offre via son Id
            var offre = await _offerRepository.Get(model.OffreId);
            var candidat = (Candidat)user;

            //Recuperer la potentiel candidature a la suppression
            var linkToRemove = offre.CandidatLink.Where(c => c.Candidat == candidat).FirstOrDefault();

            

            if (user != null && offre != null)
            {
                var candidature = new Candidature()
                {
                    Candidat = candidat,
                    Offre = offre,
                    CandidatId = user.Id,
                    OffreId = offre.OffreId,
                    CandDate = DateTime.Now,
                    Status = 1
                };
                //Si avait deja postuler alors on Annule au cas contraire on lance sa candidature

                //var subject = "";
                var notif = "";
                if (model.DejaCandidat)
                {
                    //On detache l'offre du candidat
                    _context.Remove(linkToRemove);
                    await _context.SaveChangesAsync();
                    model.DejaCandidat = false;
                    notif = "Annuler";
                   // subject = "Annulation de candidature";
                }
                else
                {
                    candidat.OffreLink.Add(candidature);
                    await _context.SaveChangesAsync();
                    model.DejaCandidat = true;
                    notif = "Envoyer";
                    // subject = "Confiirmation de candidature";

                }
                //vu qu'on va etre rediriger vers la page detail je dois predisposer le "model" pour qu'il soit pret a gerer la situation
                //On mappe les donnees vers la ViewModel de la candidature
                  model = new CandidatureViewModel()
                {
                    OffreDetail = offre,
                    OffreId = offre.OffreId,
                    CandidatId = user.Id,
                    CandidatureDate = DateTime.Now,
                };

                //ENVOIE DE MAIL
                //  var to = candidat.Email;
                // var body = $"Hello {candidat.LastName} \n Nous vous confirmons votre candidature au Poste de {offre.IntitulePoste} qui a ete publier le {offre.CreatedAt}";

                //_sendMail.Send(to, subject, body);

                TempData["message"] = $"GOOD! vous venez d'{notif} Votre candidature";
                TempData["type"] = "success";
                return RedirectToAction("Detail", model);
            }
            return BadRequest();
           
        }

        // VALIDER OU REJETER UNE CANDIDATURE. ELLE ENVOIE 2 PARAMETRES (ID OFFRE ET ID CANDIDAT) MAIS ON LES INSERRE DANS LE VIEWMODEL

        [HttpPost]
        [Authorize(Roles = "Entreprise")]
        public async Task<IActionResult> ValiderOrRejeterCandidature(CandidatureViewModel model, int status)
        {
            if (model.OffreId != 0 && model.CandidatId != null)
            {
                //On recupere l'offre via son Id
                var offre = await _offerRepository.Get(model.OffreId);
                //On recupere le candidat 
                ApplicationUser user = await _userManager.FindByIdAsync(model.CandidatId);
                var candidat = (Candidat)user;

                var linkToUpdate = offre.CandidatLink.Where(c => c.Candidat == candidat).FirstOrDefault();
                linkToUpdate.Status = status;
                if (offre != null && candidat != null)
                {
                    var candidature = new Candidature()
                    {
                      Candidat = candidat,
                      Offre = offre,
                      CandidatId = user.Id,
                       OffreId = offre.OffreId,
                     // CandDate = DateTime.Now,
                        Status = status
                    };
                    //On met jour la candidature avec la valeur du statut
                    _context.Update(linkToUpdate);
                    await _context.SaveChangesAsync();

                    //Message flah et redirectionn
                    TempData["message"] = $"Good! Vous savez trancher sur la candidature {candidat.LastName}, ce dernier sera notifier par mail";
                    TempData["type"] = "success";
                    return RedirectToAction("Detail", model);
                }
            }

            TempData["message"] = "Erreur! survenu au niveaau des valeurs de l'offre et du candidat";
            TempData["type"] = "danger";
            return View(model);
        }





    }
}
