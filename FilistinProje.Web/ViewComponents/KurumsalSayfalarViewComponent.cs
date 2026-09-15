using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Web.ViewComponents
{
    public class KurumsalSayfalarViewComponent : ViewComponent
    {
        private readonly KanvasDbContext _context;

        public KurumsalSayfalarViewComponent(KanvasDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
        {
            var pages = await _context.KurumsalSayfalar
                .AsNoTracking()
                .Where(x => !x.SilindiMi && !string.IsNullOrWhiteSpace(x.UrlSlug))
                .OrderBy(x => x.Sira)
                .ThenBy(x => x.Id)
                .ToListAsync();

            return View(viewName, pages);
        }
    }
}
