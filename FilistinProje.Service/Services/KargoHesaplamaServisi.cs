using System.Linq;
using System.Threading.Tasks;
using FilistinProje.Data;
using FilistinProje.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Service.Services
{
    public class KargoHesaplamaServisi : IKargoHesaplamaServisi
    {
        private readonly KanvasDbContext _context;

        public KargoHesaplamaServisi(KanvasDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> HesaplaAsync(string sehir, decimal siparisToplami, decimal ucretsizKargoLimiti)
        {
            if (string.IsNullOrWhiteSpace(sehir))
                return 0;

            if (siparisToplami >= ucretsizKargoLimiti)
                return 0;

            var bolgeFiyat = await _context.KargoBolgeler
                .Where(x => !x.SilindiMi && x.Fiyat > 0)
                .Where(x => x.Sehirler.Any(s =>
                    !s.SilindiMi &&
                    (s.SehirAdi == sehir || s.SehirAdiEn == sehir || s.SehirAdiAr == sehir)))
                .Select(x => (decimal?)x.Fiyat)
                .FirstOrDefaultAsync();

            if (bolgeFiyat.HasValue)
                return bolgeFiyat.Value;

            var firmaFiyati = await _context.KargoBolgeFiyatlari
                .Where(x => !x.SilindiMi && x.Fiyat > 0)
                .Where(x => x.Bolge.Sehirler.Any(s =>
                    !s.SilindiMi &&
                    (s.SehirAdi == sehir || s.SehirAdiEn == sehir || s.SehirAdiAr == sehir)))
                .Where(x => x.KargoFirmasi.AktifMi && !x.KargoFirmasi.SilindiMi)
                .Select(x => (decimal?)x.Fiyat)
                .FirstOrDefaultAsync();

            return firmaFiyati ?? 0;
        }

        public async Task<bool> SehirdeAktifKargoVarMiAsync(string sehir)
        {
            if (string.IsNullOrWhiteSpace(sehir))
                return false;

            var bolgeFiyatVar = await _context.KargoBolgeler
                .Where(x => !x.SilindiMi)
                .Where(x => x.Sehirler.Any(s =>
                    !s.SilindiMi &&
                    (s.SehirAdi == sehir || s.SehirAdiEn == sehir || s.SehirAdiAr == sehir)))
                .AnyAsync(x => x.Fiyat > 0);

            if (bolgeFiyatVar)
                return true;

            var firmaFiyatVar = await _context.KargoBolgeFiyatlari
                .Where(x => !x.SilindiMi && x.Fiyat > 0)
                .Where(x => x.Bolge.Sehirler.Any(s =>
                    !s.SilindiMi &&
                    (s.SehirAdi == sehir || s.SehirAdiEn == sehir || s.SehirAdiAr == sehir)))
                .Where(x => x.KargoFirmasi.AktifMi && !x.KargoFirmasi.SilindiMi)
                .AnyAsync();

            return firmaFiyatVar;
        }
    }
}
