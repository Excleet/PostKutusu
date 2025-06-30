using Otel.BLL.Services.Abstract;
using Otel.DAL.Repositories.Abstract;
using Otel.Entity.DTOs;
using Otel.Entity.Entities;

namespace Otel.BLL.Services.Concrete
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<bool> ValidateAdminAsync(AdminLoginDto loginDto)
        {
            return await _adminRepository.ValidateAdminAsync(loginDto.KullaniciAdi, loginDto.Sifre);
        }

        public async Task<Admin?> GetAdminByKullaniciAdiAsync(string kullaniciAdi)
        {
            return await _adminRepository.GetByKullaniciAdiAsync(kullaniciAdi);
        }

        public async Task UpdateSonGirisTarihiAsync(int adminId)
        {
            await _adminRepository.UpdateSonGirisTarihiAsync(adminId);
        }

        public async Task<bool> ChangePasswordAsync(int adminId, string eskiSifre, string yeniSifre)
        {
            try
            {
                var admin = await _adminRepository.GetByIdAsync(adminId);
                if (admin == null || admin.Sifre != eskiSifre)
                    return false;

                admin.Sifre = yeniSifre;
                await _adminRepository.UpdateAsync(admin);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
