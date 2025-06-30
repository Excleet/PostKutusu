using Microsoft.EntityFrameworkCore;
using Otel.DAL.Data;
using Otel.DAL.Repositories.Abstract;
using Otel.Entity.Entities;

namespace Otel.DAL.Repositories.Concrete
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        public AdminRepository(OtelDbContext context) : base(context)
        {
        }

        public async Task<Admin?> GetByKullaniciAdiAsync(string kullaniciAdi)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.KullaniciAdi == kullaniciAdi && a.Aktif);
        }

        public async Task<bool> ValidateAdminAsync(string kullaniciAdi, string sifre)
        {
            // Debug için tüm adminleri listeleyelim
            var allAdmins = await _dbSet.ToListAsync();
            Console.WriteLine($"Toplam admin sayısı: {allAdmins.Count}");
            foreach (var admin in allAdmins)
            {
                Console.WriteLine($"Admin: {admin.KullaniciAdi} - Şifre: {admin.Sifre} - Aktif: {admin.Aktif}");
            }
            
            Console.WriteLine($"Gelen veriler - Kullanıcı: '{kullaniciAdi}' - Şifre: '{sifre}'");
            
            var result = await _dbSet
                .AnyAsync(a => a.KullaniciAdi == kullaniciAdi && 
                              a.Sifre == sifre && 
                              a.Aktif);
                              
            Console.WriteLine($"Validation sonucu: {result}");
            return result;
        }

        public async Task UpdateSonGirisTarihiAsync(int adminId)
        {
            var admin = await GetByIdAsync(adminId);
            if (admin != null)
            {
                admin.SonGirisTarihi = DateTime.Now;
                await UpdateAsync(admin);
            }
        }
    }
}
