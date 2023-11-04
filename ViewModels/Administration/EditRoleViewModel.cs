using System.ComponentModel.DataAnnotations;

namespace AnnonceManager.ViewModels.Administration
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Enter The New Role")]
        [MinLength(3)]
        public string RoleName { get; set; } = string.Empty;

        public List<string> Users { get; set; } = new List<string>();
    }
}
