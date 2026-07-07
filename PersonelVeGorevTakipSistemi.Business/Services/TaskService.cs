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

        // Belirli bir çalışana atanmış tüm görevleri departman bilgisiyle listeler
        public List<Task> GetByEmployeeId(int employeeId)
        {
            return _context.Tasks
                .Include(t => t.Employee)
                .ThenInclude(e => e.Department)
                .Where(t => t.EmployeeId == employeeId)
                .ToList();
        }

        // Id değerine göre görevi detaylı getirir
        public Task GetById(int id)
        {
            return _context.Tasks
                .Include(t => t.Employee)
                .ThenInclude(e => e.Department)
                .FirstOrDefault(t => t.Id == id);
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
    }
}
