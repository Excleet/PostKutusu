using System.ComponentModel.DataAnnotations;

namespace Otel.Entity.Entities
{
    public class Rezervasyon
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad gereklidir")]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir")]
        public string AdSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon gereklidir")]
        [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir")]
        public string Telefon { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giriş tarihi gereklidir")]
        public DateTime GirisTarihi { get; set; }

        [Required(ErrorMessage = "Çıkış tarihi gereklidir")]
        public DateTime CikisTarihi { get; set; }

        [Required(ErrorMessage = "Oda tipi gereklidir")]
        [StringLength(100, ErrorMessage = "Oda tipi en fazla 100 karakter olabilir")]
        public string OdaTipi { get; set; } = string.Empty;

        public int MisafirSayisi { get; set; }

        [StringLength(500, ErrorMessage = "Mesaj en fazla 500 karakter olabilir")]
        public string? Mesaj { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        public bool OnayDurumu { get; set; } = false;

        // Hesaplanan özellik - geceler
        public int GeceSayisi => (int)(CikisTarihi - GirisTarihi).TotalDays;
    }
}
