using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Tinygubackend.Controllers
{
  [Route("/api/v1/")]
  public class RedirectController : Controller
  {
    private TinyguContext _tinyguContext;
    public RedirectController(TinyguContext tinyguContext)
    {
      _tinyguContext = tinyguContext;
    }
    
    [HttpGet("redirect/{shortUrl}")]
    public IActionResult RedirectToUrl(string shortUrl)
    {
      var query = _tinyguContext.Links.Where(l => l.ShortUrl == shortUrl);
      int count = query.Count();
      if (count == 1)
      {
        string longUrl = query.First().LongUrl;
        if (!longUrl.Contains("http://") && !longUrl.Contains("https://"))
        {
          longUrl = "http://" + longUrl;
        }
        return Redirect(longUrl);
      }
      if (count == 0)
      {
        HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
        return Json(new
        {
          error = $"No LongUrl wit ShortUrl '{shortUrl}' found!"
        });
      }
      HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
      return Json(new
      {
        error = $"Duplicate entry with ShortUrl '{shortUrl}'!"
      });
    }
  }
}
