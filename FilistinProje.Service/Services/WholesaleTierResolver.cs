using FilistinProje.Core.Varliklar;

namespace FilistinProje.Service.Services;

internal static class WholesaleTierResolver
{
    internal static UrunToptanFiyatKademesi? Resolve(
        IEnumerable<UrunToptanFiyatKademesi> tiers,
        int? urunSecenekId,
        int adet)
    {
        var applicable = tiers
            .Where(x => !x.SilindiMi && x.AktifMi && x.MinAdet <= adet);

        var variantTier = urunSecenekId.HasValue
            ? applicable
                .Where(x => x.UrunSecenekId == urunSecenekId.Value)
                .OrderByDescending(x => x.MinAdet)
                .ThenBy(x => x.Sira)
                .ThenBy(x => x.Id)
                .FirstOrDefault()
            : null;

        return variantTier ?? applicable
            .Where(x => !x.UrunSecenekId.HasValue)
            .OrderByDescending(x => x.MinAdet)
            .ThenBy(x => x.Sira)
            .ThenBy(x => x.Id)
            .FirstOrDefault();
    }
}
