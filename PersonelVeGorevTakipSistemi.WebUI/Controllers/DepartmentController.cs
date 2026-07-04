using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonelVeGorevTakipSistemi.Business.Services;
using PersonelVeGorevTakipSistemi.Core.Entities;

namespace PersonelVeGorevTakipSistemi.WebUI.Controllers
{
    // Sadece Admin rolundeki kullanicilarin erisebilecegi departman yonetim kontrolcusu
    [Authorize(Roles = "Yönetici")]
    public class DepartmentController : Controller
    {
        private readonly DepartmentService _departmentService;

        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // Departmanlari listeleyen sayfa
        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentService.GetAll();
            return View(departments);
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
