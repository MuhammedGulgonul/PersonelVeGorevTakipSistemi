using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PersonelVeGorevTakipSistemi.Core.Entities;
using PersonelVeGorevTakipSistemi.DataAccess;
using PersonelVeGorevTakipSistemi.Business.Helpers;

namespace PersonelVeGorevTakipSistemi.Business.Services
{
    // Personel islemlerinin is kurallarini ve veritabanı sorgularini yoneten servis sinifi
    public class EmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        // Tüm personelleri bagli olduklari departman bilgileriyle birlikte listeler
        public List<Employee> GetAll()
        {
            return _context.Employees.Include(e => e.Department).ToList();
        }

        // Id degerine göre personeli departman bilgisiyle getirir
        public Employee GetById(int id)
        {
            return _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);
        }

        // Yeni personel ekler (E-posta benzersizlik kontrolu ve sifre hashleme yapar)
        public bool Add(Employee employee)
        {
            // E-posta adresi veritabaninda zaten var mi kontrol ediyoruz
            bool emailExists = _context.Employees.Any(e => e.Email == employee.Email);
            if (emailExists)
            {
                return false; // E-posta varsa eklemeyi iptal et, false don
            }

            // Sifreyi veritabanina kaydetmeden once guvenli sekilde hash'liyoruz
            employee.PasswordHash = PasswordHasher.HashPassword(employee.PasswordHash);
            employee.CreatedDate = DateTime.Now;
            employee.IsActive = true;
            employee.IsPasswordResetRequested = false;

            _context.Employees.Add(employee);
            _context.SaveChanges();
            return true;
        }

        // Personel bilgilerini gunceller (Secmeli sifre guncellemesi barindirir)
        public bool Update(Employee employee, string newPassword = null)
        {
            var existingEmployee = _context.Employees.Find(employee.Id);
            if (existingEmployee == null)
            {
                return false;
            }

            // Kendi disindaki personellerde bu e-posta adresi var mi kontrol ediyoruz
            bool emailExists = _context.Employees.Any(e => e.Email == employee.Email && e.Id != employee.Id);
            if (emailExists)
            {
                return false;
            }

            // Temel bilgileri guncelliyoruz
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Role = employee.Role;
            existingEmployee.IsActive = employee.IsActive;
            existingEmployee.DepartmentId = employee.DepartmentId;

            // Eger yeni bir sifre girilmisse hash'leyip guncelliyoruz, bos birakilmissa eski sifresini koruyoruz
            if (!string.IsNullOrEmpty(newPassword))
            {
                existingEmployee.PasswordHash = PasswordHasher.HashPassword(newPassword);
            }

            _context.SaveChanges();
            return true;
        }

        // Personeli silmek yerine aktif/pasif durumunu degistirir
        public void ToggleStatus(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                employee.IsActive = !employee.IsActive;
                _context.SaveChanges();
            }
        }

        // Personelin sifresini varsayilan "123456" olarak sifirlar ve talebi kapatir
        public bool ResetPassword(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                employee.PasswordHash = PasswordHasher.HashPassword("123456");
                employee.IsPasswordResetRequested = false;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
