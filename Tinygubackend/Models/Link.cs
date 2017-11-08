namespace Tinygubackend.Models
{
    public class Link : Base
    {
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
        public User Owner { get; set; }
    }
}