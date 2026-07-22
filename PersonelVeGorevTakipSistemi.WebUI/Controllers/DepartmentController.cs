using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonelVeGorevTakipSistemi.Business.Services;
using PersonelVeGorevTakipSistemi.Core.Entities;
using PersonelVeGorevTakipSistemi.DataAccess;
using PersonelVeGorevTakipSistemi.WebUI.Helpers;

namespace PersonelVeGorevTakipSistemi.WebUI.Controllers
{
    // Sadece Admin rolundeki kullanicilarin erisebilecegi departman yonetim kontrolcusu
    [Authorize(Roles = "Yönetici")]
    public class DepartmentController : Controller
    {
        private readonly DepartmentService _departmentService;
        private readonly AppDbContext _context;

        public DepartmentController(DepartmentService departmentService, AppDbContext context)
        {
            _departmentService = departmentService;
            _context = context;
        }

        // Departmanlari listeleyen sayfa
        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentService.GetAll();
            return View(departments);
        }

        // Departmanı ve bağlı çalışanlarını listeleyen detay sayfası
        [HttpGet]
        public IActionResult Details(int id)
        {
            var department = _departmentService.GetWithEmployees(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // Yeni departman ekleme sayfasini acan metot
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Yeni departman ekleme formunu kaydeden metot
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            _departmentService.Add(department);

            // Departman ekleme log kaydi
            AuditLogger.LogAction(_context, HttpContext, "Departman İşlemi", "Yeni departman oluşturuldu", $"Departman Adı: {department.Name}");

            TempData["SuccessMessage"] = "Departman başarıyla eklenmiştir.";
            return RedirectToAction("Index");
        }

        // Departman duzenleme sayfasini acan metot
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = _departmentService.GetById(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // Departman duzenleme formunu kaydeden metot
        [HttpPost]
        public IActionResult Edit(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            _departmentService.Update(department);

            // Departman guncelleme log kaydi
            AuditLogger.LogAction(_context, HttpContext, "Departman İşlemi", "Departman güncellendi", $"Departman ID: {department.Id}, Yeni Adı: {department.Name}");

            TempData["SuccessMessage"] = "Departman başarıyla güncellenmiştir.";
            return RedirectToAction("Index");
        }

        // Departman silme islemini yapan metot
        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool isDeleted = _departmentService.Delete(id);

            if (isDeleted)
            {
                // Departman silme log kaydi
                AuditLogger.LogAction(_context, HttpContext, "Departman İşlemi", "Departman silindi", $"Silinen Departman ID: {id}");

                TempData["SuccessMessage"] = "Departman başarıyla silinmiştir.";
            }
            else
            {
                // Silinememe nedeni bagli aktif personellerin olmasidir
                TempData["ErrorMessage"] = "Bu departmanda aktif çalışan personel bulunduğu için silme işlemi engellendi.";
            }

            return RedirectToAction("Index");
        }
    }
}
