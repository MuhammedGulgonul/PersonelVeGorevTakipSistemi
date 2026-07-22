namespace PersonelVeGorevTakipSistemi.Core.Enums
{
    // Görevin kimler tarafından görülebileceğini belirleyen gizlilik seviyeleri
    public enum TaskVisibility
    {
        Private = 0,    // Özel (Sadece atanan personel ve Yönetici görebilir)
        Department = 1, // Departmana Açık (Aynı departmandaki herkes görebilir)
        Public = 2      // Herkese Açık (Şirketteki tüm çalışanlar görebilir)
    }
}
