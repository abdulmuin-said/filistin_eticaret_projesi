namespace FilistinProje.Core.Models
{
    public class PersonelIndexViewModel
    {
        public PersonelStatViewModel Stats { get; set; } = new();
        public List<PersonelListItemViewModel> Personel { get; set; } = new();
        public List<PersonelRoleGroupViewModel> RolGruplari { get; set; } = new();
    }

    public class PersonelStatViewModel
    {
        public int ToplamPersonel { get; set; }
        public int SuanAktif { get; set; }
        public int Son7GunGiris { get; set; }
        public int BlokeEdilen { get; set; }
    }

    public class PersonelListItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string AdSoyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string RolAdi { get; set; } = string.Empty;
        public string RolLabel { get; set; } = string.Empty;
        public DateTime? SonGirisUtc { get; set; }
        public DateTime? OncekiGirisUtc { get; set; }
        public bool EngelliMi { get; set; }
        public bool KendisiMi { get; set; }
        public DateTime KayitTarihi { get; set; }
    }

    public class PersonelRoleGroupViewModel
    {
        public string RolAdi { get; set; } = string.Empty;
        public string RolLabel { get; set; } = string.Empty;
        public string RolAciklamasi { get; set; } = string.Empty;
        public int PersonelSayisi { get; set; }
        public int Sira { get; set; }
        public List<PersonelListItemViewModel> Personeller { get; set; } = new();
    }

    public class YetkiMatrisiViewModel
    {
        public List<YetkiMatrisiRoleItem> Roller { get; set; } = new();
        public List<YetkiMatrisiControllerItem> Controllerlar { get; set; } = new();
        public Dictionary<string, Dictionary<string, bool>> Matris { get; set; } = new();
    }

    public class YetkiMatrisiRoleItem
    {
        public string RolAdi { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Renk { get; set; } = "secondary";
        public int Sira { get; set; }
    }

    public class YetkiMatrisiControllerItem
    {
        public string ControllerAdi { get; set; } = string.Empty;
        public string DisplayAdi { get; set; } = string.Empty;
        public string Grup { get; set; } = string.Empty;
        public string Ikon { get; set; } = "fa-circle";
    }
}
