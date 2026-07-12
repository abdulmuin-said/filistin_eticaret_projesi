namespace FilistinProje.Core.Varliklar
{
    public class KargoBolgeSehir : BaseEntity
    {
        public int BolgeId { get; set; }
        public KargoBolge Bolge { get; set; } = null!;
        public string SehirAdi { get; set; } = string.Empty;
        public string? SehirAdiEn { get; set; }
        public string? SehirAdiAr { get; set; }
    }
}
