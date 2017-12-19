using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tinygubackend.Contexts;
using Tinygubackend.Models;
using Tinygubackend.Services;
using Xunit;

namespace UnitTests.Services
{
    public class LinksServiceTests
    {
        private List<Link> _data = new List<Link>
        {
            new Link
            {
                LongUrl = "google.com",
                ShortUrl = "g",
                Owner = null,
                Id = 1,
                DateCreated = DateTime.Parse("2017-11-08T12:07:55.323428")
            },
            new Link
            {
                LongUrl = "google.com2",
                ShortUrl = "g2",
                Owner = null,
                Id = 2,
                DateCreated = DateTime.Parse("2017-11-08T12:07:55.323428")
            },
            new Link
            {
                LongUrl = "google.com3",
                ShortUrl = "g3",
                Owner = null,
                Id = 3,
                DateCreated = DateTime.Parse("2017-11-08T12:07:55.323428")
            },
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
                var service = new LinksService(context);
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
                var service = new LinksService(context);
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
                var service = new LinksService(context);
                Action act = () => service.GetSingle(_data[_data.Count - 1].Id + 1);
                act.ShouldThrow<KeyNotFoundException>();
            }
        }

        [Fact]
        public void UpdateSingleLink()
        {
            string name = "Update_Single_Link";
            PopulateDB(name);
                Link newLink = new Link
                {
                    LongUrl = "test",
                    ShortUrl = "test",
                    Owner = null,
                    Id = 2,
                    DateCreated = DateTime.Parse("2017-11-08T12:07:55.323428")
                };
            using (var context = GetContext(name))
            {
                var service = new LinksService(context);
                var link = service.UpdateOne(2, newLink);
                link.ShouldBeEquivalentTo(newLink, options =>
                    options.Excluding(o => o.DateCreated));
            }
            using (var context = GetContext(name))
            {
                var service = new LinksService(context);
                service.GetSingle(2).ShouldBeEquivalentTo(newLink, options =>
                    options.Excluding(o => o.DateCreated));
            }
        }
    }
}