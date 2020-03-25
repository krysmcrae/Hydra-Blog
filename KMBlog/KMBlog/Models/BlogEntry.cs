using KMBlog.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMBlog.Models
{
    public class BlogEntry
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string Slug { get; set; }
        public string Abstract { get; set; }
        public string AbstractImageURL { get; set; }
        public PostType PostType { get; set; }
        public string Body1 { get; set; }
        public string Body2 { get; set; }
        public string Body3 { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string MediaURL { get; set; }
        public bool Published { get; set; }


        //Navigational Properties
        public virtual ICollection<EntryComment> Comments { get; set;}


        //Constructor
        public BlogEntry()
        {
            Comments = new HashSet<EntryComment>();
        }

    }
}