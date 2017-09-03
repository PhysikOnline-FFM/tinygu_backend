using System;
using System.Collections.Generic;

namespace Tinygubackend.Models
{
  public class User : Base
  {
    public string Name { get; set; }
    public List<Link> Links { get; set; }
  }
}
