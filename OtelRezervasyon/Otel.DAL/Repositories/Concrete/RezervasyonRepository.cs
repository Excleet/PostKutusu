using Microsoft.EntityFrameworkCore;
using Otel.DAL.Data;
using Otel.DAL.Repositories.Abstract;
using Otel.Entity.Entities;

namespace Otel.DAL.Repositories.Concrete
{
    public class RezervasyonRepository : GenericRepository<Rezervasyon>, IRezervasyonRepository
    {
        public RezervasyonRepository(OtelDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Rezervasyon>> GetRezervasyonlarByTarihAsync(DateTime baslangic, DateTime bitis)
        {
            return await _dbSet
                .Where(r => r.GirisTarihi >= baslangic && r.CikisTarihi <= bitis)
                .OrderByDescending(r => r.OlusturmaTarihi)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rezervasyon>> GetOnaysizRezervasyonlarAsync()
        {
            return await _dbSet
                .Where(r => !r.OnayDurumu)
                .OrderByDescending(r => r.OlusturmaTarihi)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rezervasyon>> GetOdaTipineGoreRezervasyonlarAsync(string odaTipi)
        {
            return await _dbSet
                .Where(r => r.OdaTipi == odaTipi)
                .OrderByDescending(r => r.OlusturmaTarihi)
                .ToListAsync();
        }

        public async Task<bool> TarihAraligindaRezervasyonVarMiAsync(DateTime girisTarihi, DateTime cikisTarihi, string odaTipi)
        {
            return await _dbSet
                .AnyAsync(r => r.OdaTipi == odaTipi && 
                              r.OnayDurumu && 
                              ((r.GirisTarihi >= girisTarihi && r.GirisTarihi < cikisTarihi) ||
                               (r.CikisTarihi > girisTarihi && r.CikisTarihi <= cikisTarihi) ||
                               (r.GirisTarihi <= girisTarihi && r.CikisTarihi >= cikisTarihi)));
        }
    }
}
