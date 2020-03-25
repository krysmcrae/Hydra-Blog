using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMBlog.Models
{
    public class EntryComment
    {
        public int Id{ get; set; }
        public int BlogEntryId { get; set; }
        public string AuthorId { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateReason { get; set; }

        //Navigational
        public BlogEntry BlogEntry { get; set; }
        public ApplicationUser Author { get; set; }


    }
}