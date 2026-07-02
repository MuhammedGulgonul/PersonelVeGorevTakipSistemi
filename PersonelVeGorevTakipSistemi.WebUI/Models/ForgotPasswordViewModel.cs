using System.ComponentModel.DataAnnotations;

namespace PersonelVeGorevTakipSistemi.WebUI.Models
{
    // Şifremi unuttum formundan gelen e-posta bilgisini tutan sınıf
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }
    }
}
