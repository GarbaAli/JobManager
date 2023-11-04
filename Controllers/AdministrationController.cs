using AnnonceManager.Models;
using AnnonceManager.ViewModels.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnnonceManager.Controllers
{
    
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult List()
        {
            var rols = _roleManager.Roles;
            return View(rols);
        }

        //

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("List", "Administration");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        //

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var rol = await _roleManager.FindByIdAsync(id);
            if (rol is null)
            {
                return NotFound();
            }

            EditRoleViewModel model = new EditRoleViewModel()
            {
                Id = rol.Id,
                RoleName = rol.Name,
                Users = new List<string>()
            };
            foreach (ApplicationUser user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, model.RoleName))
                {
                    model.Users.Add(user.Email);
                }
            }
            return View(model);
        }

        // Editer Un Role

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role is null)
                {
                    return NotFound();
                }
                role.Name = model.RoleName;
                IdentityResult result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("List", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        //Ajouter et retirer un Role a un User

        [HttpGet]
        public async Task<IActionResult> AddRemoveRoleUser(string RoleId)
        {
            if (!string.IsNullOrEmpty(RoleId))
            {
                var role = await _roleManager.FindByIdAsync(RoleId);
                if (role != null)
                {
                    List<AddRemoveUserRole> models = new List<AddRemoveUserRole>();
                    foreach (ApplicationUser user in _userManager.Users)
                    {
                        AddRemoveUserRole model = new AddRemoveUserRole();
                        model.UserId = user.Id;
                        model.UserName = user.UserName;
                        model.Pseudo = user.Pseudo;
                        model.IsSelected = false;

                        if (await _userManager.IsInRoleAsync(user, role.Name))
                        {
                            model.IsSelected = true;
                        }
                        models.Add(model);
                    }
                    ViewBag.Role = RoleId;
                    ViewBag.Name = role.Name;
                    return View(models);
                }
            }
            return RedirectToAction("List", "Administration");
        }

        //

        [HttpPost]
        public async Task<IActionResult> AddRemoveRoleUser(List<AddRemoveUserRole> model, string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role != null)
            {
                IdentityResult result = null;
                for (int i = 0; i < model.Count; i++)
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(model[i].UserId);
                    if (await _userManager.IsInRoleAsync(user, role.Name) && !model[i].IsSelected)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else if (!await _userManager.IsInRoleAsync(user, role.Name) && model[i].IsSelected)
                    {
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }
            }
            return RedirectToAction("Edit", new { id = RoleId });
        }
    }
}
