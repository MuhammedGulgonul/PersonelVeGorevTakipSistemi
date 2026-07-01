using System.Linq;
using PersonelVeGorevTakipSistemi.Core.Entities;
using PersonelVeGorevTakipSistemi.DataAccess;
using PersonelVeGorevTakipSistemi.Business.Helpers;

namespace PersonelVeGorevTakipSistemi.Business.Services
{
    // Giriş işlemlerinin iş kurallarını yöneten servis sınıfı
    public class AuthService
    {
        private readonly AppDbContext _context;

        // Constructor ile veritabanı bağlantımızı (AppDbContext) içeriye alıyoruz (Dependency Injection)
        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        // E-posta ve şifreyi kontrol ederek kullanıcıyı doğrular
        public Employee Authenticate(string email, string password)
        {
            // Veritabanında e-posta adresiyle eşleşen ve aktif olan personeli arıyoruz
            var employee = _context.Employees.FirstOrDefault(e => e.Email == email && e.IsActive);
            
            // Eğer personel bulunduysa şifresini kontrol ediyoruz
            if (employee != null)
            {
                bool isPasswordCorrect = PasswordHasher.VerifyPassword(password, employee.PasswordHash);
                if (isPasswordCorrect)
                {
                    return employee; // Bilgiler doğruysa personeli geri dönüyoruz
                }
            }

            return null; // Bilgiler yanlışsa veya personel aktif değilse null dönüyoruz
        }
    }
}
