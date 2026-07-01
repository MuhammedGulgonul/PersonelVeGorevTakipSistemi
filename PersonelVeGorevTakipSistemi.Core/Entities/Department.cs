using System.Collections.Generic;

namespace PersonelVeGorevTakipSistemi.Core.Entities
{
    // Şirket departman bilgilerini tutan sınıf
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Departmana bağlı personellerin listesi
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
