using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Tinygubackend;

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

    /// <summary>
    /// Redirect to a longUrl.
    /// </summary>
    /// <param name="shortUrl"></param>
    /// <returns></returns>
    [HttpGet("redirect/{shortUrl}")]
    public IActionResult RedirectToUrl(string shortUrl)
    {
      try
      {
        // throws if no entry was found
        var query = _tinyguContext.Links.First(l => l.ShortUrl == shortUrl);

        string longUrl = query.LongUrl;
        if (!longUrl.Contains("http://") && !longUrl.Contains("https://"))
        {
          longUrl = "http://" + longUrl;
        }
        return Redirect(longUrl);
      }
      catch (InvalidOperationException e)
      {
        SetHttpStatusCode(HttpStatusCode.BadRequest);
        return Json(new
        {
          error = $"No Url with shortUrl '{shortUrl}'",
          details = e.Message
        });
      }
      catch (Exception e)
      {
        SetHttpStatusCode(HttpStatusCode.InternalServerError);
        return Json(new
        {
          error = e.Message
        });
      }
    }

    private void SetHttpStatusCode(HttpStatusCode code)
    {
      HttpContext.Response.StatusCode = (int)code;
    }
  }
}
