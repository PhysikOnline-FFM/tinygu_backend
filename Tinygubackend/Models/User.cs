using System;
using System.Collections.Generic;
#pragma warning disable 1591
namespace Tinygubackend.Models
{
  public class User : Base
  {
    public string Name { get; set; }
    public List<Link> Links { get; set; }
  }
}
