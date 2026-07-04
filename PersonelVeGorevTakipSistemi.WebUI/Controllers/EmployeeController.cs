using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonelVeGorevTakipSistemi.Business.Services;
using PersonelVeGorevTakipSistemi.Core.Entities;

namespace PersonelVeGorevTakipSistemi.WebUI.Controllers
{
    // Sadece Admin rolundeki kullanicilarin erisebilecegi personel yonetim kontrolcusu
    [Authorize(Roles = "Yönetici")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;
        private readonly DepartmentService _departmentService;

        public EmployeeController(EmployeeService employeeService, DepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        // Personelleri listeleyen sayfa
        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employeeService.GetAll();
            return View(employees);
        }

        // Yeni personel ekleme sayfasini acan metot
        [HttpGet]
        public IActionResult Create()
        {
            // Dropdown secimi icin departmanlar listesini ViewBag ile sayfaya gonderiyoruz
            ViewBag.Departments = _departmentService.GetAll();
            return View();
        }

        // Yeni personel ekleme formunu kaydeden metot
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _departmentService.GetAll();
                return View(employee);
            }

            bool isAdded = _employeeService.Add(employee);

            if (isAdded)
            {
                TempData["SuccessMessage"] = "Personel başarıyla eklenmiştir.";
                return RedirectToAction("Index");
            }

            // E-posta zaten varsa hata mesaji donduruyoruz
            ModelState.AddModelError("Email", "Bu e-posta adresi sistemde zaten kayıtlı.");
            ViewBag.Departments = _departmentService.GetAll();
            return View(employee);
        }

        // Personel duzenleme sayfasini acan metot
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Departments = _departmentService.GetAll();
            return View(employee);
        }

        // Personel duzenleme formunu kaydeden metot
        [HttpPost]
        public IActionResult Edit(Employee employee, string newPassword)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _departmentService.GetAll();
                return View(employee);
            }

            bool isUpdated = _employeeService.Update(employee, newPassword);

            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Personel bilgileri başarıyla güncellenmiştir.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Email", "Bu e-posta adresi sistemde zaten kayıtlı.");
            ViewBag.Departments = _departmentService.GetAll();
            return View(employee);
        }

        // Personelin aktiflik/pasiflik durumunu degistiren metot
        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            _employeeService.ToggleStatus(id);
            TempData["SuccessMessage"] = "Personel aktiflik durumu güncellenmiştir.";
            return RedirectToAction("Index");
        }

        // Talep eden personelin sifresini sifirlayan metot
        [HttpPost]
        public IActionResult ResetPassword(int id)
        {
            bool result = _employeeService.ResetPassword(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Personel şifresi başarıyla 123456 olarak sıfırlanmıştır.";
            }
            else
            {
                TempData["ErrorMessage"] = "Personel şifresi sıfırlanamadı.";
            }

            return RedirectToAction("Index");
        }
    }
}
