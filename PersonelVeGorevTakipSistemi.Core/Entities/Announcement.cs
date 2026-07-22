using System;

namespace PersonelVeGorevTakipSistemi.Core.Entities
{
    // Şirket içi duyuruları tutan sınıf
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
