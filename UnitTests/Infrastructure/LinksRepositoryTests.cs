using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tinygubackend.Contexts;
using Tinygubackend.Models;
using Tinygubackend.Infrastructure;
using Xunit;
using Tinygubackend.Core.Exceptions;

namespace UnitTests.Infrastructure
{
    public class LinksRepositoryTests
    {
        private List<Link> _data = new List<Link>
        {
            new Link
            {
                LongUrl = "google.com",
                ShortUrl = "g",
                Owner = null,
                DateCreated = DateTime.Parse("2017-11-08T12:07:55.323428")
            },
            new Link
            {
                LongUrl = "google.com2",
                ShortUrl = "g2",
                Owner = null,
                DateCreated = DateTime.Parse("2017-11-08T12:07:55.323428")
            },
            new Link
            {
                LongUrl = "google.com3",
                ShortUrl = "g3",
                Owner = null,
                DateCreated = DateTime.Parse("2017-11-08T12:07:55.323428")
            },
        };

        private Link _updateLink = new Link
        {
            LongUrl = "test",
            ShortUrl = "test",
            Owner = null,
            Id = 2,
        };

        private Link _updateLink_wrongId = new Link
        {
            LongUrl = "test",
            ShortUrl = "test",
            Owner = null,
            Id = 4,
        };

        private Link _updateLink_missingUrl = new Link
        {
            LongUrl = "test",
            Owner = null,
            Id = 2,
        };

        private Link _newLink = new Link
        {
            LongUrl = "test",
            ShortUrl = "test",            
            Owner = null,
        };

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
            context.Links.AddRange(_data);
            context.SaveChanges();
        }

        [Fact]
        public void GetsAllLinks()
        {
            string name = "Get_All_Links";
            PopulateDB(name);

            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                var links = service.GetAll();
                links.Count.Should().Be(_data.Count);
                for (int i = 0; i < _data.Count; i++)
                {
                    links[i].ShouldBeEquivalentTo(_data[i]);
                }
            }
        }

        [Fact]
        public void GetsSingleLink()
        {
            string name = "Gets_Single_Link";
            PopulateDB(name);
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                var link = service.GetSingle(2);
                link.ShouldBeEquivalentTo(_data[1]);
            }
        }

        [Fact]
        public void GetsSingle_Throws_IfNotFound()
        {
            string name = "Gets_Single_Throws";
            PopulateDB(name);
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                Action act = () => service.GetSingle(4);
                act.ShouldThrow<IdNotFoundException>();
            }
        }

        [Fact]
        public void UpdateSingleLink()
        {
            string name = "Update_Single_Link";
            PopulateDB(name);

            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                var link = service.UpdateOne(_updateLink);
                link.ShouldBeEquivalentTo(_updateLink, options =>
                    options.Excluding(o => o.DateCreated));
            }
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                service.GetSingle(2).ShouldBeEquivalentTo(_updateLink, options =>
                    options.Excluding(o => o.DateCreated));
            }
        }

        [Fact]
        public void UpdateSingle_Throws()
        {
            string name = "Update_Single_Throws";
            PopulateDB(name);
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                Action act_wrongId = () => service.UpdateOne(_updateLink_wrongId);
                act_wrongId.ShouldThrow<IdNotFoundException>();
                Action act_missingUrl = () => service.UpdateOne(_updateLink_missingUrl);
                act_missingUrl.ShouldThrow<PropertyIsMissingException>();
            }
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                service.GetAll().Count.Should().Be(_data.Count);
            }
        }

        [Fact]
        public void CreateSingleLink()
        {
            string name = "Create_Single_Link";
            PopulateDB(name);

            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                var link = service.CreateOne(_newLink);
                link.ShouldBeEquivalentTo(_newLink, options =>
                    options.Excluding(o => o.DateCreated).Excluding(o => o.Id));
            }
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                service.GetAll().Count.Should().Be(_data.Count + 1);
                service.GetSingle(4).ShouldBeEquivalentTo(_updateLink, options =>
                    options.Excluding(o => o.DateCreated).Excluding(o => o.Id));
            }
        }
    }
}