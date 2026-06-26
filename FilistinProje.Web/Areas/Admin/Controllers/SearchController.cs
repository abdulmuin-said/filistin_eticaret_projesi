using Microsoft.AspNetCore.Mvc;
using FilistinProje.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using FilistinProje.Web.Resources;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SearchController(KanvasDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string q)
        {
            if (string.IsNullOrEmpty(q)) return RedirectToAction("Index", "Home");

            q = q.ToLower();

            var tipProduct = _localizer["Admin_SearchTipProduct"].Value;
            var tipCustomer = _localizer["Admin_SearchTipCustomer"].Value;
            var noName = _localizer["Admin_SearchNoName"].Value;

            var urunler = await _context.Urunler
                .Where(x => x.Baslik.ToLower().Contains(q) || x.Id.ToString() == q)
                .Select(x => new SearchResult { 
                    Baslik = x.Baslik, 
                    Tip = tipProduct, 
                    Url = $"/Admin/Urun/Duzenle/{x.Id}",
                    Detay = $"₪{x.Fiyat}" 
                })
                .ToListAsync();

            var musteriler = await _context.Users
                .Where(x => (x.Email != null && x.Email.ToLower().Contains(q)) || 
                            (x.UserName != null && x.UserName.ToLower().Contains(q)) || 
                            (x.AdSoyad != null && x.AdSoyad.ToLower().Contains(q)))
                .Select(x => new SearchResult { 
                    Baslik = x.AdSoyad ?? x.Email ?? noName, 
                    Tip = tipCustomer, 
                    Url = $"/Admin/Kullanici/Duzenle/{x.Id}",
                    Detay = x.Email ?? ""
                })
                .ToListAsync();

            var sonuclar = new List<SearchResult>();
            sonuclar.AddRange(urunler);
            sonuclar.AddRange(musteriler);

            ViewBag.ArananKelime = q;
            return View(sonuclar);
        }
    }

    // Basit bir DTO sınıfı
    public class SearchResult
    {
        public string Baslik { get; set; } = string.Empty;
        public string Tip { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Detay { get; set; } = string.Empty;
    }
}
