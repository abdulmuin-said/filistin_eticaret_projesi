using System.Collections.Generic;

namespace FilistinProje.Core.DTOs
{
    public class SepetMergeResult
    {
        public bool Basarili { get; set; } = true;
        public string? MessageKey { get; set; }
        public string? HataMesaji { get; set; }
        public List<string> EngellenenUrunler { get; set; } = new();
    }
}
