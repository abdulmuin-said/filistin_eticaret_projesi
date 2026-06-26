using System;

namespace FilistinProje.Core.Varliklar
{
    public class BankaHesap : BaseEntity
    {
        public string BankaAdi { get; set; } = string.Empty;
        public string HesapSahibi { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
        public string? SubeKodu { get; set; }
        public string? HesapNo { get; set; }
        public bool AktifMi { get; set; } = true;
        public int Sira { get; set; } = 0;
    }
}
