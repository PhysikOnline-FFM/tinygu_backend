using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tinygubackend.Models
{
    public class Link : Base
    {
        [MaxLength(500)]
        public string LongUrl { get; set; }
        [MaxLength(50)]
        public string ShortUrl { get; set; }
        public User Owner { get; set; }
        public Group Group { get; set; }
    }
}