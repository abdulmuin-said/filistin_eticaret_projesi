using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> ClaimCoupon([FromBody] WheelClaimRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                return Ok(new { error = "Kupon kodu gerekli." });
            }

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

            var existingClaim = await _context.Kuponlar
                .AnyAsync(k => k.Kod == request.Code && !k.SilindiMi);
            if (existingClaim)
            {
                HttpContext.Session.SetString("UygulananKupon", request.Code);
                return Ok(new { redirect = "/Sepet" });
            }

            var kupon = new Kupon
            {
                Kod = request.Code,
                Tip = 0,
                Deger = request.DiscountType == "freeship" ? 0 : request.DiscountValue,
                MinSepetTutari = 0,
                SonKullanmaTarihi = DateTime.UtcNow.AddDays(30),
                KullanimLimiti = 1,
                KullanilanMiktar = 0,
                AktifMi = true
            };

            _context.Kuponlar.Add(kupon);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("UygulananKupon", request.Code);

            return Ok(new { redirect = "/Sepet" });
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

    public class WheelClaimRequest
    {
        public string Code { get; set; } = string.Empty;
        public string DiscountType { get; set; } = "discount";
        public int DiscountValue { get; set; }
    }
}
