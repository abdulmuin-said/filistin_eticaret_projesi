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

            var fiyat = await _context.KargoBolgeler
                .Where(x => !x.SilindiMi)
                .Where(x => x.Sehirler.Any(s => s.SehirAdi == sehir && !s.SilindiMi))
                .Select(x => (decimal?)x.Fiyat)
                .FirstOrDefaultAsync();

            return fiyat ?? 0;
        }
    }
}
