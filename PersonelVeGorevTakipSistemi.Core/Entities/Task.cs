using System;
using System.Collections.Generic;
using PersonelVeGorevTakipSistemi.Core.Enums;

namespace PersonelVeGorevTakipSistemi.Core.Entities
{
    // Personellere atanan görevlerin bilgilerini tutan sınıf
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskState Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }

        // Görevin atandığı personel ilişkisi
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        // Göreve yüklenen dosyaların listesi
        public ICollection<TaskFile> TaskFiles { get; set; } = new List<TaskFile>();
    }
}
