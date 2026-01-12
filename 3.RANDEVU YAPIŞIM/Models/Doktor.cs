namespace _3.RANDEVU_YAPISIM.Models
{
    public class Doktor
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = null!;
        public string Brans { get; set; } = null!;

        // ✅ Silmek yerine arşive alma (Soft Delete)
        public bool Arsivli { get; set; } = false;
    }
}
