namespace SporSalonuYonetim.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public IList<string>? Roles { get; set; } // Kullanıcının rolleri
    }
}
