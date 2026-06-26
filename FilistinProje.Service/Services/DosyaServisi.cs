using FilistinProje.Core.DTOs;
using FilistinProje.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FilistinProje.Service.Services
{
    public class DosyaServisi : IDosyaServisi
    {
        private readonly IWebHostEnvironment _env;
        private static readonly HashSet<string> IzinVerilenResimUzantilari = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };
        private static readonly HashSet<string> IzinVerilenDokumanUzantilari = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp", ".pdf"
        };
        private static readonly HashSet<string> IzinVerilenResimIcerikTipleri = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg", "image/png", "image/webp"
        };
        private static readonly HashSet<string> IzinVerilenDokumanIcerikTipleri = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg", "image/png", "image/webp", "application/pdf"
        };
        private const int MaksDosyaBoyutu = 5 * 1024 * 1024;

        public DosyaServisi(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<FileSaveResultDto> KaydetAsync(IFormFile dosya, string altKlasor, bool pdfDestegi = false)
        {
            try
            {
                if (dosya == null || dosya.Length == 0)
                    return new FileSaveResultDto { Success = false, ErrorMessage = "Dosya seçilmedi." };

                if (dosya.Length > MaksDosyaBoyutu)
                    return new FileSaveResultDto { Success = false, ErrorMessage = "Dosya boyutu en fazla 5MB olabilir." };

                var uzanti = Path.GetExtension(dosya.FileName).ToLowerInvariant();
                var izinVerilenUzantilar = pdfDestegi ? IzinVerilenDokumanUzantilari : IzinVerilenResimUzantilari;
                if (!izinVerilenUzantilar.Contains(uzanti))
                {
                    var mesaj = pdfDestegi
                        ? "Yalnızca PDF, JPEG, PNG ve WebP formatları desteklenir."
                        : "Yalnızca JPEG, PNG ve WebP formatları desteklenir.";
                    return new FileSaveResultDto { Success = false, ErrorMessage = mesaj };
                }

                if (!IcerikTipiGecerliMi(dosya.ContentType, pdfDestegi) || !DosyaImzasiGecerliMi(dosya, uzanti))
                    return new FileSaveResultDto { Success = false, ErrorMessage = "Dosya türü doğrulanamadı." };

                if (!TryBuildUploadFolder(altKlasor, out var yuklemeKlasoru, out var urlKlasoru))
                    return new FileSaveResultDto { Success = false, ErrorMessage = "Geçersiz yükleme klasörü." };

                if (!Directory.Exists(yuklemeKlasoru))
                    Directory.CreateDirectory(yuklemeKlasoru);

                var benzersizAd = Guid.NewGuid().ToString("N") + uzanti;
                var tamYol = Path.Combine(yuklemeKlasoru, benzersizAd);

                await using var stream = new FileStream(tamYol, FileMode.CreateNew);
                await dosya.CopyToAsync(stream);

                var url = "/" + urlKlasoru + "/" + benzersizAd;
                return new FileSaveResultDto { Success = true, Url = url };
            }
            catch
            {
                return new FileSaveResultDto { Success = false, ErrorMessage = "Dosya kaydedilemedi." };
            }
        }

        public bool Sil(string dosyaYolu)
        {
            if (string.IsNullOrWhiteSpace(dosyaYolu))
                return false;

            var goreceliYol = dosyaYolu.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
            var webRoot = Path.GetFullPath(_env.WebRootPath);
            var tamYol = Path.GetFullPath(Path.Combine(webRoot, goreceliYol));

            if (!tamYol.StartsWith(webRoot, StringComparison.OrdinalIgnoreCase))
                return false;

            if (File.Exists(tamYol))
            {
                try
                {
                    File.Delete(tamYol);
                    return true;
                }
                catch { }
            }

            return false;
        }

        private bool TryBuildUploadFolder(string altKlasor, out string yuklemeKlasoru, out string urlKlasoru)
        {
            yuklemeKlasoru = string.Empty;
            urlKlasoru = string.Empty;

            if (string.IsNullOrWhiteSpace(altKlasor))
                return false;

            var temizKlasor = altKlasor.Trim().Trim('/', '\\').Replace('\\', '/');
            if (temizKlasor.Contains("..", StringComparison.Ordinal) || Path.IsPathRooted(temizKlasor))
                return false;

            var webRoot = Path.GetFullPath(_env.WebRootPath);
            var adayKlasor = Path.GetFullPath(Path.Combine(webRoot, temizKlasor.Replace('/', Path.DirectorySeparatorChar)));

            if (!adayKlasor.StartsWith(webRoot, StringComparison.OrdinalIgnoreCase))
                return false;

            yuklemeKlasoru = adayKlasor;
            urlKlasoru = temizKlasor;
            return true;
        }

        private static bool IcerikTipiGecerliMi(string? contentType, bool pdfDestegi)
        {
            if (string.IsNullOrWhiteSpace(contentType))
                return true;

            var izinVerilenTipler = pdfDestegi ? IzinVerilenDokumanIcerikTipleri : IzinVerilenResimIcerikTipleri;
            return izinVerilenTipler.Contains(contentType.Trim());
        }

        private static bool DosyaImzasiGecerliMi(IFormFile dosya, string uzanti)
        {
            Span<byte> buffer = stackalloc byte[12];
            using var stream = dosya.OpenReadStream();
            var okunan = stream.Read(buffer);
            var bytes = buffer[..okunan];

            return uzanti switch
            {
                ".jpg" or ".jpeg" => bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF,
                ".png" => bytes.Length >= 8 &&
                    bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47 &&
                    bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A,
                ".webp" => bytes.Length >= 12 &&
                    bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46 &&
                    bytes[8] == 0x57 && bytes[9] == 0x45 && bytes[10] == 0x42 && bytes[11] == 0x50,
                ".pdf" => bytes.Length >= 4 && bytes[0] == 0x25 && bytes[1] == 0x50 && bytes[2] == 0x44 && bytes[3] == 0x46,
                _ => false
            };
        }
    }
}
