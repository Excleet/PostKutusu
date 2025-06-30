using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Otel.BLL.Services.Abstract;
using Otel.BLL.Services.Concrete;
using Otel.DAL.Data;
using Otel.DAL.Repositories.Abstract;
using Otel.DAL.Repositories.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database Configuration
builder.Services.AddDbContext<OtelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository Dependencies
builder.Services.AddScoped<IRezervasyonRepository, RezervasyonRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

// Service Dependencies
builder.Services.AddScoped<IRezervasyonService, RezervasyonService>();
builder.Services.AddScoped<IAdminService, AdminService>();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
        options.LogoutPath = "/Admin/Logout";
        options.AccessDeniedPath = "/Admin/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Database Migration
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OtelDbContext>();
    context.Database.EnsureCreated();
    
    // Admin kullanıcısının var olduğundan emin ol
    if (!context.Adminler.Any())
    {
        var admin = new Otel.Entity.Entities.Admin
        {
            KullaniciAdi = "admin",
            Sifre = "123456", // Düz metin şifre
            AdSoyad = "Sistem Yöneticisi",
            Email = "admin@valeriaantique.com",
            Aktif = true,
            OlusturmaTarihi = DateTime.Now
        };
        
        context.Adminler.Add(admin);
        context.SaveChanges();
        
        // Debug için log ekleyelim
        Console.WriteLine($"Admin kullanıcısı oluşturuldu: {admin.KullaniciAdi} - {admin.Sifre} - Aktif: {admin.Aktif}");
    }
    else
    {
        // Mevcut admin bilgilerini kontrol edelim
        var existingAdmin = context.Adminler.First();
        Console.WriteLine($"Mevcut admin: {existingAdmin.KullaniciAdi} - {existingAdmin.Sifre} - Aktif: {existingAdmin.Aktif}");
    }
}

app.Run();
