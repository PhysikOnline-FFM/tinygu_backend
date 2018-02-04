using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinygubackend.Contexts;
using Tinygubackend.Core.Exceptions;
using Tinygubackend.Models;
using Tinygubackend.Services;
using Xunit;

namespace UnitTests.Services
{
    public class AuthServiceTests
    {
        private List<User> _users = new List<User>
        {
            new User
            {
            Name = "User",
            Email = "Email",
            Role = "Role",
            Password = "$2a$04$8O3jRqAGyE7WJU./vm2tb.Cm.FueyZ.vHBl3yE9jpXLeLSQnIpb8S", // test
            }
        };

        private IConfiguration _config;

        public AuthServiceTests()
        {
            Mock<IConfiguration> configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Jwt:Key"]).Returns("long_super_secret_key");
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("issuer");
            _config = configuration.Object;
        }
        private TinyguContext GetContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<TinyguContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            return new TinyguContext(options);
        }

        private void PopulateDB(string databaseName)
        {
            var context = GetContext(databaseName);
            context.Users.AddRange(_users);
            context.SaveChanges();
        }

        [Fact]
        public async void AuthorizesUser()
        {
            string name = "Authorize_User";
            PopulateDB(name);
            using(var context = GetContext(name))
            {
                var service = new AuthService(_config, context);
                await service.Authorize("User", "test");
                service.Token.Should().NotBeNullOrEmpty();
            }
        }

        [Fact]
        public void Throws_IfUserNotFound()
        {
            string name = "Throws_If_User_Not_Found";
            PopulateDB(name);
            using(var context = GetContext(name))
            {
                var service = new AuthService(_config, context);
                Func<Task> act = async() => await service.Authorize("User-not-available", "test");
                act.ShouldThrow<EntityNotFoundException>();
                act = async() => await service.Authorize("User", "not-test");
                act.ShouldThrow<UnauthorizedAccessException>();
            }
        }
    }
}