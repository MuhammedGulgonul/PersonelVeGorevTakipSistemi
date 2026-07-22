using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PersonelVeGorevTakipSistemi.Core.Entities;
using PersonelVeGorevTakipSistemi.DataAccess;

namespace PersonelVeGorevTakipSistemi.WebUI.Helpers
{
    // Sistemdeki kullanici hareketlerini (giriş, çıkış, görev güncelleme, dosya silme vb.) veritabanina kaydeden yardimci sinif
    public static class AuditLogger
    {
        public static void LogAction(AppDbContext dbContext, HttpContext httpContext, string actionType, string message, string details = "")
        {
            try
            {
                string userName = "Sistem / Ziyaretçi";
                string userEmail = "bilinmiyor";

                // Giris yapmis kullanicinin bilgilerini aliyoruz
                if (httpContext?.User?.Identity != null && httpContext.User.Identity.IsAuthenticated)
                {
                    userName = httpContext.User.Identity.Name;
                    userEmail = httpContext.User.FindFirst(ClaimTypes.Email)?.Value ?? "bilinmiyor";
                }

                var log = new Log
                {
                    LogLevel = actionType, // Islem Grubu (Giris/Cikis, Görev Islemi, Personel Islemi, vb.)
                    Message = message,     // Yapilan islemin ozeti
                    Exception = $"İşlemi Yapan: {userName} ({userEmail})\nDetay: {details}", // Islemi yapan ve ekstra bilgiler
                    TimeStamp = DateTime.Now
                };

                dbContext.Logs.Add(log);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                // Log yazma sirasindaki olasi hatalar ana uygulamayi kilitlememelidir
            }
        }
    }
}
