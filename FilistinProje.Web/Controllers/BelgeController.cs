using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Interfaces;
using FilistinProje.Service.Services;
using FilistinProje.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Web.Controllers
{
    [Authorize]
    public class BelgeController : Controller
    {
        private readonly KanvasDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDosyaServisi _dosyaServisi;
        private readonly IWebHostEnvironment _env;

        public BelgeController(
            KanvasDbContext context,
            UserManager<AppUser> userManager,
            IDosyaServisi dosyaServisi,
            IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _dosyaServisi = dosyaServisi;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Kimlik(string userId, bool indir = false)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return Challenge();
            }

            var isOwner = string.Equals(currentUserId, userId, StringComparison.Ordinal);
            var isAdminAllowed = AdminPermissionMatrix.CanAccess(User, "Kullanici", HttpMethods.Get) ||
                AdminPermissionMatrix.CanAccess(User, "Siparis", HttpMethods.Get) ||
                AdminPermissionMatrix.CanAccess(User, "Toptanci", HttpMethods.Get);

            if (!isOwner && !isAdminAllowed)
            {
                return Forbid();
            }

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null || string.IsNullOrWhiteSpace(user.KimlikFotografYolu))
            {
                return NotFound();
            }

            return await ReturnSensitiveDocumentAsync(user.KimlikFotografYolu, HassasBelgeKategorisi.Kimlik, indir, $"kimlik_{user.Id}");
        }

        [HttpGet]
        public async Task<IActionResult> SiparisKimlik(int siparisId, bool indir = false)
        {
            var siparis = await _context.Siparisler
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == siparisId && !x.SilindiMi);

            if (siparis == null || string.IsNullOrWhiteSpace(siparis.KimlikFotoYolu))
            {
                return NotFound();
            }

            var auth = await CanAccessOrderDocumentAsync(siparis);
            if (!auth)
            {
                return Forbid();
            }

            return await ReturnSensitiveDocumentAsync(siparis.KimlikFotoYolu, HassasBelgeKategorisi.Kimlik, indir, $"siparis-kimlik_{siparis.Id}");
        }

        [HttpGet]
        public async Task<IActionResult> Recete(int siparisId, bool indir = false)
        {
            var siparis = await _context.Siparisler
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == siparisId && !x.SilindiMi);

            if (siparis == null || string.IsNullOrWhiteSpace(siparis.ReceteDosyaYolu))
            {
                return NotFound();
            }

            var auth = await CanAccessOrderDocumentAsync(siparis);
            if (!auth)
            {
                return Forbid();
            }

            return await ReturnSensitiveDocumentAsync(siparis.ReceteDosyaYolu, HassasBelgeKategorisi.Recete, indir, $"recete_{siparis.Id}");
        }

        [HttpGet]
        public async Task<IActionResult> Fatura(int siparisId, bool indir = true)
        {
            var siparis = await _context.Siparisler
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == siparisId && !x.SilindiMi);

            if (siparis == null || string.IsNullOrWhiteSpace(siparis.FaturaDosyaYolu))
            {
                return NotFound();
            }

            if (!await CanAccessOrderDocumentAsync(siparis))
            {
                return Forbid();
            }

            return await ReturnSensitiveDocumentAsync(
                siparis.FaturaDosyaYolu,
                HassasBelgeKategorisi.Fatura,
                indir,
                $"fatura_{siparis.Id}");
        }

        private async Task<bool> CanAccessOrderDocumentAsync(Siparis siparis)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(siparis.AppUserId) && string.Equals(siparis.AppUserId, currentUserId, StringComparison.Ordinal))
            {
                return true;
            }

            if (AdminPermissionMatrix.CanAccess(User, "Siparis", HttpMethods.Get))
            {
                return true;
            }

            var email = User.Identity?.Name;
            if (!string.IsNullOrWhiteSpace(email) && string.Equals(email, siparis.Eposta, StringComparison.OrdinalIgnoreCase))
            {
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == currentUserId);
                return user != null && string.Equals(user.Email, siparis.Eposta, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        private async Task<IActionResult> ReturnSensitiveDocumentAsync(string storedReference, HassasBelgeKategorisi expectedCategory, bool download, string safeBaseName)
        {
            var path = ResolveSensitiveDocumentPath(storedReference, expectedCategory, out var storedFileName);
            if (path == null || !System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var contentType = DosyaServisi.GetSafeContentType(storedFileName);
            var extension = Path.GetExtension(storedFileName).ToLowerInvariant();
            var downloadName = MakeSafeDownloadName(safeBaseName, extension);

            Response.Headers["Cache-Control"] = "no-store, no-cache, max-age=0";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            Response.Headers["X-Content-Type-Options"] = "nosniff";

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 64 * 1024, FileOptions.Asynchronous | FileOptions.SequentialScan);
            if (download)
            {
                return File(stream, contentType, downloadName, enableRangeProcessing: false);
            }

            Response.Headers["Content-Disposition"] = $"inline; filename=\"{downloadName}\"";
            return File(stream, contentType, enableRangeProcessing: false);
        }

        private string? ResolveSensitiveDocumentPath(string storedReference, HassasBelgeKategorisi expectedCategory, out string storedFileName)
        {
            storedFileName = string.Empty;

            if (DosyaServisi.TryParsePrivateReference(storedReference, out var kategori, out var privateFileName))
            {
                if (kategori != expectedCategory)
                {
                    return null;
                }

                storedFileName = privateFileName;
                return BuildSafePrivatePath(kategori, privateFileName);
            }

            var expectedLegacyFolder = expectedCategory switch
            {
                HassasBelgeKategorisi.Recete => "uploads/receteler",
                HassasBelgeKategorisi.Fatura => "uploads/invoices",
                _ => "uploads/kimlikler"
            };

            if (!_dosyaServisi.EskiWebRootYoluGecerliMi(storedReference, expectedLegacyFolder))
            {
                return null;
            }

            storedFileName = Path.GetFileName(storedReference.Replace('\\', '/'));
            var legacyPath = Path.GetFullPath(Path.Combine(_env.WebRootPath, expectedLegacyFolder, storedFileName));
            var legacyRoot = Path.GetFullPath(Path.Combine(_env.WebRootPath, expectedLegacyFolder));

            return legacyPath.StartsWith(legacyRoot, StringComparison.OrdinalIgnoreCase)
                ? legacyPath
                : null;
        }

        private string? BuildSafePrivatePath(HassasBelgeKategorisi kategori, string fileName)
        {
            if (!DosyaServisi.IsSafeStoredFileName(fileName))
            {
                return null;
            }

            var root = Path.GetFullPath(Path.Combine(_dosyaServisi.GetPrivateStorageRoot(), "hassas", DosyaServisi.GetCategorySegment(kategori)));
            var path = Path.GetFullPath(Path.Combine(root, fileName));
            return path.StartsWith(root, StringComparison.OrdinalIgnoreCase) ? path : null;
        }

        private static string MakeSafeDownloadName(string baseName, string extension)
        {
            var safe = new string(baseName.Where(c => char.IsLetterOrDigit(c) || c is '-' or '_').ToArray());
            if (string.IsNullOrWhiteSpace(safe))
            {
                safe = "belge";
            }

            return safe + extension;
        }
    }
}


