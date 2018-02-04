using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tinygubackend;
using Tinygubackend.Core.Exceptions;
using Tinygubackend.Infrastructure;
using Tinygubackend.Models;

namespace Tinygubackend.Controllers
{
    public struct AuthInfo
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
    /// <summary>
    /// Handles Routes related to link operations.
    /// </summary>
    [Route("/api/v1/users/")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userService;

        public UsersController(IUserRepository userService)
        {
            _userService = userService;
        }

        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromBody] AuthInfo authInfo)
        {
            try
            {
                return Json(await _userService.Authorize(authInfo.userName, authInfo.password));
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized();
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAll()
        {
            return Json(await _userService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne([FromBody] User user)
        {
            try
            {
                return Json(await _userService.CreateOne(user));
            }
            catch (Exception e) when (e is DuplicateEntryException || e is PropertyIsMissingException)
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