using FilistinProje.Core.DTOs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FilistinProje.Service.Interfaces
{
    public interface IDosyaServisi
    {
        Task<FileSaveResultDto> KaydetAsync(IFormFile dosya, string altKlasor, bool pdfDestegi = false);
        bool Sil(string dosyaYolu);
    }
}
