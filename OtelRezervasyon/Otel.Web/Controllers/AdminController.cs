using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otel.BLL.Services.Abstract;
using Otel.Entity.DTOs;
using System.Security.Claims;

namespace Otel.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IRezervasyonService _rezervasyonService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, IRezervasyonService rezervasyonService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _rezervasyonService = rezervasyonService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginDto model)
        {
            Console.WriteLine($"Login deneme - Kullanıcı: '{model.KullaniciAdi}' - Şifre: '{model.Sifre}'");
            
            // Form verilerini de kontrol edelim
            Console.WriteLine("Form verileri:");
            foreach (var key in Request.Form.Keys)
            {
                Console.WriteLine($"  {key}: '{Request.Form[key]}'");
            }
            
            // ModelState'i kontrol edelim
            Console.WriteLine($"ModelState Valid: {ModelState.IsValid}");
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"ModelState Error - {error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }
            
            // Model binding çalışmıyorsa, manual olarak form değerlerini al
            if (string.IsNullOrEmpty(model.KullaniciAdi) && Request.Form.ContainsKey("KullaniciAdi"))
            {
                model.KullaniciAdi = Request.Form["KullaniciAdi"].ToString();
                Console.WriteLine($"Manual binding - KullaniciAdi: '{model.KullaniciAdi}'");
            }
            
            if (string.IsNullOrEmpty(model.Sifre) && Request.Form.ContainsKey("Sifre"))
            {
                model.Sifre = Request.Form["Sifre"].ToString();
                Console.WriteLine($"Manual binding - Sifre: '{model.Sifre}'");
            }
            
            if (Request.Form.ContainsKey("BeniHatirla"))
            {
                model.BeniHatirla = Request.Form["BeniHatirla"].ToString().Contains("true");
                Console.WriteLine($"Manual binding - BeniHatirla: {model.BeniHatirla}");
            }
            
            // Validation için minimum check
            if (!string.IsNullOrEmpty(model.KullaniciAdi) && !string.IsNullOrEmpty(model.Sifre))
            {
                Console.WriteLine("Manuel validation geçerli");
                var isValid = await _adminService.ValidateAdminAsync(model);
                Console.WriteLine($"Validation sonucu: {isValid}");
                
                if (isValid)
                {
                    Console.WriteLine("Validation başarılı, admin bilgileri alınıyor...");
                    var admin = await _adminService.GetAdminByKullaniciAdiAsync(model.KullaniciAdi);
                    if (admin != null)
                    {
                        Console.WriteLine($"Admin bulundu: {admin.KullaniciAdi}");
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, admin.KullaniciAdi),
                            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                            new Claim("FullName", admin.AdSoyad ?? admin.KullaniciAdi)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.BeniHatirla,
                            ExpiresUtc = model.BeniHatirla ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(24)
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                            new ClaimsPrincipal(claimsIdentity), authProperties);

                        await _adminService.UpdateSonGirisTarihiAsync(admin.Id);

                        Console.WriteLine("Giriş başarılı, Dashboard'a yönlendiriliyor");
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        Console.WriteLine("Admin bilgileri alınamadı");
                    }
                }
                else
                {
                    Console.WriteLine("Validation başarısız");
                }
                
                TempData["Error"] = "Kullanıcı adı veya şifre hatalı.";
            }
            else
            {
                Console.WriteLine("Kullanıcı adı veya şifre boş");
                TempData["Error"] = "Kullanıcı adı ve şifre gereklidir.";
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var rezervasyonlar = await _rezervasyonService.GetAllRezervasyonlarAsync();
            var onaysizRezervasyonlar = await _rezervasyonService.GetOnaysizRezervasyonlarAsync();
            
            ViewBag.ToplamRezervasyonSayisi = rezervasyonlar.Count();
            ViewBag.OnaysizRezervasyonSayisi = onaysizRezervasyonlar.Count();
            ViewBag.BugunGirisler = rezervasyonlar.Count(r => r.GirisTarihi.Date == DateTime.Today);
            ViewBag.BugunCikislar = rezervasyonlar.Count(r => r.CikisTarihi.Date == DateTime.Today);
            
            return View(rezervasyonlar.Take(10));
        }

        [Authorize]
        public async Task<IActionResult> Rezervasyonlar(int? page, string? durum, DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            page ??= 1;
            int pageSize = 20;

            var rezervasyonlar = await _rezervasyonService.GetAllRezervasyonlarAsync();

            // Filtreleme
            if (!string.IsNullOrEmpty(durum))
            {
                if (durum == "onaysiz")
                    rezervasyonlar = rezervasyonlar.Where(r => !r.OnayDurumu);
                else if (durum == "onayli")
                    rezervasyonlar = rezervasyonlar.Where(r => r.OnayDurumu);
            }

            if (baslangicTarihi.HasValue)
                rezervasyonlar = rezervasyonlar.Where(r => r.GirisTarihi >= baslangicTarihi.Value);

            if (bitisTarihi.HasValue)
                rezervasyonlar = rezervasyonlar.Where(r => r.CikisTarihi <= bitisTarihi.Value);

            var pagedRezervasyonlar = rezervasyonlar
                .Skip((page.Value - 1) * pageSize)
                .Take(pageSize);

            ViewBag.CurrentPage = page.Value;
            ViewBag.TotalPages = (int)Math.Ceiling((double)rezervasyonlar.Count() / pageSize);
            ViewBag.Durum = durum;
            ViewBag.BaslangicTarihi = baslangicTarihi;
            ViewBag.BitisTarihi = bitisTarihi;

            return View(pagedRezervasyonlar);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> OnaylaRezervasyonuFromList(int id)
        {
            var result = await _rezervasyonService.OnaylaRezervasyonAsync(id);
            if (result)
            {
                TempData["Success"] = "Rezervasyon onaylandı.";
            }
            else
            {
                TempData["Error"] = "Rezervasyon onaylanırken bir hata oluştu.";
            }
            return RedirectToAction("Rezervasyonlar");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SilRezervasyonuFromList(int id)
        {
            var result = await _rezervasyonService.DeleteRezervasyonAsync(id);
            if (result)
            {
                TempData["Success"] = "Rezervasyon silindi.";
            }
            else
            {
                TempData["Error"] = "Rezervasyon silinirken bir hata oluştu.";
            }
            return RedirectToAction("Rezervasyonlar");
        }

        [Authorize]
        public async Task<IActionResult> RezervasyonDetay(int id)
        {
            var rezervasyon = await _rezervasyonService.GetRezervasyonByIdAsync(id);
            if (rezervasyon == null)
            {
                return NotFound();
            }
            return View(rezervasyon);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
