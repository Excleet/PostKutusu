using Microsoft.AspNetCore.Mvc;
using Otel.BLL.Services.Abstract;
using Otel.Entity.DTOs;

namespace Otel.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRezervasyonService _rezervasyonService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IRezervasyonService rezervasyonService, ILogger<HomeController> logger)
        {
            _rezervasyonService = rezervasyonService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Rooms()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Rezervasyon()
        {
            var model = new RezervasyonDto
            {
                GirisTarihi = DateTime.Today.AddDays(1),
                CikisTarihi = DateTime.Today.AddDays(2),
                MisafirSayisi = 2
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Rezervasyon(RezervasyonDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _rezervasyonService.AddRezervasyonAsync(model);
                if (result)
                {
                    TempData["Success"] = "Rezervasyonunuz başarıyla alınmıştır. En kısa sürede size dönüş yapacağız.";
                    return RedirectToAction("RezervasyonTamamlandi");
                }
                else
                {
                    TempData["Error"] = "Rezervasyon oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.";
                }
            }
            return View(model);
        }

        public IActionResult RezervasyonTamamlandi()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CheckAvailability(DateTime girisTarihi, DateTime cikisTarihi, string odaTipi)
        {
            try
            {
                var isValid = await _rezervasyonService.ValidateRezervasyonTarihi(girisTarihi, cikisTarihi);
                if (!isValid)
                {
                    return Json(new { success = false, message = "Geçersiz tarih aralığı." });
                }

                var isAvailable = !await _rezervasyonService.TarihAraligindaRezervasyonVarMiAsync(girisTarihi, cikisTarihi, odaTipi);
                
                return Json(new { 
                    success = true, 
                    available = isAvailable,
                    message = isAvailable ? "Oda müsait" : "Bu tarihler arasında oda dolu"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Oda müsaitlik kontrolünde hata");
                return Json(new { success = false, message = "Bir hata oluştu." });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
