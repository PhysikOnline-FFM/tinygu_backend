using System;
#pragma warning disable 1591
namespace Tinygubackend.Models
{
    public class Base
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}