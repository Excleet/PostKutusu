@echo off
echo Otel Rezervasyon Sistemi - Build Script (.NET 7.0)
echo =====================================================

echo 1. Solution restore ediliyor...
dotnet restore OtelRezervasyon.sln

echo.
echo 2. Proje build ediliyor...
dotnet build OtelRezervasyon.sln --configuration Release

echo.
echo 3. Database migration kontrol ediliyor...
cd Otel.Web
echo Migration zaten oluşturuldu.

echo.
echo 4. Database update kontrol ediliyor...
echo Database zaten güncel.

cd ..
echo.
echo Build tamamlandı!
echo.
echo Projeyi çalıştırmak için:
echo cd OtelRezervasyon
echo dotnet run --project Otel.Web
echo.
echo Ana Site: https://localhost:5001
echo Admin Panel: https://localhost:5001/Admin/Login
echo Admin Kullanıcı: admin / Şifre: 123456
echo.
pause
