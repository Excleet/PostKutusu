using Otel.Entity.DTOs;
using Otel.Entity.Entities;

namespace Otel.BLL.Services.Abstract
{
    public interface IRezervasyonService
    {
        Task<IEnumerable<RezervasyonDto>> GetAllRezervasyonlarAsync();
        Task<RezervasyonDto?> GetRezervasyonByIdAsync(int id);
        Task<bool> AddRezervasyonAsync(RezervasyonDto rezervasyonDto);
        Task<bool> UpdateRezervasyonAsync(RezervasyonDto rezervasyonDto);
        Task<bool> DeleteRezervasyonAsync(int id);
        Task<bool> OnaylaRezervasyonAsync(int id);
        Task<IEnumerable<RezervasyonDto>> GetOnaysizRezervasyonlarAsync();
        Task<IEnumerable<RezervasyonDto>> GetRezervasyonlarByTarihAsync(DateTime baslangic, DateTime bitis);
        Task<bool> TarihAraligindaRezervasyonVarMiAsync(DateTime girisTarihi, DateTime cikisTarihi, string odaTipi);
        Task<bool> ValidateRezervasyonTarihi(DateTime girisTarihi, DateTime cikisTarihi);
    }
}
