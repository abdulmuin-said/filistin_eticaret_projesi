namespace FilistinProje.Core.DTOs
{
    public class HassasBelgeKayitDto
    {
        public bool Success { get; set; }
        public string BelgeAdi { get; set; } = string.Empty;
        public string Kategori { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Boyut { get; set; }
        public string? HataKodu { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
