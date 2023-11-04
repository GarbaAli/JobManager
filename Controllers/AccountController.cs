using AnnonceManager.Models;
using AnnonceManager.Repositories;
using AnnonceManager.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnonceManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;
        private readonly IOfferRepository _offerRepository;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager, IOfferRepository offerRepository)
        {
            _userManager = userManager;
            _signManager = signManager;
            _offerRepository = offerRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CandidatRegister()
        {
            return View();
        }

        //

        [AllowAnonymous]
        [HttpGet]
        public IActionResult EntrepriseRegister()
        {
            return View();
        }

        //


        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //On verifie si l'utilisateur es une entreprise
            var estEntreprise = await _userManager.IsInRoleAsync(user, "Entreprise");

            AccountViewModel model;
            if (estEntreprise)
            {
                var entreprise = (Entreprise)user;
                model = new AccountViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    LibelleEntreprise = entreprise.LibelleEntreprise,
                    AdresseEnt = entreprise.AdresseEnt,
                    Description = entreprise.Description,
                    Pseudo = user.Pseudo,
                    roleName = "Entreprise"
                };
            }
            else
            {
                var candidat = (Candidat)user;
                var OffreCandidats = await _offerRepository.GetOffreForCandidat(candidat.Id);
                model = new AccountViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                   FirstName = candidat.FirstName,
                   LastName = candidat.LastName,
                   Experience = candidat.Experience,
                    Pseudo = user.Pseudo,
                    UserAccount = user,
                    OffreCandidat = OffreCandidats,
                    roleName = "Candidat"

                };
            }
           
            return View(model);
        }

        //


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CandidatRegister(CandidatRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (UniquePseudo(model.Pseudo)) //Si le pseudo existe deja
                {
                    ViewData["notif"] = "Ce Pseudo existe deja veuillez entree un nouveau";
                    return View(model);
                }
                Candidat user = new ()
                {
                    //UserName = GenerateUserName(model.FirstName, model.LastName),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Experience = model.Experience,
                    UserName = model.Email,
                    Email = model.Email,
                    Pseudo = model.Pseudo
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Candidat");
                    await _signManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            ViewData["notif"] = "";
            return View(model);
        }
       
        //

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> EntrepriseRegister(EntrepriseRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (UniquePseudo(model.Pseudo)) //Si le pseudo existe deja
                {
                    ViewData["notif"] = "Ce Pseudo existe deja veuillez entree un nouveau";
                    return View(model);
                }
                Entreprise user = new()
                {
                    //UserName = GenerateUserName(model.FirstName, model.LastName),
                    Description = model.Description,
                    AdresseEnt = model.AdresseEnt,
                    LibelleEntreprise = model.LibelleEntreprise,
                    UserName = model.Email,
                    Email = model.Email,
                    Pseudo = model.Pseudo
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //Si la creation es OK, on relie le candidat au role qui le correspond
                    await _userManager.AddToRoleAsync(user, "Entreprise");
                    await _signManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            ViewData["notif"] = "";
            return View(model);
        }

        //

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(model.Email, model.Password, model.Remember, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(ReturnUrl) && !Url.IsLocalUrl(ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        //Redirige uniquement si l\url es local
                        return LocalRedirect(ReturnUrl);
                    }
                }
                ModelState.AddModelError(string.Empty, "Login Invalid Attempt");
            }
            return View(model);
        }

        //

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //

        [AllowAnonymous]
        [AcceptVerbs("Get", "post")]
        public async Task<IActionResult> CheckingExistingEMail(EntrepriseRegisterViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"This Email {model.Email} is already in use");
            }
        }

        
        
        //Verifier l'unicite du pseudo de chaque utilisateur
        
        public bool UniquePseudo(string pseudo)
        {
            foreach (var user in _userManager.Users)
            {
                if (user.Pseudo.Equals(pseudo))
                {
                    return true;
                }
            }
            return false;
           
        }

        // Modifier Entreprise

        [HttpGet]
        public async Task<IActionResult> EditEntreprise(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var esEntreprise = await _userManager.IsInRoleAsync(user, "Entreprise");
                    if (esEntreprise)
                    {
                        var entreprise = (Entreprise)user;
                        EntrepriseEditViewModel model = new EntrepriseEditViewModel()
                        {
                            AdresseEnt = entreprise.AdresseEnt,
                            LibelleEntreprise = entreprise.LibelleEntreprise,
                            Description = entreprise.Description,
                            Password = user.PasswordHash,
                            ConfirmPassword = user.PasswordHash,
                            Pseudo = user.Pseudo
                            
                        };
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //

        [HttpPost]
        public async Task<IActionResult> Edit(EntrepriseEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    var entreprise = (Entreprise)user;
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        //Acher le password
                        var passwordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                       

                        if (passwordHash != user.PasswordHash)
                        {
                            entreprise.Pseudo = model.Pseudo;
                            entreprise.PasswordHash = passwordHash;
                            entreprise.LibelleEntreprise = model.LibelleEntreprise;
                            entreprise.Description = model.Description;
                            entreprise.AdresseEnt = model.AdresseEnt;
                        }
                    }
                    else
                    {
                        entreprise.Pseudo = model.Pseudo;
                        entreprise.LibelleEntreprise = model.LibelleEntreprise;
                        entreprise.Description = model.Description;
                        entreprise.AdresseEnt = model.AdresseEnt;
                    }


                    IdentityResult result = await _userManager.UpdateAsync(entreprise);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Detail", new { id = model.Id });
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }


            }

            return View(model);
        }
    }
}
