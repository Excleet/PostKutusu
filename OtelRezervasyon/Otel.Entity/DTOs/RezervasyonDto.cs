namespace Otel.Entity.DTOs
{
    public class RezervasyonDto
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public DateTime GirisTarihi { get; set; }
        public DateTime CikisTarihi { get; set; }
        public string OdaTipi { get; set; } = string.Empty;
        public int MisafirSayisi { get; set; }
        public string? Mesaj { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public bool OnayDurumu { get; set; }
        public int GeceSayisi { get; set; }
    }
}
