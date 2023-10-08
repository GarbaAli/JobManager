using AnnonceManager.Models;
using AnnonceManager.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnnonceManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult CandidatRegister()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult EntrepriseRegister()
        {
            return View();
        }

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
            //var revendications = await _userManager.GetClaimsAsync(user);
            var model = new AccountViewModel()
            {
                UserAccount = user,

                //    Claims = revendications
            };
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CandidatRegister(CandidatRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Candidat user = new ()
                {
                    //UserName = GenerateUserName(model.FirstName, model.LastName),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Experience = model.Experience,
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
       
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> EntrepriseRegister(EntrepriseRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Entreprise user = new()
                {
                    //UserName = GenerateUserName(model.FirstName, model.LastName),
                    Description = model.Description,
                    AdresseEnt = model.AdresseEnt,
                    LibelleEntreprise = model.LibelleEntreprise,
                    UserName = model.Username,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }



        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


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
    }
}
