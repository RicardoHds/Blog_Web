using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Models;

namespace Blog.ViewModels
{
    public class PostVM
    {
        public Post post { get; set; }
        // public Category category { get; set; }
        public List<Category> categories { get; set; }
        public Comment comment { get; set; }
        public List<Comment> comments { get; set; }
    }
}