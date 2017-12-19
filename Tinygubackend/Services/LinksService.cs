using System.Collections.Generic;
using System.Linq;
using Tinygubackend.Contexts;
using Tinygubackend.Models;

namespace Tinygubackend.Services
{
    public interface ILinksService
    {
        List<Link> GetAll();
        Link GetSingle(int id);
        Link UpdateOne(int id, Link updatedLink);
        Link CreateOne(Link newLink);
        void DeleteOne(int id);
    }
    public class LinksService : ILinksService
    {
        private readonly TinyguContext _tinyguContext;

        public LinksService(TinyguContext tinyguContext)
        {
            _tinyguContext = tinyguContext;
        }

        public List<Link> GetAll()
        {
            return _tinyguContext.Links.ToList();
        }

        public Link GetSingle(int id)
        {
            Link link = _tinyguContext.Links.SingleOrDefault(_ => _.Id == id);
            if (link == null)
            {
                throw new KeyNotFoundException($"Could not find link with id {id}!");
            }
            return link;
        }

        public Link UpdateOne(int id, Link updatedLink)
        {
            Link oldLink = GetSingle(id);
            if (oldLink == null)
            {
                throw new KeyNotFoundException($"Could not find link with id {id}!");
            }
            oldLink.ShortUrl = updatedLink.ShortUrl;
            oldLink.LongUrl = updatedLink.LongUrl;
            oldLink.Owner = updatedLink.Owner;
            _tinyguContext.SaveChanges();
            return oldLink;
        }

        public Link CreateOne(Link newLink)
        {
            _tinyguContext.Links.Add(newLink);
            _tinyguContext.SaveChanges();
            return newLink;
        }

        public void DeleteOne(int id)
        {
            Link link = _tinyguContext.Links.SingleOrDefault(_ => _.Id == id);
            if (link == null)
            {
                throw new KeyNotFoundException($"Could not find link with id {id}!");
            }
            _tinyguContext.Links.Remove(link);
            _tinyguContext.SaveChanges();
        }
    }
}