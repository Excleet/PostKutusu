using Otel.Entity.Entities;

namespace Otel.DAL.Repositories.Abstract
{
    public interface IAdminRepository : IGenericRepository<Admin>
    {
        Task<Admin?> GetByKullaniciAdiAsync(string kullaniciAdi);
        Task<bool> ValidateAdminAsync(string kullaniciAdi, string sifre);
        Task UpdateSonGirisTarihiAsync(int adminId);
    }
}
