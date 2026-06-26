using System.Threading.Tasks;

namespace FilistinProje.Service.Interfaces
{
    public interface IKargoHesaplamaServisi
    {
        Task<decimal> HesaplaAsync(string sehir, decimal siparisToplami, decimal ucretsizKargoLimiti);
    }
}
