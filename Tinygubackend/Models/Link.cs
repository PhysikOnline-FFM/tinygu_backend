using System;

namespace Tinygubackend.Models
{
  public class Link
  {
    public int Id { get; set; }
    public string LongUrl { get; set; }
    public string ShortUrl { get; set; }
    public DateTime Timestamp { get; set; }
    public User Owner { get; set; }
  }
}
