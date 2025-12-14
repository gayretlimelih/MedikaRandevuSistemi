using System;
using System.ComponentModel.DataAnnotations;

namespace _3.RANDEVU_YAPISIM.Models
{
    public class Hasta
    {
        public int Id { get; set; }

        [Required]
        public string AdSoyad { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Sifre { get; set; } = string.Empty;

        public string? TcKimlik { get; set; }
        public string? Adres { get; set; }
        public DateTime? DogumTarihi { get; set; }

        // Rol: Admin / Hasta
        public string Role { get; set; } = "Hasta";

        // Aktif - Pasif (Soft Delete)
        public bool Aktif { get; set; } = true;
    }
}
