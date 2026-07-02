using System;
using System.Collections.Generic;

namespace PersonelVeGorevTakipSistemi.Core.Entities
{
    // Şirket çalışanlarının bilgilerini tutan sınıf
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPasswordResetRequested { get; set; }

        // Departman ilişkisi
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        // Personele atanan görevlerin listesi
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
