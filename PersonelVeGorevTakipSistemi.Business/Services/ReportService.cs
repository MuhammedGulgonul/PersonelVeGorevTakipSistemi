using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PersonelVeGorevTakipSistemi.Core.Entities;
using PersonelVeGorevTakipSistemi.Core.Enums;
using Task = PersonelVeGorevTakipSistemi.Core.Entities.Task;

namespace PersonelVeGorevTakipSistemi.Business.Services
{
    // Excel ve PDF raporlarini ureten servis sinifi
    public class ReportService
    {
        // Personel listesini Excel dosyasi (byte dizisi) olarak uretir
        public byte[] ExportEmployeesToExcel(List<Employee> employees)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Personel Listesi");

                // Ust Basliklar
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Adı";
                worksheet.Cell(1, 3).Value = "Soyadı";
                worksheet.Cell(1, 4).Value = "E-posta";
                worksheet.Cell(1, 5).Value = "Ünvan";
                worksheet.Cell(1, 6).Value = "Departman";
                worksheet.Cell(1, 7).Value = "Sistem Rolü";
                worksheet.Cell(1, 8).Value = "Durum";
                worksheet.Cell(1, 9).Value = "Kayıt Tarihi";

                // Baslik Stilleri (Koyu Tas Rengi ve Beyaz Yazi)
                var headerRange = worksheet.Range(1, 1, 1, 9);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2e2c29");
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Verileri Dolduruyoruz
                int row = 2;
                foreach (var emp in employees)
                {
                    worksheet.Cell(row, 1).Value = "#" + emp.Id;
                    worksheet.Cell(row, 2).Value = emp.FirstName;
                    worksheet.Cell(row, 3).Value = emp.LastName;
                    worksheet.Cell(row, 4).Value = emp.Email;
                    worksheet.Cell(row, 5).Value = emp.Title ?? "-";
                    worksheet.Cell(row, 6).Value = emp.Department != null ? emp.Department.Name : "Belirtilmemiş";
                    worksheet.Cell(row, 7).Value = emp.Role;
                    worksheet.Cell(row, 8).Value = emp.IsActive ? "Aktif" : "Pasif";
                    worksheet.Cell(row, 9).Value = emp.CreatedDate.ToString("dd.MM.yyyy");

                    // Hizalamalar
                    worksheet.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    row++;
                }

                // Sutun genisliklerini icerige gore otomatik ayarla
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        // Gorev listesini PDF dosyasi (byte dizisi) olarak uretir
        public byte[] ExportTasksToPdf(List<Task> tasks)
        {
            using (var stream = new MemoryStream())
            {
                // Yatay A4 boyutunda, 20px kenar bosluklu PDF belgesi
                var document = new Document(PageSize.A4.Rotate(), 20f, 20f, 20f, 20f);
                var writer = PdfWriter.GetInstance(document, stream);
                
                document.Open();

                // Turkce karakter destegi icin sistemden Arial fontunu yukleyip gomuyoruz
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                
                Font titleFont = new Font(bf, 16, Font.BOLD, new BaseColor(46, 44, 41)); // Tas Rengi
                Font headerFont = new Font(bf, 10, Font.BOLD, BaseColor.White);
                Font cellFont = new Font(bf, 9, Font.NORMAL, new BaseColor(69, 62, 52));
                Font overdueFont = new Font(bf, 9, Font.BOLD, BaseColor.Red); // Gecikenler icin kirmizi font

                // Rapor Basligi
                Paragraph title = new Paragraph("ŞİRKET DETAYLI GÖREV RAPORU", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 15f;
                document.Add(title);

                // 6 kolonlu tablo
                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2.5f, 3.5f, 1.5f, 1.5f, 2f, 1.5f }); // Sutun genislik oranlari

                // Kolon Ust Basliklari
                string[] headers = { "Görev Başlığı", "Açıklama", "Durum", "Öncelik", "Sorumlu Personel", "Son Teslim" };
                foreach (var h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, headerFont));
                    cell.BackgroundColor = new BaseColor(46, 44, 41); // Tas Rengi
                    cell.Padding = 6f;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                // Gorev Verilerini ekliyoruz
                foreach (var task in tasks)
                {
                    table.AddCell(new PdfPCell(new Phrase(task.Title, cellFont)) { Padding = 5f });
                    
                    string desc = task.Description ?? "";
                    if (desc.Length > 90) desc = desc.Substring(0, 90) + "...";
                    table.AddCell(new PdfPCell(new Phrase(desc, cellFont)) { Padding = 5f });

                    string statusText = task.Status == TaskState.Pending ? "Yapılacak" : (task.Status == TaskState.InProgress ? "Yapılıyor" : "Tamamlandı");
                    table.AddCell(new PdfPCell(new Phrase(statusText, cellFont)) { Padding = 5f, HorizontalAlignment = Element.ALIGN_CENTER });

                    string priorityText = task.Priority == TaskPriority.Low ? "Düşük" : (task.Priority == TaskPriority.Medium ? "Orta" : "Yüksek");
                    table.AddCell(new PdfPCell(new Phrase(priorityText, cellFont)) { Padding = 5f, HorizontalAlignment = Element.ALIGN_CENTER });

                    string empName = task.Employee != null ? (task.Employee.FirstName + " " + task.Employee.LastName) : "Atanmamış";
                    table.AddCell(new PdfPCell(new Phrase(empName, cellFont)) { Padding = 5f });

                    // Bitis tarihi gecmis ve tamamlanmamis ise kirmizi kalin yaz
                    bool isOverdue = task.DueDate < DateTime.Today && task.Status != TaskState.Completed;
                    var dueDatePhrase = new Phrase(task.DueDate.ToString("dd.MM.yyyy"), isOverdue ? overdueFont : cellFont);
                    var dueDateCell = new PdfPCell(dueDatePhrase) 
                    { 
                        Padding = 5f, 
                        HorizontalAlignment = Element.ALIGN_CENTER 
                    };
                    table.AddCell(dueDateCell);
                }

                document.Add(table);
                document.Close();
                return stream.ToArray();
            }
        }
    }
}
