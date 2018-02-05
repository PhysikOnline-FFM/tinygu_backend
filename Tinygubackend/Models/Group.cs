using System.ComponentModel.DataAnnotations;

namespace Tinygubackend.Models
{
    public class Group : Base
    {
        [MaxLength(50)]
        public string Name { get; set; }
        public int OwnerId { get; set; }
        [MaxLength(50)]
        public string AccessControl { get; set; }
    }
}