namespace FilistinProje.Core.Helpers
{
    /// <summary>
    /// Vitrinde gösterilen indirim oranları için tek hesaplama noktasıdır.
    /// Pozitif oranlarda JavaScript'teki Math.round ile aynı olacak şekilde
    /// yarım değerleri sıfırdan uzağa yuvarlar.
    /// </summary>
    public static class IndirimHesaplayici
    {
        public static int? YuzdeHesapla(decimal eskiFiyat, decimal etkinFiyat)
        {
            if (eskiFiyat <= 0 || etkinFiyat < 0 || etkinFiyat >= eskiFiyat)
            {
                return null;
            }

            var yuzde = ((eskiFiyat - etkinFiyat) / eskiFiyat) * 100m;
            return decimal.ToInt32(decimal.Round(yuzde, 0, MidpointRounding.AwayFromZero));
        }
    }
}
