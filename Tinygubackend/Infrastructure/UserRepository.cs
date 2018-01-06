using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tinygubackend.Contexts;
using Tinygubackend.Models;

namespace Tinygubackend.Infrastructure
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetSingle(int id);
        User GetSingle(string userName, string password);
        User UpdateOne(User updatedUser);
        User CreateOne(User newUser);
        void DeleteOne(int id);
    }
    public class UserRepository : IUserRepository
    {
        private readonly TinyguContext _tinyguContext;

        public UserRepository(TinyguContext tinyguContext)
        {
            _tinyguContext = tinyguContext;
        }

        public User CreateOne(User newUser)
        {
            throw new NotImplementedException();
        }

        public void DeleteOne(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            return _tinyguContext.Users.Include(u => u.Links).ToList();
        }

        public User GetSingle(int id)
        {
            throw new NotImplementedException();
        }

        public User GetSingle(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public User UpdateOne(User updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}