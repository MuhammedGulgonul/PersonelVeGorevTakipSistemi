using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PersonelVeGorevTakipSistemi.Core.Entities;
using PersonelVeGorevTakipSistemi.DataAccess;

namespace PersonelVeGorevTakipSistemi.Business.Services
{
    // Şirket içi duyuru işlemlerini yöneten servis sınıfı
    public class AnnouncementService
    {
        private readonly AppDbContext _context;

        public AnnouncementService(AppDbContext context)
        {
            _context = context;
        }

        // Aktif tüm duyuruları yeniden eskiye doğru getirir
        public List<Announcement> GetActiveAnnouncements()
        {
            try
            {
                return _context.Announcements
                    .Include(a => a.Employee)
                    .Where(a => a.IsActive)
                    .OrderByDescending(a => a.CreatedDate)
                    .ToList();
            }
            catch (Exception)
            {
                return new List<Announcement>();
            }
        }

        // Yeni duyuru ekler (Sadece Yönetici)
        public void Add(Announcement announcement)
        {
            announcement.CreatedDate = DateTime.Now;
            announcement.IsActive = true;
            _context.Announcements.Add(announcement);
            _context.SaveChanges();
        }

        // Duyuruyu siler (Sadece Yönetici)
        public void Delete(int id)
        {
            var announcement = _context.Announcements.Find(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
                _context.SaveChanges();
            }
        }
    }
}
