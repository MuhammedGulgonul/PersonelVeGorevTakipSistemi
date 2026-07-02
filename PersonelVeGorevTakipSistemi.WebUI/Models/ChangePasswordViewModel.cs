using System.ComponentModel.DataAnnotations;

namespace PersonelVeGorevTakipSistemi.WebUI.Models
{
    // Şifre değiştirme formundan gelen verileri ve doğrulama kurallarını tutan sınıf
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Eski şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre zorunludur.")]
        [MinLength(6, ErrorMessage = "Yeni şifre en az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre tekrarı zorunludur.")]
        [Compare("NewPassword", ErrorMessage = "Yeni şifreler uyuşmuyor.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
