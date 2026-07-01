using System;

namespace PersonelVeGorevTakipSistemi.Core.Entities
{
    // Sistemde oluşan hataların ve olayların kayıtlarını tutan sınıf
    public class Log
    {
        public int Id { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
