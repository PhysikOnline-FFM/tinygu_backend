using System;
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
using Tinygubackend.Core.Exceptions;

namespace Tinygubackend.Controllers
{
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

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }

        private void SetHttpStatusCode(HttpStatusCode code)
        {
            HttpContext.Response.StatusCode = (int)code;
        }

        private object ErrorMessage(string error)
        {
            return new {error};
        }
    }
}