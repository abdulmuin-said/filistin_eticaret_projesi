namespace FilistinProje.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync(); // Değişiklikleri veritabanına kaydet
        void Commit(); // Senkron kaydetme (Gerekirse)
    }
}