using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinygubackend.Contexts;
using Tinygubackend.Models;
using Tinygubackend.Services;
using Tinygubackend.Core.Exceptions;

namespace Tinygubackend.Infrastructure
{
    public interface IUserRepository
    {
        Task<AuthReturnInfo> Authorize(string userName, string password);
        Task<List<User>> GetAll();
        Task<User> GetSingle(int id);
        Task<User> UpdateOne(User updatedUser);
        Task<User> CreateOne(User newUser);
        Task DeleteOne(int id);
    }

    public struct AuthReturnInfo
    {
        public string Token { get; set; }

    }
    public class UserRepository : IUserRepository
    {
        private readonly TinyguContext _tinyguContext;
        private readonly IAuthService _authService;

        public UserRepository(TinyguContext tinyguContext, IAuthService authService)
        {
            _tinyguContext = tinyguContext;
            _authService = authService;
        }

        public async Task<AuthReturnInfo> Authorize(string userName, string password)
        {
            // TODO add LDAP
            await _authService.Authorize(userName, password);
            await SetLoginTime(userName);
            return new AuthReturnInfo
            {
                Token = _authService.Token,
            };
        }

        private async Task SetLoginTime(string userName)
        {
            User user = await _tinyguContext.Users.SingleOrDefaultAsync(_ => _.Name == userName);
            user.DateLogin = DateTime.Now;
            await _tinyguContext.SaveChangesAsync();
        }

        public async Task<User> CreateOne(User newUser)
        {
            if (
                String.IsNullOrEmpty(newUser.Email)
                || String.IsNullOrEmpty(newUser.Password)
                || String.IsNullOrEmpty(newUser.Name)
            )
            {
                throw new PropertyIsMissingException();
            }
            if (await DoesUserExist(_ => _.Name == newUser.Name))
            {
                throw new DuplicateEntryException("Name already exists!");
            }
            if (await DoesUserExist(_ => _.Email == newUser.Email))
            {
                throw new DuplicateEntryException("Email already exists!");
            }
            newUser.Password = AuthService.HashPassword(newUser.Password);
            _tinyguContext.Users.Add(newUser);
            await _tinyguContext.SaveChangesAsync();
            return newUser;
        }

        public async Task DeleteOne(int id)
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

        public async Task<User> UpdateOne(User updatedUser)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> DoesUserExist(Func<User, bool> predicate)
        {
            User testUser = await _tinyguContext.Users.SingleOrDefaultAsync(user => predicate(user));
            return testUser != null;
        }
    }
}