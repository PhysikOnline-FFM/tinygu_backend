using System;
using System.Collections.Generic;
#pragma warning disable 1591
namespace Tinygubackend.Models
{
  public class User : Base
  {
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public List<Link> Links { get; set; }
    public DateTime DateLogin { get; set; }
  }
}
