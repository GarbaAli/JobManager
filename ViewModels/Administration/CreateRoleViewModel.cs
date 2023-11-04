using System.ComponentModel.DataAnnotations;

namespace AnnonceManager.ViewModels.Administration
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Entree le libelle du nouveau Role")]
        [MinLength(3)]
        public string RoleName { get; set; }
    }
}
