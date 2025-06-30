using Otel.Entity.Entities;

namespace Otel.DAL.Repositories.Abstract
{
    public interface IRezervasyonRepository : IGenericRepository<Rezervasyon>
    {
        Task<IEnumerable<Rezervasyon>> GetRezervasyonlarByTarihAsync(DateTime baslangic, DateTime bitis);
        Task<IEnumerable<Rezervasyon>> GetOnaysizRezervasyonlarAsync();
        Task<IEnumerable<Rezervasyon>> GetOdaTipineGoreRezervasyonlarAsync(string odaTipi);
        Task<bool> TarihAraligindaRezervasyonVarMiAsync(DateTime girisTarihi, DateTime cikisTarihi, string odaTipi);
    }
}
