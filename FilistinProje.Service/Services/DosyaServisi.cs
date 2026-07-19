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
            var kategori = altKlasor.Contains("invoice", StringComparison.OrdinalIgnoreCase) ||
                altKlasor.Contains("fatura", StringComparison.OrdinalIgnoreCase)
                    ? HassasBelgeKategorisi.Fatura
                    : altKlasor.Contains("recete", StringComparison.OrdinalIgnoreCase)
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
                var sadecePdf = kategori == HassasBelgeKategorisi.Fatura;
                var pdfDestegi = kategori is HassasBelgeKategorisi.Recete or HassasBelgeKategorisi.Fatura;
                var maksBoyut = pdfDestegi ? MaksDokumanDosyaBoyutu : MaksResimDosyaBoyutu;

                if (dosya == null || dosya.Length == 0)
                {
                    return Fail("empty_file", "No file selected.");
                }

                if (dosya.Length > maksBoyut)
                {
                    return Fail("file_too_large", pdfDestegi
                        ? "File size cannot exceed 12MB."
                        : "File size cannot exceed 8MB.");
                }

                var uzanti = Path.GetExtension(dosya.FileName);
                if (string.IsNullOrWhiteSpace(uzanti) || dosya.FileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    return Fail("invalid_name", "Invalid file name.");
                }

                uzanti = uzanti.ToLowerInvariant();
                var izinVerilenUzantilar = sadecePdf
                    ? new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".pdf" }
                    : pdfDestegi ? IzinVerilenDokumanUzantilari : IzinVerilenResimUzantilari;
                if (!izinVerilenUzantilar.Contains(uzanti))
                {
                    return Fail("invalid_extension", pdfDestegi
                        ? "Only PDF, JPEG, PNG, and WebP formats are supported."
                        : "Only JPEG, PNG, and WebP formats are supported.");
                }

                if (sadecePdf && !string.Equals(dosya.ContentType?.Split(';', 2)[0].Trim(), "application/pdf", StringComparison.OrdinalIgnoreCase))
                {
                    return Fail("invalid_mime", "Only PDF files are supported.");
                }

                if (!sadecePdf && !IcerikTipiGecerliMi(dosya.ContentType, pdfDestegi))
                {
                    return Fail("invalid_mime", "File type could not be verified.");
                }

                await using var kaynak = dosya.OpenReadStream();
                if (!await DosyaIcerigiGecerliMiAsync(kaynak, uzanti))
                {
                    return Fail("invalid_signature", "File type could not be verified.");
                }

                kaynak.Position = 0;
                if (await AktifIcerikVarMiAsync(kaynak))
                {
                    return Fail("active_content", "File rejected for security reasons.");
                }

                var klasor = GetSensitiveCategoryFolder(kategori);
                Directory.CreateDirectory(klasor);

                var belgeAdi = Guid.NewGuid().ToString("N") + uzanti;
                var tamYol = Path.GetFullPath(Path.Combine(klasor, belgeAdi));

                if (!tamYol.StartsWith(Path.GetFullPath(klasor), StringComparison.OrdinalIgnoreCase))
                {
                    return Fail("invalid_path", "File could not be saved.");
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
                return Fail("save_failed", "File could not be saved.");
            }
        }

        public async Task<FileSaveResultDto> GeciciHassasBelgeKaydetAsync(
            IFormFile dosya,
            HassasBelgeKategorisi kategori,
            string storageKey)
        {
            if (!IsSafeStorageKey(storageKey))
            {
                return new FileSaveResultDto { Success = false, ErrorMessage = "Invalid temporary upload key." };
            }

            var kayit = await HassasBelgeKaydetAsync(dosya, kategori);
            if (!kayit.Success)
            {
                return new FileSaveResultDto { Success = false, ErrorMessage = kayit.ErrorMessage };
            }

            var permanentPath = Path.Combine(GetSensitiveCategoryFolder(kategori), kayit.BelgeAdi);
            var temporaryRoot = GetTemporaryCategoryFolder(storageKey, kategori);
            Directory.CreateDirectory(temporaryRoot);
            var temporaryPath = Path.GetFullPath(Path.Combine(temporaryRoot, kayit.BelgeAdi));

            try
            {
                File.Move(permanentPath, temporaryPath);
                return new FileSaveResultDto
                {
                    Success = true,
                    Url = $"temporary://{GetCategorySegment(kategori)}/{storageKey}/{kayit.BelgeAdi}"
                };
            }
            catch
            {
                HassasBelgeSil(kategori, kayit.BelgeAdi);
                return new FileSaveResultDto { Success = false, ErrorMessage = "File could not be moved to temporary area." };
            }
        }

        public bool GeciciBelgeyiKaliciYap(
            string temporaryReference,
            string storageKey,
            HassasBelgeKategorisi expectedCategory,
            out string privateReference)
        {
            privateReference = string.Empty;
            if (!TryParseTemporaryReference(temporaryReference, out var kategori, out var referenceStorageKey, out var fileName) ||
                kategori != expectedCategory ||
                !string.Equals(referenceStorageKey, storageKey, StringComparison.Ordinal))
            {
                return false;
            }

            var temporaryRoot = GetTemporaryCategoryFolder(storageKey, kategori);
            var sourcePath = Path.GetFullPath(Path.Combine(temporaryRoot, fileName));
            var destinationRoot = GetSensitiveCategoryFolder(kategori);
            Directory.CreateDirectory(destinationRoot);
            var destinationPath = Path.GetFullPath(Path.Combine(destinationRoot, fileName));

            if (!sourcePath.StartsWith(temporaryRoot, StringComparison.OrdinalIgnoreCase) ||
                !destinationPath.StartsWith(destinationRoot, StringComparison.OrdinalIgnoreCase) ||
                !File.Exists(sourcePath) ||
                File.Exists(destinationPath))
            {
                return false;
            }

            try
            {
                File.Move(sourcePath, destinationPath);
                privateReference = BuildPrivateReference(kategori, fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool KaliciBelgeyiGeciciyeGeriAl(
            string privateReference,
            string storageKey,
            HassasBelgeKategorisi expectedCategory,
            out string temporaryReference)
        {
            temporaryReference = string.Empty;
            if (!TryParsePrivateReference(privateReference, out var kategori, out var fileName) ||
                kategori != expectedCategory ||
                !IsSafeStorageKey(storageKey))
            {
                return false;
            }

            var sourceRoot = GetSensitiveCategoryFolder(kategori);
            var destinationRoot = GetTemporaryCategoryFolder(storageKey, kategori);
            Directory.CreateDirectory(destinationRoot);
            var sourcePath = Path.GetFullPath(Path.Combine(sourceRoot, fileName));
            var destinationPath = Path.GetFullPath(Path.Combine(destinationRoot, fileName));

            if (!sourcePath.StartsWith(sourceRoot, StringComparison.OrdinalIgnoreCase) ||
                !destinationPath.StartsWith(destinationRoot, StringComparison.OrdinalIgnoreCase) ||
                !File.Exists(sourcePath) ||
                File.Exists(destinationPath))
            {
                return false;
            }

            try
            {
                File.Move(sourcePath, destinationPath);
                temporaryReference = $"temporary://{GetCategorySegment(kategori)}/{storageKey}/{fileName}";
                return true;
            }
            catch
            {
                return false;
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

        private string GetTemporaryCategoryFolder(string storageKey, HassasBelgeKategorisi kategori)
        {
            var root = Path.GetFullPath(Path.Combine(GetPrivateStorageRoot(), "gecici"));
            var path = Path.GetFullPath(Path.Combine(root, storageKey, GetCategorySegment(kategori)));
            if (!path.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Invalid temporary document path.");
            }

            return path;
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
                "faturalar" => HassasBelgeKategorisi.Fatura,
                _ => default
            };

            if (kategori == default)
            {
                return false;
            }

            belgeAdi = parts[1];
            return true;
        }

        public static bool TryParseTemporaryReference(
            string? reference,
            out HassasBelgeKategorisi kategori,
            out string storageKey,
            out string belgeAdi)
        {
            kategori = default;
            storageKey = string.Empty;
            belgeAdi = string.Empty;
            if (string.IsNullOrWhiteSpace(reference) ||
                !reference.StartsWith("temporary://", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var parts = reference["temporary://".Length..]
                .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length != 3 || !IsSafeStorageKey(parts[1]) || !IsSafeStoredFileName(parts[2]))
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

            storageKey = parts[1];
            belgeAdi = parts[2];
            return true;
        }

        private static bool IsSafeStorageKey(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                value.Length == 32 &&
                value.All(c => char.IsAsciiHexDigit(c));
        }

        public static string GetCategorySegment(HassasBelgeKategorisi kategori)
        {
            return kategori switch
            {
                HassasBelgeKategorisi.Recete => "receteler",
                HassasBelgeKategorisi.Fatura => "faturalar",
                _ => "kimlikler"
            };
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
