using System;
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

    public interface IRandomGenerator
    {
        int Next(int a, int b);
    }
    public class LinksRepository : ILinksRepository
    {
        private readonly TinyguContext _tinyguContext;
        private readonly IRandomGenerator _random;

        private class DefaultRandom : IRandomGenerator
        {
            public int Next(int a, int b) 
            {
                return (new Random()).Next(a, b);
            }
        }

        public LinksRepository(TinyguContext tinyguContext, IRandomGenerator random = null)
        {
            _tinyguContext = tinyguContext;
            _random = random?? new DefaultRandom();
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
            if (string.IsNullOrEmpty(newLink.LongUrl))
            {
                throw new PropertyIsMissingException();
            }
            if (DoesShortUrlAlreadyExists(newLink.ShortUrl))
            {
                throw new DuplicateEntryException();
            }
            if (string.IsNullOrEmpty(newLink.ShortUrl))
            {
                newLink.ShortUrl = GetRandomShortUrl();
            }
            _tinyguContext.Links.Add(newLink);
            _tinyguContext.SaveChanges();
            return newLink;
        }

        private bool DoesShortUrlAlreadyExists(string shortUrl)
        {
            return _tinyguContext.Links.SingleOrDefault(_ => _.ShortUrl == shortUrl) != null;
        }

        private string GetRandomShortUrl()
        {
            const string letters = "abcdefghkmnpqrstuvwxyz23456789";
            int length = 3;
            int numLetters = letters.Length;
            string result;
            do
            {
                result = "";
                for (int i = 0; i < length; i++)
                {
                    char c = letters[_random.Next(0, numLetters)];
                    result += c;
                }
                length++;
            } while (DoesShortUrlAlreadyExists(result));
            
            return result;
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