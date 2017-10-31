using System;
#pragma warning disable 1591
namespace Tinygubackend.Models
{
  public class Link : Base
  {
    public string LongUrl { get; set; }
    public string ShortUrl { get; set; }
    public User Owner { get; set; }
  }
}
