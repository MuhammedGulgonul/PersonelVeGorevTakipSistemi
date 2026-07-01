using System;
using Microsoft.EntityFrameworkCore;
using PersonelVeGorevTakipSistemi.Core.Entities;
using Task = PersonelVeGorevTakipSistemi.Core.Entities.Task;

namespace PersonelVeGorevTakipSistemi.DataAccess
{
    // Veritabanı bağlantısını ve tabloları yöneten ana DbContext sınıfımız
    public class AppDbContext : DbContext
    {
        // Constructor (Yapıcı Metot): WebUI katmanından gönderilecek veritabanı adresini (connection string) alır ve temel sınıfa (DbContext) iletir.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Veritabanındaki tablolarımızın C# tarafındaki karşılıkları (DbSet)
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskFile> TaskFiles { get; set; }
        public DbSet<Log> Logs { get; set; }

        // Veritabanı tablolarının detaylı yapılandırmalarını ve başlangıç verilerini (Seed Data) burada tanımlıyoruz
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Başlangıç Departman Verileri (Seed Data)
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Yönetim", Description = "Şirketin idari ve genel yönetim işleri." },
                new Department { Id = 2, Name = "Yazılım Geliştirme", Description = "Yazılım projeleri geliştirme ve Ar-Ge süreçleri." },
                new Department { Id = 3, Name = "İnsan Kaynakları", Description = "İşe alım, eğitim ve çalışan ilişkileri süreçleri." }
            );

            // 2. Başlangıç Admin Personel Verisi (Seed Data)
            // Şifre olarak "123456" şifresinin SHA-256 hash karşılığı kullanılmıştır.
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    FirstName = "Ahmet",
                    LastName = "Yılmaz",
                    Email = "admin@sirket.com",
                    PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", 
                    Role = "Admin",
                    IsActive = true,
                    CreatedDate = new DateTime(2026, 7, 1),
                    DepartmentId = 1
                }
            );
        }
    }
}
