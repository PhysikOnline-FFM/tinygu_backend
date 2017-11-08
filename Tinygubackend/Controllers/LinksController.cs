using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinygubackend;
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
        /// <param name="id">Id of this Link</param>
        /// <returns>The requested link</returns>
        [HttpGet("{id}")]
        public IActionResult OneLink(int id)
        {
            Link link = _tinyguContext.Links.SingleOrDefault(l => l.Id == id);

            if (link == null)
            {
                return StatusCode(400);
            }

            return Json(link);
        }

        /// <summary>
        /// Updates one link.
        /// </summary>
        /// <param name="updatedLink"></param>
        /// <param name="id">Id of this Link</param>
        /// <returns>The updated Link</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateLink([FromBody] Link updatedLink, int id)
        {
            try
            {
                Link link = _tinyguContext.Links.SingleOrDefault(_ => _.Id == id);

                if (link == null)
                {
                    return StatusCode(400);
                }

                link.ShortUrl = updatedLink.ShortUrl;
                link.LongUrl = updatedLink.LongUrl;
                link.Owner = updatedLink.Owner;

                _tinyguContext.SaveChanges();
                return Json(link);
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

        /// <summary>
        /// Create one link.
        /// </summary>
        /// <param name="newLink">The link to be created</param>
        /// <returns>The link</returns>
        [HttpPost]
        public IActionResult CreateLink([FromBody] Link newLink)
        {
            try
            {
                _tinyguContext.Links.Add(newLink);
                _tinyguContext.SaveChanges();
                return Json(newLink);
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

        /// <summary>
        /// Deletes one link.
        /// </summary>
        /// <param name="id">Id of this Link</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteLink(int id)
        {
            try
            {
                Link link = _tinyguContext.Links.SingleOrDefault(_ => _.Id == id);
                if (link == null)
                {
                    return StatusCode(400);
                }
                _tinyguContext.Links.Remove(link);
                _tinyguContext.SaveChanges();
                return StatusCode(200);
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