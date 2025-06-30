using Otel.BLL.Services.Abstract;
using Otel.DAL.Repositories.Abstract;
using Otel.Entity.DTOs;
using Otel.Entity.Entities;

namespace Otel.BLL.Services.Concrete
{
    public class RezervasyonService : IRezervasyonService
    {
        private readonly IRezervasyonRepository _rezervasyonRepository;

        public RezervasyonService(IRezervasyonRepository rezervasyonRepository)
        {
            _rezervasyonRepository = rezervasyonRepository;
        }

        public async Task<IEnumerable<RezervasyonDto>> GetAllRezervasyonlarAsync()
        {
            var rezervasyonlar = await _rezervasyonRepository.GetAllAsync();
            return rezervasyonlar.Select(r => new RezervasyonDto
            {
                Id = r.Id,
                AdSoyad = r.AdSoyad,
                Email = r.Email,
                Telefon = r.Telefon,
                GirisTarihi = r.GirisTarihi,
                CikisTarihi = r.CikisTarihi,
                OdaTipi = r.OdaTipi,
                MisafirSayisi = r.MisafirSayisi,
                Mesaj = r.Mesaj,
                OlusturmaTarihi = r.OlusturmaTarihi,
                OnayDurumu = r.OnayDurumu,
                GeceSayisi = r.GeceSayisi
            }).OrderByDescending(r => r.OlusturmaTarihi);
        }

        public async Task<RezervasyonDto?> GetRezervasyonByIdAsync(int id)
        {
            var rezervasyon = await _rezervasyonRepository.GetByIdAsync(id);
            if (rezervasyon == null) return null;

            return new RezervasyonDto
            {
                Id = rezervasyon.Id,
                AdSoyad = rezervasyon.AdSoyad,
                Email = rezervasyon.Email,
                Telefon = rezervasyon.Telefon,
                GirisTarihi = rezervasyon.GirisTarihi,
                CikisTarihi = rezervasyon.CikisTarihi,
                OdaTipi = rezervasyon.OdaTipi,
                MisafirSayisi = rezervasyon.MisafirSayisi,
                Mesaj = rezervasyon.Mesaj,
                OlusturmaTarihi = rezervasyon.OlusturmaTarihi,
                OnayDurumu = rezervasyon.OnayDurumu,
                GeceSayisi = rezervasyon.GeceSayisi
            };
        }

        public async Task<bool> AddRezervasyonAsync(RezervasyonDto rezervasyonDto)
        {
            try
            {
                // Tarih validasyonu
                var isValidResult = ValidateRezervasyonTarihi(rezervasyonDto.GirisTarihi, rezervasyonDto.CikisTarihi);
                if (!await isValidResult)
                    return false;

                var rezervasyon = new Rezervasyon
                {
                    AdSoyad = rezervasyonDto.AdSoyad,
                    Email = rezervasyonDto.Email,
                    Telefon = rezervasyonDto.Telefon,
                    GirisTarihi = rezervasyonDto.GirisTarihi,
                    CikisTarihi = rezervasyonDto.CikisTarihi,
                    OdaTipi = rezervasyonDto.OdaTipi,
                    MisafirSayisi = rezervasyonDto.MisafirSayisi,
                    Mesaj = rezervasyonDto.Mesaj,
                    OlusturmaTarihi = DateTime.Now,
                    OnayDurumu = false
                };

                await _rezervasyonRepository.AddAsync(rezervasyon);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateRezervasyonAsync(RezervasyonDto rezervasyonDto)
        {
            try
            {
                var rezervasyon = await _rezervasyonRepository.GetByIdAsync(rezervasyonDto.Id);
                if (rezervasyon == null) return false;

                rezervasyon.AdSoyad = rezervasyonDto.AdSoyad;
                rezervasyon.Email = rezervasyonDto.Email;
                rezervasyon.Telefon = rezervasyonDto.Telefon;
                rezervasyon.GirisTarihi = rezervasyonDto.GirisTarihi;
                rezervasyon.CikisTarihi = rezervasyonDto.CikisTarihi;
                rezervasyon.OdaTipi = rezervasyonDto.OdaTipi;
                rezervasyon.MisafirSayisi = rezervasyonDto.MisafirSayisi;
                rezervasyon.Mesaj = rezervasyonDto.Mesaj;
                rezervasyon.OnayDurumu = rezervasyonDto.OnayDurumu;

                await _rezervasyonRepository.UpdateAsync(rezervasyon);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteRezervasyonAsync(int id)
        {
            try
            {
                await _rezervasyonRepository.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> OnaylaRezervasyonAsync(int id)
        {
            try
            {
                var rezervasyon = await _rezervasyonRepository.GetByIdAsync(id);
                if (rezervasyon == null) return false;

                rezervasyon.OnayDurumu = true;
                await _rezervasyonRepository.UpdateAsync(rezervasyon);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<RezervasyonDto>> GetOnaysizRezervasyonlarAsync()
        {
            var rezervasyonlar = await _rezervasyonRepository.GetOnaysizRezervasyonlarAsync();
            return rezervasyonlar.Select(r => new RezervasyonDto
            {
                Id = r.Id,
                AdSoyad = r.AdSoyad,
                Email = r.Email,
                Telefon = r.Telefon,
                GirisTarihi = r.GirisTarihi,
                CikisTarihi = r.CikisTarihi,
                OdaTipi = r.OdaTipi,
                MisafirSayisi = r.MisafirSayisi,
                Mesaj = r.Mesaj,
                OlusturmaTarihi = r.OlusturmaTarihi,
                OnayDurumu = r.OnayDurumu,
                GeceSayisi = r.GeceSayisi
            });
        }

        public async Task<IEnumerable<RezervasyonDto>> GetRezervasyonlarByTarihAsync(DateTime baslangic, DateTime bitis)
        {
            var rezervasyonlar = await _rezervasyonRepository.GetRezervasyonlarByTarihAsync(baslangic, bitis);
            return rezervasyonlar.Select(r => new RezervasyonDto
            {
                Id = r.Id,
                AdSoyad = r.AdSoyad,
                Email = r.Email,
                Telefon = r.Telefon,
                GirisTarihi = r.GirisTarihi,
                CikisTarihi = r.CikisTarihi,
                OdaTipi = r.OdaTipi,
                MisafirSayisi = r.MisafirSayisi,
                Mesaj = r.Mesaj,
                OlusturmaTarihi = r.OlusturmaTarihi,
                OnayDurumu = r.OnayDurumu,
                GeceSayisi = r.GeceSayisi
            });
        }

        public async Task<bool> TarihAraligindaRezervasyonVarMiAsync(DateTime girisTarihi, DateTime cikisTarihi, string odaTipi)
        {
            return await _rezervasyonRepository.TarihAraligindaRezervasyonVarMiAsync(girisTarihi, cikisTarihi, odaTipi);
        }

        public Task<bool> ValidateRezervasyonTarihi(DateTime girisTarihi, DateTime cikisTarihi)
        {
            // Giriş tarihi bugünden önce olamaz
            if (girisTarihi.Date < DateTime.Today)
                return Task.FromResult(false);

            // Çıkış tarihi giriş tarihinden sonra olmalı
            if (cikisTarihi <= girisTarihi)
                return Task.FromResult(false);

            // Maksimum 30 gün konaklama
            if ((cikisTarihi - girisTarihi).TotalDays > 30)
                return Task.FromResult(false);

            return Task.FromResult(true);
        }
    }
}
