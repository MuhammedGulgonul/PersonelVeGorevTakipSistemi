using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonelVeGorevTakipSistemi.Business.Services;
using PersonelVeGorevTakipSistemi.Core.Enums;
using Task = PersonelVeGorevTakipSistemi.Core.Entities.Task;

using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace PersonelVeGorevTakipSistemi.WebUI.Controllers
{
    // Giriş yapmış tüm personellerin erişebileceği görev yönetim kontrolcüsü
    [Authorize]
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;
        private readonly EmployeeService _employeeService;
        private readonly DepartmentService _departmentService;
        private readonly FileService _fileService;
        private readonly IWebHostEnvironment _environment;

        public TaskController(TaskService taskService, EmployeeService employeeService, DepartmentService departmentService, FileService fileService, IWebHostEnvironment environment)
        {
            _taskService = taskService;
            _employeeService = employeeService;
            _departmentService = departmentService;
            _fileService = fileService;
            _environment = environment;
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

            // İstek AJAX ile yapıldıysa kartın güncel HTML parçacığını (butonları yenilenmiş olarak) dönüyoruz
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var updatedTask = _taskService.GetById(id);
                return PartialView("_TaskCardPartial", updatedTask);
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

        // Görev detay ekranını gösteren metot
        [HttpGet]
        public IActionResult Details(int id)
        {
            var task = _taskService.GetById(id);
            if (task == null)
            {
                return NotFound();
            }

            // Güvenlik Kontrolü: Çalışan sadece kendisine atanmış görevi görebilir
            if (!IsYönetici && task.EmployeeId != CurrentUserId)
            {
                TempData["ErrorMessage"] = "Bu görevin detaylarını görme yetkiniz bulunmamaktadır.";
                return RedirectToAction("Index");
            }

            return View(task);
        }

        // Göreve dosya yükleyen metot (Maks 5MB ve PDF/Görsel kontrolü ile)
        [HttpPost]
        public IActionResult UploadFile(int taskId, IFormFile file)
        {
            var task = _taskService.GetById(taskId);
            if (task == null)
            {
                return NotFound();
            }

            // Güvenlik Kontrolü: Çalışan sadece kendi görevine dosya yükleyebilir
            if (!IsYönetici && task.EmployeeId != CurrentUserId)
            {
                TempData["ErrorMessage"] = "Bu göreve dosya yükleme yetkiniz bulunmamaktadır.";
                return RedirectToAction("Index");
            }

            // Dosya seçilmiş mi kontrolü
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Lütfen yüklemek için bir dosya seçin.";
                return RedirectToAction("Details", new { id = taskId });
            }

            // 5MB Boyut Sınırı Kontrolü
            if (file.Length > 5 * 1024 * 1024)
            {
                TempData["ErrorMessage"] = "Yüklenecek dosya boyutu en fazla 5MB olmalıdır.";
                return RedirectToAction("Details", new { id = taskId });
            }

            // Dosya Uzantısı Kontrolü (PDF ve Resimler)
            string ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".pdf" && ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif")
            {
                TempData["ErrorMessage"] = "Yalnızca PDF ve Görsel (JPG, JPEG, PNG, GIF) formatlarında dosya yükleyebilirsiniz.";
                return RedirectToAction("Details", new { id = taskId });
            }

            // Dosyayı sunucuya ve veritabanına kaydediyoruz
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            _fileService.SaveFile(taskId, file.FileName, file.ContentType, file.OpenReadStream(), uploadsFolder);

            TempData["SuccessMessage"] = "Dosya başarıyla yüklenmiştir.";
            return RedirectToAction("Details", new { id = taskId });
        }

        // Dosya indirme işlemini yapan metot
        [HttpGet]
        public IActionResult DownloadFile(int id)
        {
            var taskFile = _fileService.GetById(id);
            if (taskFile == null)
            {
                return NotFound();
            }

            var task = _taskService.GetById(taskFile.TaskId);
            
            // Güvenlik Kontrolü: Çalışan sadece yetkili olduğu görevin dosyasını indirebilir
            if (!IsYönetici && task.EmployeeId != CurrentUserId)
            {
                return Forbid();
            }

            string relativePath = taskFile.FilePath.Replace("/uploads/", "");
            string physicalPath = Path.Combine(_environment.WebRootPath, "uploads", relativePath);

            if (!System.IO.File.Exists(physicalPath))
            {
                return NotFound();
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(physicalPath);
            return File(fileBytes, taskFile.ContentType, taskFile.FileName);
        }

        // Dosya silme işlemini yapan metot
        [HttpPost]
        public IActionResult DeleteFile(int id)
        {
            var taskFile = _fileService.GetById(id);
            if (taskFile == null)
            {
                return NotFound();
            }

            var task = _taskService.GetById(taskFile.TaskId);

            // Güvenlik Kontrolü: Çalışan sadece kendi görevine ait dosyayı silebilir
            if (!IsYönetici && task.EmployeeId != CurrentUserId)
            {
                TempData["ErrorMessage"] = "Bu dosyayı silme yetkiniz bulunmamaktadır.";
                return RedirectToAction("Index");
            }

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            _fileService.DeleteFile(id, uploadsFolder);

            TempData["SuccessMessage"] = "Dosya başarıyla silinmiştir.";
            return RedirectToAction("Details", new { id = taskFile.TaskId });
        }
    }
}
