using System.Collections.Generic;
using System.Linq;
using Tinygubackend.Contexts;
using Tinygubackend.Core.Exceptions;
using Tinygubackend.Models;

namespace Tinygubackend.Infrastructure
{
    public interface ILinksRepository
    {
        List<Link> GetAll();
        Link GetSingle(int id);
        Link UpdateOne(Link updatedLink);
        Link CreateOne(Link newLink);
        void DeleteOne(int id);
    }
    public class LinksRepository : ILinksRepository
    {
        private readonly TinyguContext _tinyguContext;

        public LinksRepository(TinyguContext tinyguContext)
        {
            _tinyguContext = tinyguContext;
        }

        /// <summary>
        /// Get all Links in DB.
        /// </summary>
        /// <returns>List of all Links.</returns>
        public List<Link> GetAll()
        {
            return _tinyguContext.Links.ToList();
        }

        /// <summary>
        /// Get one Link from DB.
        /// </summary>
        /// <param name="id">Id of the target Link.</param>
        /// <returns>Link</returns>
        public Link GetSingle(int id)
        {
            Link link = _tinyguContext.Links.SingleOrDefault(_ => _.Id == id);
            if (link == null)
            {
                throw new IdNotFoundException();
            }
            return link;
        }

        /// <summary>
        /// Updates a singe Link.
        /// </summary>
        /// <param name="updatedLink">Link to upgrade from.</param>
        /// <returns>Updated Link.</returns>
        public Link UpdateOne(Link updatedLink)
        {
            int id = updatedLink.Id;
            Link oldLink = GetSingle(id);
            if (oldLink == null)
            {
                throw new IdNotFoundException();
            }
            if (updatedLink.ShortUrl == null || updatedLink.LongUrl == null)
            {
                throw new PropertyIsMissingException();
            }
            oldLink.ShortUrl = updatedLink.ShortUrl;
            oldLink.LongUrl = updatedLink.LongUrl;
            oldLink.Owner = updatedLink.Owner;
            _tinyguContext.SaveChanges();
            return oldLink;
        }

        /// <summary>
        /// Creates a single Link.
        /// </summary>
        /// <param name="newLink">New Link.</param>
        /// <returns>Newly created Link.</returns>
        public Link CreateOne(Link newLink)
        {
            if (newLink.ShortUrl == null || newLink.LongUrl == null)
            {
                throw new PropertyIsMissingException();
            }
            _tinyguContext.Links.Add(newLink);
            _tinyguContext.SaveChanges();
            return newLink;
        }

        /// <summary>
        /// Delete a Link by Id.
        /// </summary>
        /// <param name="id">Id to delete.</param>
        public void DeleteOne(int id)
        {
            Link link = _tinyguContext.Links.SingleOrDefault(_ => _.Id == id);
            if (link == null)
            {
                throw new IdNotFoundException();
            }
            _tinyguContext.Links.Remove(link);
            _tinyguContext.SaveChanges();
        }
    }
}