using Otel.Entity.DTOs;
using Otel.Entity.Entities;

namespace Otel.BLL.Services.Abstract
{
    public interface IAdminService
    {
        Task<bool> ValidateAdminAsync(AdminLoginDto loginDto);
        Task<Admin?> GetAdminByKullaniciAdiAsync(string kullaniciAdi);
        Task UpdateSonGirisTarihiAsync(int adminId);
        Task<bool> ChangePasswordAsync(int adminId, string eskiSifre, string yeniSifre);
    }
}
