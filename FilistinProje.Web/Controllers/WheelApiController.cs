using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FilistinProje.Web.Controllers
{
    [Route("api/wheel")]
    [ApiController]
    public class WheelApiController : ControllerBase
    {
        private readonly KanvasDbContext _context;

        public WheelApiController(KanvasDbContext context)
        {
            _context = context;
        }

        [HttpPost("claim")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClaimCoupon()
        {
            var userName = HttpContext.User.Identity?.Name ?? string.Empty;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Ok(new { error = "Giriş yapmanız gerekiyor." });
            }

            var appUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (appUser == null)
            {
                return Ok(new { error = "Kullanıcı bulunamadı." });
            }

            var existingClaim = await _context.CarkKazanimlari
                .AsNoTracking()
                .Include(x => x.Kupon)
                .FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && !x.SilindiMi);
            if (existingClaim != null)
            {
                if (existingClaim.Kupon is { AktifMi: true, SilindiMi: false })
                {
                    HttpContext.Session.SetString("UygulananKupon", existingClaim.Kupon.Kod);
                }
                return Ok(new { redirect = "/Sepet" });
            }

            var prizes = await _context.CarkOdulleri
                .Where(x => x.AktifMi && !x.SilindiMi)
                .OrderBy(x => x.Id)
                .ToListAsync();
            if (prizes.Count == 0)
            {
                return Ok(new { error = "Aktif ödül bulunamadı." });
            }

            var prize = prizes[Random.Shared.Next(prizes.Count)];
            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                Kupon? kupon = null;
                if (prize.Tip != "none")
                {
                    kupon = new Kupon
                    {
                        Kod = $"7ANRPS48-{Guid.NewGuid():N}"[..16].ToUpperInvariant(),
                        Tip = prize.Tip == "freeship" ? 1 : 0,
                        Deger = prize.Tip == "freeship" ? 25 : Math.Clamp(prize.Deger, 1, 50),
                        MinSepetTutari = 0,
                        SonKullanmaTarihi = DateTime.UtcNow.AddDays(30),
                        KullanimLimiti = 1,
                        KullanilanMiktar = 0,
                        AktifMi = true
                    };
                    _context.Kuponlar.Add(kupon);
                    await _context.SaveChangesAsync();
                }

                _context.CarkKazanimlari.Add(new CarkKazanimi
                {
                    AppUserId = appUser.Id,
                    CarkOdulId = prize.Id,
                    KuponId = kupon?.Id
                });
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                if (kupon != null)
                {
                    HttpContext.Session.SetString("UygulananKupon", kupon.Kod);
                }
                return Ok(new { redirect = "/Sepet" });
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                return Ok(new { error = "Çark hakkınız daha önce kullanılmış." });
            }
        }

        [HttpGet("prizes")]
        public async Task<IActionResult> GetPrizes()
        {
            var prizes = await _context.CarkOdulleri
                .Where(x => x.AktifMi && !x.SilindiMi)
                .OrderBy(x => x.Sira)
                .Select(x => new
                {
                    x.LabelTr,
                    x.LabelEn,
                    x.LabelAr,
                    x.Tip,
                    x.Deger,
                    x.Renk,
                    x.MesajTr,
                    x.MesajEn,
                    x.MesajAr
                })
                .ToListAsync();

            return Ok(prizes);
        }
    }

}
