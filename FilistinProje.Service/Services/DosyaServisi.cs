using FilistinProje.Core.DTOs;
using FilistinProje.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FilistinProje.Service.Services
{
    public class DosyaServisi : IDosyaServisi
    {
        private const int MaksResimDosyaBoyutu = 8 * 1024 * 1024;
        private const int MaksDokumanDosyaBoyutu = 12 * 1024 * 1024;
        private const string SecureStorageFolder = "secure-storage";
        private const string SensitiveStorageFolder = "hassas";

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

        private static readonly byte[][] AktifIcerikImzalari =
        {
            "<script"u8.ToArray(),
            "<html"u8.ToArray(),
            "<!doctype"u8.ToArray(),
            "<svg"u8.ToArray(),
            "<?php"u8.ToArray(),
            "javascript:"u8.ToArray()
        };

        public DosyaServisi(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<FileSaveResultDto> KaydetAsync(IFormFile dosya, string altKlasor, bool pdfDestegi = false)
        {
            var kategori = altKlasor.Contains("recete", StringComparison.OrdinalIgnoreCase)
                ? HassasBelgeKategorisi.Recete
                : HassasBelgeKategorisi.Kimlik;

            var sonuc = await HassasBelgeKaydetAsync(dosya, kategori);
            if (!sonuc.Success)
            {
                return new FileSaveResultDto { Success = false, ErrorMessage = sonuc.ErrorMessage };
            }

            return new FileSaveResultDto
            {
                Success = true,
                Url = BuildPrivateReference(kategori, sonuc.BelgeAdi)
            };
        }

        public async Task<HassasBelgeKayitDto> HassasBelgeKaydetAsync(IFormFile dosya, HassasBelgeKategorisi kategori)
        {
            try
            {
                var pdfDestegi = kategori == HassasBelgeKategorisi.Recete;
                var maksBoyut = pdfDestegi ? MaksDokumanDosyaBoyutu : MaksResimDosyaBoyutu;

                if (dosya == null || dosya.Length == 0)
                {
                    return Fail("empty_file", "Dosya seçilmedi.");
                }

                if (dosya.Length > maksBoyut)
                {
                    return Fail("file_too_large", pdfDestegi
                        ? "Dosya boyutu en fazla 12MB olabilir."
                        : "Dosya boyutu en fazla 8MB olabilir.");
                }

                var uzanti = Path.GetExtension(dosya.FileName);
                if (string.IsNullOrWhiteSpace(uzanti) || dosya.FileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    return Fail("invalid_name", "Dosya adı geçersiz.");
                }

                uzanti = uzanti.ToLowerInvariant();
                var izinVerilenUzantilar = pdfDestegi ? IzinVerilenDokumanUzantilari : IzinVerilenResimUzantilari;
                if (!izinVerilenUzantilar.Contains(uzanti))
                {
                    return Fail("invalid_extension", pdfDestegi
                        ? "Yalnızca PDF, JPEG, PNG ve WebP formatları desteklenir."
                        : "Yalnızca JPEG, PNG ve WebP formatları desteklenir.");
                }

                if (!IcerikTipiGecerliMi(dosya.ContentType, pdfDestegi))
                {
                    return Fail("invalid_mime", "Dosya türü doğrulanamadı.");
                }

                await using var kaynak = dosya.OpenReadStream();
                if (!await DosyaIcerigiGecerliMiAsync(kaynak, uzanti))
                {
                    return Fail("invalid_signature", "Dosya türü doğrulanamadı.");
                }

                kaynak.Position = 0;
                if (await AktifIcerikVarMiAsync(kaynak))
                {
                    return Fail("active_content", "Güvenlik nedeniyle bu dosya türü kabul edilemedi.");
                }

                var klasor = GetSensitiveCategoryFolder(kategori);
                Directory.CreateDirectory(klasor);

                var belgeAdi = Guid.NewGuid().ToString("N") + uzanti;
                var tamYol = Path.GetFullPath(Path.Combine(klasor, belgeAdi));

                if (!tamYol.StartsWith(Path.GetFullPath(klasor), StringComparison.OrdinalIgnoreCase))
                {
                    return Fail("invalid_path", "Dosya kaydedilemedi.");
                }

                kaynak.Position = 0;
                await using var hedef = new FileStream(tamYol, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                await kaynak.CopyToAsync(hedef);

                return new HassasBelgeKayitDto
                {
                    Success = true,
                    BelgeAdi = belgeAdi,
                    Kategori = GetCategorySegment(kategori),
                    ContentType = GetSafeContentType(uzanti),
                    Boyut = dosya.Length
                };
            }
            catch
            {
                return Fail("save_failed", "Dosya kaydedilemedi.");
            }
        }

        public bool Sil(string dosyaYolu)
        {
            if (string.IsNullOrWhiteSpace(dosyaYolu))
            {
                return false;
            }

            if (TryParsePrivateReference(dosyaYolu, out var kategori, out var belgeAdi))
            {
                return HassasBelgeSil(kategori, belgeAdi);
            }

            var goreceliYol = dosyaYolu.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
            var webRoot = Path.GetFullPath(_env.WebRootPath);
            var tamYol = Path.GetFullPath(Path.Combine(webRoot, goreceliYol));

            if (!tamYol.StartsWith(webRoot, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            try
            {
                if (File.Exists(tamYol))
                {
                    File.Delete(tamYol);
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public bool HassasBelgeSil(HassasBelgeKategorisi kategori, string belgeAdi)
        {
            if (!IsSafeStoredFileName(belgeAdi))
            {
                return false;
            }

            var klasor = GetSensitiveCategoryFolder(kategori);
            var tamYol = Path.GetFullPath(Path.Combine(klasor, belgeAdi));
            if (!tamYol.StartsWith(Path.GetFullPath(klasor), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            try
            {
                if (File.Exists(tamYol))
                {
                    File.Delete(tamYol);
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public bool EskiWebRootYoluGecerliMi(string? path, string expectedFolder)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var normalizedPath = path.Trim().Replace('\\', '/');
            var normalizedFolder = "/" + expectedFolder.Trim('/').Replace('\\', '/') + "/";
            if (!normalizedPath.StartsWith(normalizedFolder, StringComparison.OrdinalIgnoreCase) ||
                normalizedPath.Contains("..", StringComparison.Ordinal) ||
                Uri.TryCreate(normalizedPath, UriKind.Absolute, out _))
            {
                return false;
            }

            return IsSafeStoredFileName(Path.GetFileName(normalizedPath));
        }

        public string GetPrivateStorageRoot()
        {
            return Path.GetFullPath(Path.Combine(_env.ContentRootPath, SecureStorageFolder));
        }

        public string GetSensitiveCategoryFolder(HassasBelgeKategorisi kategori)
        {
            return Path.GetFullPath(Path.Combine(GetPrivateStorageRoot(), SensitiveStorageFolder, GetCategorySegment(kategori)));
        }

        public static string BuildPrivateReference(HassasBelgeKategorisi kategori, string belgeAdi)
        {
            return $"private://{GetCategorySegment(kategori)}/{belgeAdi}";
        }

        public static bool TryParsePrivateReference(string? reference, out HassasBelgeKategorisi kategori, out string belgeAdi)
        {
            kategori = default;
            belgeAdi = string.Empty;

            if (string.IsNullOrWhiteSpace(reference) || !reference.StartsWith("private://", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var parts = reference["private://".Length..].Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length != 2 || !IsSafeStoredFileName(parts[1]))
            {
                return false;
            }

            kategori = parts[0].ToLowerInvariant() switch
            {
                "kimlikler" => HassasBelgeKategorisi.Kimlik,
                "receteler" => HassasBelgeKategorisi.Recete,
                _ => default
            };

            if (kategori == default)
            {
                return false;
            }

            belgeAdi = parts[1];
            return true;
        }

        public static string GetCategorySegment(HassasBelgeKategorisi kategori)
        {
            return kategori == HassasBelgeKategorisi.Recete ? "receteler" : "kimlikler";
        }

        public static bool IsSafeStoredFileName(string? belgeAdi)
        {
            if (string.IsNullOrWhiteSpace(belgeAdi))
            {
                return false;
            }

            if (belgeAdi.Contains('/') || belgeAdi.Contains('\\') || belgeAdi.Contains("..", StringComparison.Ordinal))
            {
                return false;
            }

            var uzanti = Path.GetExtension(belgeAdi);
            return belgeAdi.Length <= 80 &&
                belgeAdi.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
                (IzinVerilenDokumanUzantilari.Contains(uzanti));
        }

        public static string GetSafeContentType(string belgeAdiOrExtension)
        {
            var extension = belgeAdiOrExtension.StartsWith('.')
                ? belgeAdiOrExtension
                : Path.GetExtension(belgeAdiOrExtension);

            return extension.ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream"
            };
        }

        private static HassasBelgeKayitDto Fail(string code, string message)
        {
            return new HassasBelgeKayitDto
            {
                Success = false,
                HataKodu = code,
                ErrorMessage = message
            };
        }

        private static bool IcerikTipiGecerliMi(string? contentType, bool pdfDestegi)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return false;
            }

            var tip = contentType.Split(';', 2)[0].Trim();
            var izinVerilenTipler = pdfDestegi ? IzinVerilenDokumanIcerikTipleri : IzinVerilenResimIcerikTipleri;
            return izinVerilenTipler.Contains(tip);
        }

        private static async Task<bool> DosyaIcerigiGecerliMiAsync(Stream stream, string uzanti)
        {
            var buffer = new byte[16];
            stream.Position = 0;
            var okunan = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length));

            return uzanti switch
            {
                ".jpg" or ".jpeg" => okunan >= 3 && buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF,
                ".png" => okunan >= 8 &&
                    buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47 &&
                    buffer[4] == 0x0D && buffer[5] == 0x0A && buffer[6] == 0x1A && buffer[7] == 0x0A,
                ".webp" => okunan >= 12 &&
                    buffer[0] == 0x52 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x46 &&
                    buffer[8] == 0x57 && buffer[9] == 0x45 && buffer[10] == 0x42 && buffer[11] == 0x50,
                ".pdf" => okunan >= 5 && buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46 && buffer[4] == 0x2D,
                _ => false
            };
        }

        private static async Task<bool> AktifIcerikVarMiAsync(Stream stream)
        {
            stream.Position = 0;
            var readLength = (int)Math.Min(stream.Length, 4096);
            var buffer = new byte[readLength];
            _ = await stream.ReadAsync(buffer.AsMemory(0, readLength));
            var lowered = System.Text.Encoding.UTF8.GetString(buffer).ToLowerInvariant();
            var loweredBytes = System.Text.Encoding.UTF8.GetBytes(lowered);

            return AktifIcerikImzalari.Any(sig => ContainsBytes(loweredBytes, sig));
        }

        private static bool ContainsBytes(byte[] haystack, byte[] needle)
        {
            if (needle.Length == 0 || haystack.Length < needle.Length)
            {
                return false;
            }

            for (var i = 0; i <= haystack.Length - needle.Length; i++)
            {
                var found = true;
                for (var j = 0; j < needle.Length; j++)
                {
                    if (haystack[i + j] != needle[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
