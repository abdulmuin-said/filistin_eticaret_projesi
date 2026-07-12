using FilistinProje.Core.DTOs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FilistinProje.Service.Interfaces
{
    public enum HassasBelgeKategorisi
    {
        Kimlik = 1,
        Recete = 2
    }

    public interface IDosyaServisi
    {
        Task<FileSaveResultDto> KaydetAsync(IFormFile dosya, string altKlasor, bool pdfDestegi = false);

        Task<HassasBelgeKayitDto> HassasBelgeKaydetAsync(IFormFile dosya, HassasBelgeKategorisi kategori);

        bool Sil(string dosyaYolu);

        bool HassasBelgeSil(HassasBelgeKategorisi kategori, string belgeAdi);

        bool EskiWebRootYoluGecerliMi(string? path, string expectedFolder);

        string GetPrivateStorageRoot();
    }
}
