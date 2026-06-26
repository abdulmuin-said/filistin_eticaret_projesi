namespace FilistinProje.Core.Varliklar
{
    public class KargoBolgeFiyat : BaseEntity
    {
        public int KargoFirmasiId { get; set; }
        public KargoFirmasi KargoFirmasi { get; set; } = null!;
        public int BolgeId { get; set; }
        public KargoBolge Bolge { get; set; } = null!;
        public decimal Fiyat { get; set; }
    }
}
