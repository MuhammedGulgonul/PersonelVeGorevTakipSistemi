using System.ComponentModel.DataAnnotations;

namespace PersonelVeGorevTakipSistemi.WebUI.Models
{
    // Giriş ekranında kullanıcının dolduracağı formu temsil eden sınıf
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
