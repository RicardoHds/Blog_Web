using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Post
    {
        // Assign post properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
    }
}
