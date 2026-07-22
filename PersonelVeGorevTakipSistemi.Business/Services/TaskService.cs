using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PersonelVeGorevTakipSistemi.Core.Enums;
using PersonelVeGorevTakipSistemi.DataAccess;
using Task = PersonelVeGorevTakipSistemi.Core.Entities.Task;

namespace PersonelVeGorevTakipSistemi.Business.Services
{
    // Görev işlemlerinin iş kurallarını ve veritabanı sorgularını yöneten servis sınıfı
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        // Tüm görevleri çalışan ve departman bilgisiyle listeler
        public List<Task> GetAll()
        {
            return _context.Tasks
                .Include(t => t.Employee)
                .ThenInclude(e => e.Department)
                .ToList();
        }

        // Çalışana atanan veya departmanına/şirkete açık tüm görünür görevleri getirir
        public List<Task> GetByEmployeeId(int employeeId)
        {
            var currentEmp = _context.Employees.Find(employeeId);
            int currentDeptId = currentEmp?.DepartmentId ?? 0;

            return _context.Tasks
                .Include(t => t.Employee)
                .ThenInclude(e => e.Department)
                .Where(t => t.EmployeeId == employeeId 
                         || (t.Visibility == Core.Enums.TaskVisibility.Department && t.Employee != null && t.Employee.DepartmentId == currentDeptId)
                         || t.Visibility == Core.Enums.TaskVisibility.Public)
                .ToList();
        }

        // Id değerine göre görevi detaylı getirir
        public Task GetById(int id)
        {
            return _context.Tasks
                .Include(t => t.Employee)
                .ThenInclude(e => e.Department)
                .Include(t => t.TaskFiles)
                .Include(t => t.TaskComments)
                .ThenInclude(c => c.Employee)
                .FirstOrDefault(t => t.Id == id);
        }

        // Göreve yeni yorum ekler
        public void AddComment(Core.Entities.TaskComment comment)
        {
            comment.CreatedDate = DateTime.Now;
            _context.TaskComments.Add(comment);
            _context.SaveChanges();
        }

        // Görevden yorum siler
        public void DeleteComment(int commentId)
        {
            var comment = _context.TaskComments.Find(commentId);
            if (comment != null)
            {
                _context.TaskComments.Remove(comment);
                _context.SaveChanges();
            }
        }

        // Yeni görev ekler
        public void Add(Task task)
        {
            task.CreatedDate = DateTime.Now;
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        // Görev bilgilerini günceller (Yönetici yetkisiyle)
        public void Update(Task task)
        {
            var existingTask = _context.Tasks.Find(task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.Status = task.Status;
                existingTask.Priority = task.Priority;
                existingTask.Visibility = task.Visibility;
                existingTask.DueDate = task.DueDate;
                existingTask.EmployeeId = task.EmployeeId;

                _context.SaveChanges();
            }
        }

        // Sadece görev durumunu günceller (Hem Çalışan hem Yönetici güncelleyebilir)
        public bool UpdateStatus(int taskId, TaskState newStatus)
        {
            var existingTask = _context.Tasks.Find(taskId);
            if (existingTask != null)
            {
                existingTask.Status = newStatus;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        // Görevi siler (Sadece Yönetici yetkisiyle)
        public void Delete(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }

        // Son teslim tarihinin üzerinden 7 gün geçen tamamlanmış görevleri veritabanından otomatik siler
        public int CleanUpOldCompletedTasks()
        {
            var thresholdDate = DateTime.Today.AddDays(-7);
            var oldCompletedTasks = _context.Tasks
                .Where(t => t.Status == TaskState.Completed && t.DueDate <= thresholdDate)
                .ToList();

            if (oldCompletedTasks.Any())
            {
                _context.Tasks.RemoveRange(oldCompletedTasks);
                _context.SaveChanges();
            }
            return oldCompletedTasks.Count;
        }
    }
}
