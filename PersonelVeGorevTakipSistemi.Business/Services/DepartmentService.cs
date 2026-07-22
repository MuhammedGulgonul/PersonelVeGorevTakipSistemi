using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PersonelVeGorevTakipSistemi.Core.Entities;
using PersonelVeGorevTakipSistemi.DataAccess;

namespace PersonelVeGorevTakipSistemi.Business.Services
{
    // Departman işlemlerinin iş kurallarını ve veritabanı sorgularını yöneten servis sınıfı
    public class DepartmentService
    {
        private readonly AppDbContext _context;

        public DepartmentService(AppDbContext context)
        {
            _context = context;
        }

        // Tüm departmanları personelleriyle birlikte listeler
        public List<Department> GetAll()
        {
            return _context.Departments
                .Include(d => d.Employees)
                .ToList();
        }

        // Id değerine göre tek bir departman getirir
        public Department GetById(int id)
        {
            return _context.Departments.Find(id);
        }

        // Id değerine göre departmanı personelleriyle birlikte detaylı getirir
        public Department GetWithEmployees(int id)
        {
            return _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefault(d => d.Id == id);
        }

        // Yeni departman ekler
        public void Add(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }

        // Departman bilgilerini günceller
        public void Update(Department department)
        {
            _context.Departments.Update(department);
            _context.SaveChanges();
        }

        // Departmanı siler (Eğer departmana bağlı aktif personel varsa silmeyi engeller)
        public bool Delete(int id)
        {
            // Departmana bağlı aktif (IsActive = true) personel var mı kontrol ediyoruz
            bool hasActiveEmployees = _context.Employees.Any(e => e.DepartmentId == id && e.IsActive);

            if (hasActiveEmployees)
            {
                return false; // Aktif çalışan varsa silme işlemine izin verme, false dön
            }

            var department = _context.Departments.Find(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                _context.SaveChanges();
                return true; // Silme işlemi başarılı
            }

            return false;
        }
    }
}
