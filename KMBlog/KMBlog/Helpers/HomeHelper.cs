using KMBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMBlog.Helpers
{
    public class HomeHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public int[] BuildAHouse()
        {
            int[] size =new  int[] { 0,0,0};
             size[1]=db.Posts.Count();
            if (size[1] <= 3)
            {
                size[0] = 1;
            }
            else
            {
                size[0] = size[1] / 3 + 1;
            }
            return size;
        }
    }
}