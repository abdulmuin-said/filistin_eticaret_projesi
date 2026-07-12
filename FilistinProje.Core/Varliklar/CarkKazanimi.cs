namespace FilistinProje.Core.Varliklar
{
    public class CarkKazanimi : BaseEntity
    {
        public string AppUserId { get; set; } = string.Empty;
        public int CarkOdulId { get; set; }
        public int? KuponId { get; set; }

        public AppUser? AppUser { get; set; }
        public CarkOdul? CarkOdul { get; set; }
        public Kupon? Kupon { get; set; }
    }
}
