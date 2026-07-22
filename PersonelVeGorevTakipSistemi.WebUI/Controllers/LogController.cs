using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PersonelVeGorevTakipSistemi.DataAccess;

namespace PersonelVeGorevTakipSistemi.WebUI.Controllers
{
    // Sadece Yonetici rolunun ozel URL uzerinden erisebilecegi log takip kontrolcusu
    [Authorize(Roles = "Yönetici")]
    [Route("Log")]
    public class LogController : Controller
    {
        private readonly AppDbContext _context;

        public LogController(AppDbContext context)
        {
            _context = context;
        }

        // Log sayfasini acan metot
        [HttpGet]
        public IActionResult Index()
        {
            var logs = _context.Logs.OrderByDescending(l => l.TimeStamp).ToList();
            return View(logs);
        }

        // Tum loglari temizleyen metot
        [HttpPost]
        [Route("Clear")]
        public IActionResult Clear()
        {
            _context.Logs.RemoveRange(_context.Logs);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Sistem hata günlükleri başarıyla temizlenmiştir.";
            return RedirectToAction("Index");
        }
    }
}
