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
                Console.WriteLine(context.Links.Count());
                Action act = () => service.GetSingle(context.Links.Last().Id + 1);
                act.ShouldThrow<IdNotFoundException>();
            }
        }

        [Fact]
        public void UpdateSingleLink()
        {
            string name = "Update_Single_Link";
            PopulateDB(name);

            Link updateLink = new Link
            {
                LongUrl = "test",
                ShortUrl = "test",
                Owner = null,
            };
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                updateLink.Id = context.Links.Last().Id;
                var link = service.UpdateOne(updateLink);
                link.ShouldBeEquivalentTo(updateLink, options =>
                    options.Excluding(o => o.DateCreated));
            }
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                service.GetSingle(updateLink.Id).ShouldBeEquivalentTo(updateLink, options =>
                    options.Excluding(o => o.DateCreated));
            }
        }

        [Fact]
        public void UpdateSingle_Throws()
        {
            string name = "Update_Single_Throws";
            PopulateDB(name);
            Link updateLink = new Link
            {
                LongUrl = "test",
                ShortUrl = "test",
                Owner = null,
            };
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                updateLink.Id = context.Links.Last().Id + 1;
                Action act_wrongId = () => service.UpdateOne(updateLink);
                act_wrongId.ShouldThrow<IdNotFoundException>();
                updateLink.Id = context.Links.Last().Id;
                updateLink.ShortUrl = null;
                Action act_missingUrl = () => service.UpdateOne(updateLink);
                act_missingUrl.ShouldThrow<PropertyIsMissingException>();
            }
        }

        [Fact]
        public void CreateSingleLink()
        {
            string name = "Create_Single_Link";
            PopulateDB(name);

            Link newLink = new Link
            {
                LongUrl = "test",
                ShortUrl = "test",            
                Owner = null,
            };
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                var link = service.CreateOne(newLink);
                link.ShouldBeEquivalentTo(newLink, options =>
                    options.Excluding(o => o.DateCreated).Excluding(o => o.Id));
            }
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                service.GetAll().Count.Should().Be(_data.Count + 1);
                service.GetSingle(context.Links.Last().Id).ShouldBeEquivalentTo(newLink, options =>
                    options.Excluding(o => o.DateCreated).Excluding(o => o.Id));
            }
        }

        [Fact]
        public void CreateSingle_Throws()
        {
            string name = "Create_Single_Throws";
            PopulateDB(name);

            Link newLink = new Link
            {
                LongUrl = "test",
                ShortUrl = "test",            
                Owner = null,
            };
            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                newLink.LongUrl = null;
                Action act = () => service.CreateOne(newLink);
                act.ShouldThrow<PropertyIsMissingException>();
            }
        }
        [Fact]
        public void DeleteSingleLink()
        {
            string name = "Delete_Single_Link";
            PopulateDB(name);

            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                service.DeleteOne(context.Links.Last().Id);
            }

            using (var context = GetContext(name))
            {
                context.Links.Count().Should().Be(_data.Count - 1);
            }
        }

        [Fact]
        public void DeleteSingle_Throws()
        {
            string name = "Delete_Single_Throws";
            PopulateDB(name);

            using (var context = GetContext(name))
            {
                var service = new LinksRepository(context);
                Action act = () => service.DeleteOne(context.Links.Last().Id + 1);
                act.ShouldThrow<IdNotFoundException>();
            }

            using (var context = GetContext(name))
            {
                context.Links.Count().Should().Be(_data.Count);
            }
        }
    }
}