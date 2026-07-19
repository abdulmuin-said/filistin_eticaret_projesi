using FilistinProje.Core.Varliklar;
using Microsoft.AspNetCore.Mvc;

namespace FilistinProje.Web.Controllers
{
    public class SozlesmelerController : Controller
    {
        [HttpGet]
        public IActionResult Gizlilik()
        {
            return View("~/Views/Kurumsal/Gizlilik.cshtml");
        }

        [HttpGet]
        public IActionResult MesafeliSatis()
        {
            return View("~/Views/Kurumsal/Detay.cshtml", MesafeliSatisSayfasi());
        }

        private static KurumsalSayfa MesafeliSatisSayfasi()
        {
            return new KurumsalSayfa
            {
                Baslik = "Mesafeli SatÄ±ÅŸ SÃ¶zleÅŸmesi",
                Icerik = """
                    <p>Bu sÃ¶zleÅŸme, 7ANRPS48 Ã¼zerinden verilen sipariÅŸlerde alÄ±cÄ± ile satÄ±cÄ± arasÄ±ndaki mesafeli satÄ±ÅŸ koÅŸullarÄ±nÄ± aÃ§Ä±klar. SipariÅŸ veren mÃ¼ÅŸteri, Ã¶deme adÄ±mÄ±nÄ± tamamladÄ±ÄŸÄ±nda bu sÃ¶zleÅŸmede yer alan temel koÅŸullarÄ± kabul etmiÅŸ sayÄ±lÄ±r.</p>

                    <h2>1. Taraflar</h2>
                    <p>SatÄ±cÄ±: 7ANRPS48. AlÄ±cÄ±: Web sitesi Ã¼zerinden Ã¼rÃ¼n veya hizmet satÄ±n alan gerÃ§ek ya da tÃ¼zel kiÅŸi. SatÄ±cÄ±ya ait gÃ¼ncel iletiÅŸim bilgilerine <a href="/Kurumsal/Iletisim">Ä°letiÅŸim</a> sayfasÄ±ndan ulaÅŸÄ±labilir.</p>

                    <h2>2. SÃ¶zleÅŸmenin Konusu</h2>
                    <p>Ä°ÅŸbu sÃ¶zleÅŸmenin konusu, alÄ±cÄ±nÄ±n elektronik ortamda sipariÅŸ verdiÄŸi Ã¼rÃ¼nlerin satÄ±ÅŸÄ±, teslimi, cayma hakkÄ±, iade koÅŸullarÄ± ve taraflarÄ±n karÅŸÄ±lÄ±klÄ± hak ve yÃ¼kÃ¼mlÃ¼lÃ¼klerinin belirlenmesidir.</p>

                    <h2>3. ÃœrÃ¼n ve SipariÅŸ Bilgileri</h2>
                    <p>ÃœrÃ¼nÃ¼n adÄ±, adedi, varyasyon seÃ§imi, satÄ±ÅŸ bedeli, Ã¶deme ÅŸekli, teslimat adresi ve sipariÅŸ tarihi sipariÅŸ Ã¶zeti ekranÄ±nda ve sipariÅŸ kayÄ±tlarÄ±nda yer alÄ±r. KiÅŸiye Ã¶zel Ã¶lÃ§Ã¼, tasarÄ±m veya Ã¼retim tercihi iÃ§eren Ã¼rÃ¼nlerde Ã¼retim sÃ¼reci sipariÅŸ onayÄ±ndan sonra baÅŸlar.</p>

                    <h2>4. Teslimat</h2>
                    <p>SipariÅŸler, Ã¼rÃ¼n tipine ve Ã¼retim yoÄŸunluÄŸuna gÃ¶re belirtilen hazÄ±rlÄ±k sÃ¼resi iÃ§inde kargoya teslim edilir. TÃ¼rkiye geneli gÃ¶nderimlerde kargo firmasÄ±ndan kaynaklanan gecikmeler satÄ±cÄ±nÄ±n doÄŸrudan kontrolÃ¼ dÄ±ÅŸÄ±nda olabilir.</p>

                    <h2>5. Cayma HakkÄ± ve Ä°ade</h2>
                    <p>Standart Ã¼rÃ¼nlerde cayma ve iade talepleri ilgili mevzuat Ã§erÃ§evesinde deÄŸerlendirilir. MÃ¼ÅŸterinin Ã¶zel Ã¶lÃ§Ã¼, Ã¶zel tasarÄ±m, kiÅŸiselleÅŸtirme veya sipariÅŸe Ã¶zel Ã¼retim tercihiyle hazÄ±rlanan Ã¼rÃ¼nlerde cayma hakkÄ± sÄ±nÄ±rlÄ± olabilir. DetaylÄ± bilgi iÃ§in <a href="/Kurumsal/IadeKosullari">Ä°ade KoÅŸullarÄ±</a> sayfasÄ± incelenmelidir.</p>

                    <h2>6. Ã–deme ve GÃ¼venlik</h2>
                    <p>Ã–deme iÅŸlemleri gÃ¼venli Ã¶deme altyapÄ±sÄ± Ã¼zerinden gerÃ§ekleÅŸtirilir. Kart bilgileri 7ANRPS48 sunucularÄ±nda saklanmaz. Ã–deme sÄ±rasÄ±nda kullanÄ±lan gÃ¼venlik doÄŸrulamalarÄ± bankanÄ±z veya Ã¶deme saÄŸlayÄ±cÄ±nÄ±z tarafÄ±ndan yÃ¼rÃ¼tÃ¼lÃ¼r.</p>

                    <h2>7. UyuÅŸmazlÄ±k</h2>
                    <p>Taraflar arasÄ±nda doÄŸabilecek uyuÅŸmazlÄ±klarda, yÃ¼rÃ¼rlÃ¼kteki tÃ¼ketici mevzuatÄ± kapsamÄ±nda yetkili tÃ¼ketici hakem heyetleri ve tÃ¼ketici mahkemeleri yetkilidir.</p>
                    """
            };
        }
    }
}


