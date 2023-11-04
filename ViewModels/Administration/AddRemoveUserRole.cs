namespace AnnonceManager.ViewModels.Administration
{
    public class AddRemoveUserRole
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Pseudo { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
