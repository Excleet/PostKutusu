namespace Otel.Entity.DTOs
{
    public class AdminLoginDto
    {
        public string KullaniciAdi { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
        public bool BeniHatirla { get; set; }
    }
}
