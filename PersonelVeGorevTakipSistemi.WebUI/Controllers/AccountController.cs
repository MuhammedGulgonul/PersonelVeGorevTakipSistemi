using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonelVeGorevTakipSistemi.Business.Services;
using PersonelVeGorevTakipSistemi.WebUI.Models;
using PersonelVeGorevTakipSistemi.DataAccess;
using PersonelVeGorevTakipSistemi.WebUI.Helpers;

namespace PersonelVeGorevTakipSistemi.WebUI.Controllers
{
    // Giriş, çıkış ve yetkilendirme yönlendirmelerini yöneten kontrolcü sınıfı
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly AppDbContext _context;

        public AccountController(AuthService authService, AppDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // Giriş yapma ekranını gösteren metot
        [HttpGet]
        public IActionResult Login()
        {
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var employee = _authService.Authenticate(model.Email, model.Password);

            if (employee == null)
            {
                ModelState.AddModelError("", "E-posta adresi veya şifre hatalı.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Name, employee.FirstName + " " + employee.LastName),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, employee.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity)
            );

            // Basarili giris log kaydi
            AuditLogger.LogAction(_context, HttpContext, "Giriş/Çıkış", "Sisteme giriş yapıldı", $"Kullanıcı Rolü: {employee.Role}");

            return RedirectToAction("Index", "Home");
        }

        // Oturumu kapatan metot
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Cikis log kaydi (SignOut yapilmadan once bilgiler HttpContext'ten okunabilsin diye once logluyoruz)
            AuditLogger.LogAction(_context, HttpContext, "Giriş/Çıkış", "Sistemden çıkış yapıldı");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // Şifre değiştirme ekranını gösteren metot
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // Şifre değiştirme formundan gelen verileri işleyen metot
        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Giriş yapmış kullanıcının Id bilgisini alıyoruz
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Şifre değiştirme servisini çağırıyoruz
            bool result = _authService.ChangePassword(userId, model.CurrentPassword, model.NewPassword);

            if (result)
            {
                // Şifre değişim log kaydı
                AuditLogger.LogAction(_context, HttpContext, "Giriş/Çıkış", "Şifre başarıyla değiştirildi");

                // Başarılı uyarısı göstermek için TempData kullanıyoruz
                TempData["SuccessMessage"] = "Şifreniz başarıyla güncellenmiştir.";
                return RedirectToAction("Index", "Home");
            }

            // Eski şifre yanlışsa hata mesajı ekliyoruz
            ModelState.AddModelError("", "Eski şifreniz hatalı.");
            return View(model);
        }

        // Şifremi unuttum ekranını gösteren metot
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            // Kullanıcı zaten giriş yapmışsa şifremi unuttum sayfasına gitmesin
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Şifremi unuttum talebini işleyen metot
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // E-posta adresi üzerinden şifre sıfırlama talebi oluşturuyoruz
            bool result = _authService.RequestPasswordReset(model.Email);

            if (result)
            {
                // Şifre sıfırlama talebi log kaydı
                AuditLogger.LogAction(_context, HttpContext, "Giriş/Çıkış", $"Şifre sıfırlama talebi oluşturuldu", $"Talep Edilen E-posta: {model.Email}");

                // Talep başarılıysa ekranda göstereceğimiz başarı mesajını hazırlıyoruz
                ViewBag.SuccessMessage = "Şifre sıfırlama talebiniz yöneticiye iletilmiştir. Geçici şifrenizi öğrenmek için lütfen yöneticinizle iletişime geçin.";
                return View();
            }

            // E-posta adresi yanlışsa hata gösteriyoruz
            ModelState.AddModelError("", "Bu e-posta adresiyle kayıtlı aktif bir personel bulunamadı.");
            return View(model);
        }
    }
}
