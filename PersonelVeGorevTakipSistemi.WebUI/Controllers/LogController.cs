using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonelVeGorevTakipSistemi.WebUI.Controllers
{
    // Sadece Yonetici rolunun ozel URL uzerinden erisebilecegi log takip kontrolcusu
    [Authorize(Roles = "Yönetici")]
    [Route("Log")]
    public class LogController : Controller
    {
        // Log sayfasini acan metot
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
