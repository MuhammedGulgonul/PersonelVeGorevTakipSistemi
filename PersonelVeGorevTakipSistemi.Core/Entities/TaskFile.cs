using System;

namespace PersonelVeGorevTakipSistemi.Core.Entities
{
    // Görevlere yüklenen PDF veya Resim dosyalarının bilgilerini tutan sınıf
    public class TaskFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public DateTime UploadedDate { get; set; }

        // Dosyanın ait olduğu görev ilişkisi
        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}
