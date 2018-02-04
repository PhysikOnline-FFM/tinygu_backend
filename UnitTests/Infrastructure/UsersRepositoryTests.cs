using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Tinygubackend.Contexts;
using Tinygubackend.Infrastructure;
using Tinygubackend.Models;
using Tinygubackend.Services;
using Xunit;
using System.Threading.Tasks;

namespace UnitTests.Infrastructure
{
    public class UsersRepositoryTests
    {
        private List<User> _data = new List<User>
        {
            new User
            {
                Name = "Test Name",
                Links = new List<Link>(),
            },
            new User
            {
                Name = "Test Name2",
                Links = new List<Link>
                {
                new Link
                {
                    LongUrl = "google.com2",
                    ShortUrl = "g2",
                    Owner = null,
                    DateCreated = DateTime.Parse("2017-11-08T12:07:55.323428")
                }
                }
            },
            new User
            {
                Name = "Test Name3",
                Links = new List<Link>()
            }
        };

        private List<Link> _links = new List<Link>
        {
            new Link
            {
                LongUrl = "google.com2",
                ShortUrl = "g2",
                Owner = null,
                DateCreated = DateTime.Parse("2017-11-08T12:07:55.323428")
            }
        };

        private IAuthService _authService;

        public UsersRepositoryTests()
        {
            Mock<IAuthService> authService = new Mock<IAuthService>();
            authService.SetupGet(x => x.Token).Returns("token");
            _authService = authService.Object;
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
            context.Users.AddRange(_data);
            context.SaveChanges();
        }

        [Fact]
        public async void GetsAllUsers()
        {
            string name = "Get_All_Users";
            PopulateDB(name);

            using (var context = GetContext(name))
            {
                var service = new UserRepository(context, _authService);
                var users = await service.GetAll();
                users.Count.Should().Be(_data.Count);
                for (int i = 0; i < _data.Count; i++)
                {
                    users[i].ShouldBeEquivalentTo(_data[i], options => options.Excluding(_ => _.Links));
                }
            }
        }

        [Fact]
        public async void GetsSingleUser()
        {
            string name = "Get_Single_User";
            PopulateDB(name);

            using (var context = GetContext(name))
            {
                var service = new UserRepository(context, _authService);
                int id = context.Users.SingleOrDefault(_ => _.Name == "Test Name").Id;
                User user = await service.GetSingle(id);
                user.ShouldBeEquivalentTo(_data[0]);
            }
        }

        [Fact]
        public async void AuthorizesUser()
        {
            string name = "Authorize_User";
            PopulateDB(name);

            using (var context = GetContext(name))
            {
                var service = new UserRepository(context, _authService);
                var result = service.Authorize("user", "password").Result;
                result.Token.Should().Be("token");
            }
        }

        [Fact]
        public async void CreatesOneUser()
        {
            string name = "Creates_User";
            PopulateDB(name);

            using (var context = GetContext(name))
            {
                var service = new UserRepository(context, _authService);
                User newUser = new User
                {
                    Name = "New User",
                    Email = "Email",
                    Password = "password"
                };
                User createdUser = await service.CreateOne(newUser);
            }

            using (var context = GetContext(name))
            {
                context.Users.ToList().Count().Should().Be(_data.Count() + 1);
            }
        }
    }
}