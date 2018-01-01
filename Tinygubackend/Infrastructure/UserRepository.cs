using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tinygubackend.Contexts;
using Tinygubackend.Models;

namespace Tinygubackend.Infrastructure
{
    public class UserRepository
    {
        private readonly TinyguContext _tinyguContext;

        public UserRepository(TinyguContext tinyguContext)
        {
            _tinyguContext = tinyguContext;
        }

        public List<User> GetAll()
        {
            return _tinyguContext.Users.Include(u => u.Links).ToList();
        }
    }
}