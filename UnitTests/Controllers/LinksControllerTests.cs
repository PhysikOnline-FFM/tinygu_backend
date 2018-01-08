using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tinygubackend.Controllers;
using Tinygubackend.Core.Exceptions;
using Tinygubackend.Infrastructure;
using Tinygubackend.Models;
using Xunit;

namespace UnitTests.Controllers
{
    public class LinksControllerTests
    {
        private ILinksRepository _linksRepository;

        private readonly List<Link> _mockLinks = new List<Link>
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
            }
        };

        public LinksControllerTests()
        {
            var mockRepo = new Mock<ILinksRepository>();
            mockRepo.Setup(r => r.GetAll()).Returns(_mockLinks);
            mockRepo.Setup(r => r.GetSingle(1)).Returns(_mockLinks[0]);
            mockRepo.Setup(r => r.GetSingle(2)).Throws<IdNotFoundException>();

            mockRepo.Setup(r => r.CreateOne(_mockLinks[0])).Returns(_mockLinks[0]);
            mockRepo.Setup(r => r.CreateOne(_mockLinks[1])).Throws<DuplicateEntryException>();
            mockRepo.Setup(r => r.CreateOne(null)).Throws<Exception>();

            mockRepo.Setup(r => r.UpdateOne(_mockLinks[0])).Returns(_mockLinks[0]);
            mockRepo.Setup(r => r.UpdateOne(_mockLinks[1])).Throws<IdNotFoundException>();
            mockRepo.Setup(r => r.UpdateOne(null)).Throws<PropertyIsMissingException>();

            mockRepo.Setup(r => r.DeleteOne(1));
            mockRepo.Setup(r => r.DeleteOne(2)).Throws<IdNotFoundException>();

            _linksRepository = mockRepo.Object;
        }

        [Fact]
        public void GetAll()
        {
            var controller = new LinksController(_linksRepository);
            controller.GetAll().Should().Equals(_mockLinks);
        }

        [Fact]
        public void OneLink()
        {
            var controller = new LinksController(_linksRepository);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.OneLink(1).Should().Equals(_mockLinks[0]);
            var result = controller.OneLink(2) as BadRequestObjectResult;
            Assert.NotNull(result);
        }
    }
}