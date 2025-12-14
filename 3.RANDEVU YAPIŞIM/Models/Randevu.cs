using System;
using System.ComponentModel.DataAnnotations;

namespace _3.RANDEVU_YAPISIM.Models
{
    public class Randevu
    {
        public int Id { get; set; }

        [Required]
        public int HastaId { get; set; }

        [Required]
        public int DoktorId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Tarih { get; set; }

        [Required]
        public string Saat { get; set; } = string.Empty;

        public string? Aciklama { get; set; }

        // Navigation özellikleri
        public Hasta? Hasta { get; set; }
        public Doktor? Doktor { get; set; }
        public string Durum { get; set; } = "Aktif"; // Aktif / İptal

    }
}
