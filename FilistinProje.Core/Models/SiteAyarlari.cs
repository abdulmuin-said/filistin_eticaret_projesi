namespace FilistinProje.Core.Models
{
    public class SiteAyarlari
    {
        public int Id { get; set; } = 1;
        public string SiteAdi { get; set; } = "7ANRPS48";
        public string MarkaAdi { get; set; } = "7ANRPS48";
        public string SiteBasligi { get; set; } = "7ANRPS48 - متجرك الإلكتروني";
        public string SiteAciklamasi { get; set; } = "منتجات متنوعة بأفضل الأسعار مع خدمة توصيل سريعة";
        public string SiteLogoUrl { get; set; } = "/74anrps48logo2.svg";
        public string FaviconUrl { get; set; } = "/74anrps48logo2.svg";
        public string BaseUrl { get; set; } = "https://www.7anrps48.com";
        public string TemaRengi { get; set; } = "#1a5632";
        public string UstBarMesaji { get; set; } = "توصيل مجاني للطلبات فوق 200 ₪";
        public string KampanyaMesaji { get; set; } = "الدفع عند الاستلام متاح";
        public bool UstBarEtkin { get; set; } = true;
        public double UstBarHizi { get; set; } = 34;
        public string FooterAciklamasi { get; set; } = "متجر إلكتروني فلسطيني يقدم منتجات متنوعة بأسعار منافسة وتوصيل سريع لجميع المدن";
        public string FooterAciklamasiEn { get; set; } = "A Palestinian e-commerce site offering varied products at competitive prices with fast delivery to all cities.";
        public string FooterAciklamasiAr { get; set; } = "متجر إلكتروني فلسطيني يقدم منتجات متنوعة بأسعار منافسة وتوصيل سريع لجميع المدن";
        public string FooterAciklamasiTr { get; set; } = "Rekabetçi fiyatlarla çeşitli ürünler sunan ve tüm şehirlere hızlı teslimat yapan bir Filistin e-ticaret sitesi.";

        public string Telefon { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;
        public string WhatsappNumarasi { get; set; } = string.Empty;
        public string CalismaSaatleri { get; set; } = string.Empty;

        public string FacebookUrl { get; set; } = string.Empty;
        public string InstagramUrl { get; set; } = string.Empty;
        public string TwitterUrl { get; set; } = string.Empty;
        public string YoutubeUrl { get; set; } = string.Empty;
        public string TiktokUrl { get; set; } = string.Empty;
        public string PinterestUrl { get; set; } = string.Empty;

        public string ParaBirimi { get; set; } = "₪";
        public decimal KargoBedeli { get; set; } = 15;
        public decimal UcretsizKargoLimiti { get; set; } = 200;
        public int StokUyariLimiti { get; set; } = 5;
        public bool StoktaYokSatisIzni { get; set; } = false;
        public bool StokBiteniGriGoster { get; set; } = true;

        public bool PaytrAktifMi { get; set; } = false;
        public bool PaytrTestModu { get; set; } = false;
        public string PaytrMerchantId { get; set; } = string.Empty;
        public string PaytrMerchantKeyProtected { get; set; } = string.Empty;
        public string PaytrMerchantSaltProtected { get; set; } = string.Empty;
        public string PaytrCallbackUrl { get; set; } = string.Empty;
        public string PaytrBasariliDonusUrl { get; set; } = string.Empty;
        public string PaytrBasarisizDonusUrl { get; set; } = string.Empty;

        public string KargoFirmasi { get; set; } = "توصيل محلي";
        public string KargoTakipUrl { get; set; } = string.Empty;
        public int SiparisTeslimSuresiGun { get; set; } = 3;
        public int IadeHakkiGun { get; set; } = 7;

        public string MetaTitle { get; set; } = "7ANRPS48 - متجر إلكتروني فلسطيني | تسوق أونلاين";
        public string MetaDescription { get; set; } = "تسوق أونلاين من 7ANRPS48. منتجات متنوعة بأفضل الأسعار مع توصيل سريع لجميع مدن فلسطين.";
        public string MetaKeywords { get; set; } = "تسوق أونلاين, فلسطين, متجر إلكتروني, توصيل, 7ANRPS48";
        public string GoogleAnalyticsId { get; set; } = string.Empty;
        public string FacebookPixelId { get; set; } = string.Empty;
        public string VarsayilanSosyalPaylasimGorseliUrl { get; set; } = "/74anrps48logo2.svg";
        public string CookieMetni { get; set; } = "نستخدم ملفات تعريف الارتباط لتحسين تجربتك وتحليل حركة الموقع.";

        public bool YeniSiparisMailBildirimi { get; set; } = true;
        public bool StokUyarisiMailBildirimi { get; set; } = true;
        public bool IadeTalebiMailBildirimi { get; set; } = true;
        public string BildirimAliciEmail { get; set; } = string.Empty;

        public bool BakimModuAktif { get; set; } = false;
        public string BakimModuMesaji { get; set; } = "نحن نعمل على تحسين الموقع لتقديم تجربة تسوق أفضل. سنعود قريباً!";

        public bool GirisZorunluMu { get; set; } = false;

        public bool KapidaOdemeAktifMi { get; set; } = true;
        public decimal KapidaOdemeHizmetBedeli { get; set; } = 15;
        public decimal KapidaOdemeLimiti { get; set; } = 2000;

        public decimal ToptanciMinSiparisTutari { get; set; } = 500;

        /// <summary>
        /// Müşterinin sipariş verdikten sonra iptal edebileceği süre (saat). 0 = iptal kapalı.
        /// </summary>
        public int IptalSuresiSaat { get; set; } = 3;
    }
}