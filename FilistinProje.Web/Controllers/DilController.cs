using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace FilistinProje.Web.Controllers
{
    public class DilController : Controller
    {
        [HttpGet]
        public IActionResult Degistir(string culture, string returnUrl = "/")
        {
            var supportedCultures = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ar",
                "en",
                "tr"
            };

            if (!supportedCultures.Contains(culture))
            {
                culture = "ar";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                });

            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            return LocalRedirect(returnUrl);
        }
    }
}
