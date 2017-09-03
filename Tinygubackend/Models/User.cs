using System.Collections.Generic;

namespace Tinygubackend.Models
{
  public class User
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Link> Links { get; set; }
  }
}
