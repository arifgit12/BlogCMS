using System;
using System.Collections.Generic;

namespace BlogCMS.Models
{
    public class Post
    {
        public Post()
        {
            Tags = new HashSet<Tag>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool isPublished { get; set; }
        public DateTime Published { get; set; }

        public string PublisherId { get; set; }
        public virtual ApplicationUser Publisher { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}