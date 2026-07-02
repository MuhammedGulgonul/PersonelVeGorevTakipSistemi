using System.Linq;
using PersonelVeGorevTakipSistemi.Core.Entities;
using PersonelVeGorevTakipSistemi.DataAccess;
using PersonelVeGorevTakipSistemi.Business.Helpers;

namespace PersonelVeGorevTakipSistemi.Business.Services
{
    // Giriş ve hesap işlemlerinin iş kurallarını yöneten servis sınıfı
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        // E-posta ve şifreyi kontrol ederek kullanıcıyı doğrular
        public Employee Authenticate(string email, string password)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Email == email && e.IsActive);
            
            if (employee != null)
            {
                bool isPasswordCorrect = PasswordHasher.VerifyPassword(password, employee.PasswordHash);
                if (isPasswordCorrect)
                {
                    return employee;
                }
            }

            return null;
        }

        // Kullanıcının şifresini günceller
        public bool ChangePassword(int employeeId, string currentPassword, string newPassword)
        {
            var employee = _context.Employees.Find(employeeId);
            
            if (employee != null && PasswordHasher.VerifyPassword(currentPassword, employee.PasswordHash))
            {
                employee.PasswordHash = PasswordHasher.HashPassword(newPassword);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        // Şifre sıfırlama talebi oluşturur
        public bool RequestPasswordReset(string email)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Email == email && e.IsActive);
            
            if (employee != null)
            {
                // Şifre sıfırlama isteğini aktif hale getiriyoruz
                employee.IsPasswordResetRequested = true;
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        // Yönetici tarafından personelin şifresini sıfırlar
        public bool ResetPassword(int employeeId)
        {
            var employee = _context.Employees.Find(employeeId);
            
            if (employee != null)
            {
                // Şifreyi varsayılan "123456" olarak güncelliyor ve sıfırlama talebini kapatıyoruz
                employee.PasswordHash = PasswordHasher.HashPassword("123456");
                employee.IsPasswordResetRequested = false;
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
