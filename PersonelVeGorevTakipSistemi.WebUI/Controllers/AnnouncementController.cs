using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonelVeGorevTakipSistemi.Business.Services;
using PersonelVeGorevTakipSistemi.Core.Entities;
using PersonelVeGorevTakipSistemi.DataAccess;
using PersonelVeGorevTakipSistemi.WebUI.Helpers;

namespace PersonelVeGorevTakipSistemi.WebUI.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class AnnouncementController : Controller
    {
        private readonly AnnouncementService _announcementService;
        private readonly AppDbContext _context;

        public AnnouncementController(AnnouncementService announcementService, AppDbContext context)
        {
            _announcementService = announcementService;
            _context = context;
        }

        // Yeni duyuru ekleme
        [HttpPost]
        public IActionResult Create(string title, string content)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "Duyuru başlığı ve içeriği boş olamaz.";
                return Redirect(Request.Headers["Referer"].ToString() ?? "/Task");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int employeeId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 1;

            var announcement = new Announcement
            {
                Title = title.Trim(),
                Content = content.Trim(),
                EmployeeId = employeeId
            };

            _announcementService.Add(announcement);

            AuditLogger.LogAction(_context, HttpContext, "Duyuru İşlemi", "Yeni duyuru yayınlandı", $"Başlık: {title}");

            TempData["SuccessMessage"] = "Duyuru başarıyla yayınlanmıştır.";
            return Redirect(Request.Headers["Referer"].ToString() ?? "/Task");
        }

        // Duyuru silme
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _announcementService.Delete(id);

            AuditLogger.LogAction(_context, HttpContext, "Duyuru İşlemi", "Duyuru yayından kaldırıldı", $"Duyuru ID: {id}");

            TempData["SuccessMessage"] = "Duyuru yayından kaldırılmıştır.";
            return Redirect(Request.Headers["Referer"].ToString() ?? "/Task");
        }
    }
}
