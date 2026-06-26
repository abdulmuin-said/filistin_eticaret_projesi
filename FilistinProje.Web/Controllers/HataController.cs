using Microsoft.AspNetCore.Mvc;

namespace FilistinProje.Web.Controllers
{
    public class HataController : Controller
    {
        [Route("Hata/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            Response.StatusCode = statusCode;

            return statusCode switch
            {
                404 => View("NotFound"),
                403 => View("ErisimEngellendi"),
                _ => View("GenelHata", statusCode)
            };
        }
    }
}
