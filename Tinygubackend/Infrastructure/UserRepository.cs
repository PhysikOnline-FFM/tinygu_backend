using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tinygubackend.Contexts;
using Tinygubackend.Models;

namespace Tinygubackend.Infrastructure
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User> GetSingle(int id);
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

        public async Task<List<User>> GetAll()
        {
            return await _tinyguContext.Users.Include(u => u.Links).ToListAsync();
        }

        public async Task<User> GetSingle(int id)
        {
            return await _tinyguContext.Users.Include(u => u.Links).SingleOrDefaultAsync(_ => _.Id == id);
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