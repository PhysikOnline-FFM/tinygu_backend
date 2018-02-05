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
using Tinygubackend.Common.Exceptions;
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

        /// <summary>
        /// Authorizes a user.
        /// </summary>
        /// <param name="authInfo">username and password</param>
        /// <returns>An object containing the token.</returns>
        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromBody] AuthInfo authInfo)
        {
            try
            {
                return Json(await _userService.Authorize(authInfo.userName, authInfo.password));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns>List</returns>
        [HttpGet, Authorize]
        public async Task<IActionResult> GetAll()
        {
            return Json(await _userService.GetAll());
        }

        /// <summary>
        /// Returns one user.
        /// </summary>
        /// <param name="id">Id of this user</param>
        /// <returns>The requested user</returns>
        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> OneUser(int id)
        {
            try
            {
                return Json(await _userService.GetSingle(id));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
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
        /// Updates one user.
        /// </summary>
        /// <param name="updatedUser"></param>
        /// <param name="id">Id of this user</param>
        /// <returns>The updated user</returns>
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] User updatedUser, int id)
        {
            try
            {
                return Json(_userService.UpdateOne(updatedUser));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
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
        /// Create one user.
        /// </summary>
        /// <param name="newLink">The user to be created</param>
        /// <returns>The user</returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> CreateOne([FromBody] User user)
        {
            try
            {
                return Json(await _userService.CreateOne(user));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
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

        /// <summary>
        /// Deletes one user.
        /// </summary>
        /// <param name="id">Id of this user</param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteOne(id);
                return StatusCode(200);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
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