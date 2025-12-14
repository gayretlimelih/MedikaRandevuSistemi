using System;

namespace _3.RANDEVU_YAPISIM.Models
{
    public class AdminRandevuVM
    {
        public int Id { get; set; }
        public string DoktorAdSoyad { get; set; } = "";
        public string Brans { get; set; } = "";
        public string HastaAdSoyad { get; set; } = "";
        public string HastaEmail { get; set; } = "";
        public DateTime Tarih { get; set; }
        public string Saat { get; set; } = "";

        public string Durum { get; set; } = "Aktif";
    }
}
