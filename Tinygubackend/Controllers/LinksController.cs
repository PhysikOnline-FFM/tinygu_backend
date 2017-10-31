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

        /// <summary>
        /// Returns all links.
        /// </summary>
        /// <returns>List</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Json(_tinyguContext.Links.ToList());
        }

        /// <summary>
        /// Returns one link.
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <returns></returns>
        [HttpGet("{shortUrl}")]
        public IActionResult OneLink(string shortUrl)
        {
            return Json(_tinyguContext.Links.Where(l => l.ShortUrl == shortUrl));
        }

        /// <summary>
        /// Create one link.
        /// </summary>
        /// <param name="newLink"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateLink([FromBody] Link newLink)
        {
            try
            {
                _tinyguContext.Links.Add(newLink);
                _tinyguContext.SaveChanges();
                return Content("Success");
            }
            catch (DbUpdateException e)
            {
                SetHttpStatusCode(HttpStatusCode.InternalServerError);
                return Json(new
                {
                    error = e.InnerException.Message
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