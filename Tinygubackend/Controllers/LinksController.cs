using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinygubackend.Models;

namespace Tinygubackend.Controllers
{
    [Route("/api/v1/links/")]
    public class LinksController : Controller
    {
        private TinyguContext _tinyguContext;
        public LinksController(TinyguContext tinyguContext)
        {
            _tinyguContext = tinyguContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Json(_tinyguContext.Links.ToList());
        }

        [HttpGet("{shortUrl}")]
        public IActionResult OneLink(string shortUrl)
        {
            return Json(_tinyguContext.Links.Where(l => l.ShortUrl == shortUrl));
        }

        [HttpPost]
        public IActionResult CreateLink([FromBody] Link newLink)
        {
            if (_tinyguContext.Links.Count(l => l.ShortUrl == newLink.ShortUrl) != 0)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new
                {
                    error = "Duplicate ShortUrl"
                });
            }
            _tinyguContext.Links.Add(newLink);
            _tinyguContext.SaveChanges();
            return Content("Success");
        }
    }
}