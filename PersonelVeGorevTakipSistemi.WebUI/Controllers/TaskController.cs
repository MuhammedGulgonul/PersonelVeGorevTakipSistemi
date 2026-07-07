using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonelVeGorevTakipSistemi.Business.Services;
using PersonelVeGorevTakipSistemi.Core.Enums;
using Task = PersonelVeGorevTakipSistemi.Core.Entities.Task;

namespace PersonelVeGorevTakipSistemi.WebUI.Controllers
{
    // Giriş yapmış tüm personellerin erişebileceği görev yönetim kontrolcüsü
    [Authorize]
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;
        private readonly EmployeeService _employeeService;
        private readonly DepartmentService _departmentService;

        public TaskController(TaskService taskService, EmployeeService employeeService, DepartmentService departmentService)
        {
            _taskService = taskService;
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        // Giriş yapan kullanıcının veritabanındaki Id değerini döndürür
        private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        // Giriş yapan kullanıcının Yönetici olup olmadığını kontrol eder
        private bool IsYönetici => User.IsInRole("Yönetici");

        // Görevleri Kanban panosu düzeninde listeleyen ve filtreleyen metot
        [HttpGet]
        public IActionResult Index(TaskState? status, TaskPriority? priority, int? departmentId)
        {
            // Kullanıcının rolüne göre görevleri çekiyoruz
            var tasks = IsYönetici ? _taskService.GetAll() : _taskService.GetByEmployeeId(CurrentUserId);

            // Arayüz filtreleme parametrelerini uyguluyoruz
            if (status.HasValue)
            {
                tasks = tasks.Where(t => t.Status == status.Value).ToList();
            }

            if (priority.HasValue)
            {
                tasks = tasks.Where(t => t.Priority == priority.Value).ToList();
            }

            if (departmentId.HasValue)
            {
                tasks = tasks.Where(t => t.Employee != null && t.Employee.DepartmentId == departmentId.Value).ToList();
            }

            // Filtre dropdown'ları için departmanları sayfaya gönderiyoruz
            ViewBag.Departments = _departmentService.GetAll();
            ViewBag.SelectedStatus = status;
            ViewBag.SelectedPriority = priority;
            ViewBag.SelectedDepartmentId = departmentId;

            return View(tasks);
        }

        // Yeni görev ekleme ekranı (Sadece Yönetici)
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsYönetici)
            {
                return RedirectToAction("Index");
            }

            // Görev atanabilecek aktif personelleri listeliyoruz
            ViewBag.Employees = _employeeService.GetAll().Where(e => e.IsActive).ToList();
            return View();
        }

        // Yeni görev ekleme formunu kaydeden metot (Sadece Yönetici)
        [HttpPost]
        public IActionResult Create(Task task)
        {
            if (!IsYönetici)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Employees = _employeeService.GetAll().Where(e => e.IsActive).ToList();
                return View(task);
            }

            _taskService.Add(task);
            TempData["SuccessMessage"] = "Görev başarıyla oluşturulmuş ve personele atanmıştır.";
            return RedirectToAction("Index");
        }

        // Görev düzenleme ekranı (Sadece Yönetici)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsYönetici)
            {
                return RedirectToAction("Index");
            }

            var task = _taskService.GetById(id);
            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Employees = _employeeService.GetAll().Where(e => e.IsActive).ToList();
            return View(task);
        }

        // Görev düzenleme formunu kaydeden metot (Sadece Yönetici)
        [HttpPost]
        public IActionResult Edit(Task task)
        {
            if (!IsYönetici)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Employees = _employeeService.GetAll().Where(e => e.IsActive).ToList();
                return View(task);
            }

            _taskService.Update(task);
            TempData["SuccessMessage"] = "Görev başarıyla güncellenmiştir.";
            return RedirectToAction("Index");
        }

        // Görev durumunu hızlıca güncelleyen metot (Çalışan kendi görevini, Yönetici tüm görevleri güncelleyebilir)
        [HttpPost]
        public IActionResult UpdateStatus(int id, TaskState status)
        {
            var task = _taskService.GetById(id);
            if (task == null)
            {
                return NotFound();
            }

            // Güvenlik Kontrolü: Çalışan sadece kendisine atanmış görevleri güncelleyebilir
            if (!IsYönetici && task.EmployeeId != CurrentUserId)
            {
                // İstek AJAX ile yapıldıysa JSON formatında hata dönüyoruz
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Bu görevin durumunu güncelleme yetkiniz bulunmamaktadır." });
                }

                TempData["ErrorMessage"] = "Bu görevin durumunu güncelleme yetkiniz bulunmamaktadır.";
                return RedirectToAction("Index");
            }

            _taskService.UpdateStatus(id, status);

            // İstek AJAX ile yapıldıysa JSON formatında başarı dönüyoruz
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }

            TempData["SuccessMessage"] = "Görev durumu başarıyla güncellenmiştir.";
            return RedirectToAction("Index");
        }

        // Görevi silen metot (Sadece Yönetici)
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!IsYönetici)
            {
                return RedirectToAction("Index");
            }

            _taskService.Delete(id);
            TempData["SuccessMessage"] = "Görev başarıyla silinmiştir.";
            return RedirectToAction("Index");
        }
    }
}
