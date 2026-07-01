using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PersonelVeGorevTakipSistemi.Business.Services;
using PersonelVeGorevTakipSistemi.WebUI.Models;

namespace PersonelVeGorevTakipSistemi.WebUI.Controllers
{
    // Giriş, çıkış ve yetkilendirme yönlendirmelerini yöneten kontrolcü sınıfı
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        // Constructor ile giriş servisini (AuthService) içeri alıyoruz
        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        // Giriş yapma ekranını gösteren metot
        [HttpGet]
        public IActionResult Login()
        {
            // Kullanıcı zaten giriş yapmışsa tekrar giriş sayfasını görmesin, ana sayfaya gitsin
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Giriş formundan gelen verileri işleyen metot
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Form verilerinde bir doğrulama hatası (boş bırakma vb.) varsa sayfayı tekrar göster
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kullanıcıyı veritabanında doğrula
            var employee = _authService.Authenticate(model.Email, model.Password);

            if (employee == null)
            {
                // Giriş başarısızsa ekrana hata mesajı ekle ve sayfayı göster
                ModelState.AddModelError("", "E-posta adresi veya şifre hatalı.");
                return View(model);
            }

            // Giriş başarılı: Kullanıcının kimlik bilgilerini (Claims) tanımlıyoruz
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Name, employee.FirstName + " " + employee.LastName),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, employee.Role) // Admin veya Employee rolü
            };

            // Kimlik bilgisini çerez şemasıyla birleştiriyoruz
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Güvenlik çerezini oluşturup kullanıcının oturumunu başlatıyoruz
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity)
            );

            // Başarıyla giriş yapınca ana sayfaya yönlendiriyoruz
            return RedirectToAction("Index", "Home");
        }

        // Oturumu kapatan metot
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Tarayıcıdaki güvenlik çerezini temizliyoruz
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            // Kullanıcıyı giriş sayfasına geri gönderiyoruz
            return RedirectToAction("Login", "Account");
        }
    }
}
