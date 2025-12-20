using System.ComponentModel.DataAnnotations;

namespace SporSalonuYonetim.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad Soyad gereklidir.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "E-mail adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-mail adresi giriniz.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Şifre onayı gereklidir.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = null!;

        // Kullanıcı adı (UserName) olarak genellikle Email kullanacağız ama yine de ekleyelim
        public string? UserName { get; set; }
    }
}