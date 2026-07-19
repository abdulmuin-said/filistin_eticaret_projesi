using FilistinProje.Data;
using FilistinProje.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var optionsBuilder = new DbContextOptionsBuilder<KanvasDbContext>();
optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=filistindb;Username=kanvasuser;Password=changeme_in_production");
using var db = new KanvasDbContext(optionsBuilder.Options);

var products = await db.Urunler.ToListAsync();
var existingSlugs = new List<string>();

foreach (var product in products)
{
    var title = product.Baslik;
    var titleEn = product.BaslikEn;
    
    var sourceText = !string.IsNullOrWhiteSpace(titleEn) ? titleEn : title;
    var baseSlug = SlugHelper.GenerateSlug(sourceText);
    
    var finalSlug = SlugHelper.EnsureUnique(baseSlug, existingSlugs);
    existingSlugs.Add(finalSlug);
    
    product.Slug = finalSlug;
    product.UrlYolu = SlugHelper.GenerateSlug(sourceText);
}

await db.SaveChangesAsync();
Console.WriteLine("All slugs updated successfully.");