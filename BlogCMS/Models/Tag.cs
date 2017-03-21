using System;

namespace BlogCMS.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}