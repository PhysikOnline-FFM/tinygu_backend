﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinygubackend;
using Tinygubackend.Models;
using Tinygubackend.Infrastructure;
using Tinygubackend.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Tinygubackend.Controllers
{
    /// <summary>
    /// Handles Routes related to link operations.
    /// </summary>
    [Route("/api/v1/links/")]
    public class LinksController : Controller
    {
        private readonly ILinksRepository _linksService;

        public LinksController(ILinksRepository linksService)
        {
            _linksService = linksService;
        }

        /// <summary>
        /// Returns all links.
        /// </summary>
        /// <returns>List</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(_linksService.GetAll());
        }

        /// <summary>
        /// Returns one link.
        /// </summary>
        /// <param name="id">Id of this Link</param>
        /// <returns>The requested link</returns>
        [HttpGet("{id}")]
        public IActionResult OneLink(int id)
        {
            try
            {
                return Json(_linksService.GetSingle(id));
            }
            catch (IdNotFoundException e)
            {
                return BadRequest(ErrorMessage(e.Message));
            }
            catch (Exception e)
            {
                SetHttpStatusCode(HttpStatusCode.InternalServerError);
                return Json(ErrorMessage(e.Message));
            }
        }

        /// <summary>
        /// Updates one link.
        /// </summary>
        /// <param name="updatedLink"></param>
        /// <param name="id">Id of this Link</param>
        /// <returns>The updated Link</returns>
        [HttpPut("{id}"), Authorize]
        public IActionResult UpdateLink([FromBody] Link updatedLink, int id)
        {
            try
            {
                return Json(_linksService.UpdateOne(updatedLink));
            }
            catch (Exception e) when (e is IdNotFoundException || e is PropertyIsMissingException)
            {
                return BadRequest(ErrorMessage(e.Message));
            }
            catch (DbUpdateException e)
            {
                SetHttpStatusCode(HttpStatusCode.InternalServerError);
                return Json(ErrorMessage(e.InnerException.Message));
            }
            catch (Exception e)
            {
                SetHttpStatusCode(HttpStatusCode.InternalServerError);
                return Json(ErrorMessage(e.Message));
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
                return Json(_linksService.CreateOne(newLink));
            }
            catch (DuplicateEntryException e)
            {
                return BadRequest(ErrorMessage(e.Message));
            }
            catch (DbUpdateException e)
            {
                SetHttpStatusCode(HttpStatusCode.InternalServerError);
                return Json(ErrorMessage(e.InnerException.Message));
            }
            catch (Exception e)
            {
                SetHttpStatusCode(HttpStatusCode.InternalServerError);
                return Json(ErrorMessage(e.Message));
            }
        }

        /// <summary>
        /// Deletes one link.
        /// </summary>
        /// <param name="id">Id of this Link</param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteLink(int id)
        {
            try
            {
                _linksService.DeleteOne(id);
                return StatusCode(200);
            }
            catch (IdNotFoundException e)
            {
                return BadRequest(ErrorMessage(e.Message));
            }
            catch (DbUpdateException e)
            {
                SetHttpStatusCode(HttpStatusCode.InternalServerError);
                return Json(ErrorMessage(e.InnerException.Message));
            }
            catch (Exception e)
            {
                SetHttpStatusCode(HttpStatusCode.InternalServerError);
                return Json(ErrorMessage(e.Message));
            }
        }

        private void SetHttpStatusCode(HttpStatusCode code)
        {
            HttpContext.Response.StatusCode = (int)code;
        }

        private object ErrorMessage(string error)
        {
            return new { error };
        }
    }
}