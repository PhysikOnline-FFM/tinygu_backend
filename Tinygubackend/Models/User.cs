using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#pragma warning disable 1591
namespace Tinygubackend.Models
{
    public class User : Base
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        [MaxLength(127)]
        public string Email { get; set; }
        public List<Link> Links { get; set; }
        public DateTime DateLogin { get; set; }
        public List<Group> Groups { get; set; }
    }
}
