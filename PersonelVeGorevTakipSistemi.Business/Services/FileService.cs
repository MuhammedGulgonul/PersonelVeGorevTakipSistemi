using System;
using System.IO;
using PersonelVeGorevTakipSistemi.Core.Entities;
using PersonelVeGorevTakipSistemi.DataAccess;

namespace PersonelVeGorevTakipSistemi.Business.Services
{
    // Gorevlere yuklenen PDF ve Görsel dosyalarin disk ve veritabani islemlerini yoneten servis
    public class FileService
    {
        private readonly AppDbContext _context;

        public FileService(AppDbContext context)
        {
            _context = context;
        }

        // Id degerine gore dosya kaydini getirir
        public TaskFile GetById(int id)
        {
            return _context.TaskFiles.Find(id);
        }

        // Dosyayi sunucu diskine kaydeder ve veritabanina yazar
        public TaskFile SaveFile(int taskId, string fileName, string contentType, Stream fileStream, string uploadsFolder)
        {
            // Eger yukleme klasoru diskte yoksa olusturuyoruz
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Dosya isimlerinin cakismamasi icin benzersiz bir isim uretiyoruz
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(fileName);
            string physicalPath = Path.Combine(uploadsFolder, uniqueFileName);

            // Dosya akisini sunucu diskine yaziyoruz
            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                fileStream.CopyTo(stream);
            }

            // Veritabani kaydini hazirliyoruz
            var taskFile = new TaskFile
            {
                FileName = fileName,
                FilePath = "/uploads/" + uniqueFileName, // Arayuzden erisilmek uzere goreli yol
                ContentType = contentType,
                UploadedDate = DateTime.Now,
                TaskId = taskId
            };

            _context.TaskFiles.Add(taskFile);
            _context.SaveChanges();

            return taskFile;
        }

        // Dosyayi hem sunucu diskinden hem de veritabanindan siler
        public bool DeleteFile(int id, string uploadsFolder)
        {
            var taskFile = _context.TaskFiles.Find(id);
            if (taskFile != null)
            {
                // Diskteki fiziki dosya yolunu bulup siliyoruz
                string relativePath = taskFile.FilePath.Replace("/uploads/", "");
                string physicalPath = Path.Combine(uploadsFolder, relativePath);
                
                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                }

                // Veritabanindaki kaydi siliyoruz
                _context.TaskFiles.Remove(taskFile);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
