using System.Security.Cryptography;
using System.Text;

namespace PersonelVeGorevTakipSistemi.Business.Helpers
{
    // Şifre şifreleme ve doğrulama işlemlerini yürüten yardımcı sınıf
    public static class PasswordHasher
    {
        // Şifreyi SHA-256 algoritmasıyla şifreler
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Girilen şifrenin doğruluğunu kontrol eder
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }
    }
}
