using System;

namespace PersonelVeGorevTakipSistemi.Core.Entities
{
    // Görevlere eklenen kullanıcı yorumlarını ve notlarını tutan sınıf
    public class TaskComment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string CommentText { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
