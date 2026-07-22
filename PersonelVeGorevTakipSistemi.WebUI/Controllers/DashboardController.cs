using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonelVeGorevTakipSistemi.Business.Services;
using PersonelVeGorevTakipSistemi.Core.Enums;

namespace PersonelVeGorevTakipSistemi.WebUI.Controllers
{
    // Sadece Yonetici rolundeki kullanicilarin erisebilecegi istatistik paneli
    [Authorize(Roles = "Yönetici")]
    public class DashboardController : Controller
    {
        private readonly TaskService _taskService;
        private readonly EmployeeService _employeeService;
        private readonly DepartmentService _departmentService;
        private readonly AnnouncementService _announcementService;

        public DashboardController(TaskService taskService, EmployeeService employeeService, DepartmentService departmentService, AnnouncementService announcementService)
        {
            _taskService = taskService;
            _employeeService = employeeService;
            _departmentService = departmentService;
            _announcementService = announcementService;
        }

        // Dashboard ana sayfasini acan ve istatistikleri hesaplayan metot
        [HttpGet]
        public IActionResult Index()
        {
            var tasks = _taskService.GetAll();
            var employees = _employeeService.GetAll();
            var departments = _departmentService.GetAll();

            // Genel Istatistikler
            ViewBag.TotalDepartments = departments.Count;
            ViewBag.TotalEmployees = employees.Count;
            ViewBag.TotalTasks = tasks.Count;

            // Durumlara Gore Gorev Sayilari
            ViewBag.PendingTasks = tasks.Count(t => t.Status == TaskState.Pending);
            ViewBag.InProgressTasks = tasks.Count(t => t.Status == TaskState.InProgress);
            ViewBag.CompletedTasks = tasks.Count(t => t.Status == TaskState.Completed);

            // Oncelik Derecesine Gore Gorev Sayilari
            ViewBag.LowPriority = tasks.Count(t => t.Priority == TaskPriority.Low);
            ViewBag.MediumPriority = tasks.Count(t => t.Priority == TaskPriority.Medium);
            ViewBag.HighPriority = tasks.Count(t => t.Priority == TaskPriority.High);

            // Duyurular ve Geciken Gorevler
            ViewBag.Announcements = _announcementService.GetActiveAnnouncements();
            ViewBag.OverdueTasksCount = tasks.Count(t => t.DueDate < System.DateTime.Today && t.Status != TaskState.Completed);

            return View();
        }
    }
}
