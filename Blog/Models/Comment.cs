using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int Id_Post { get; set; }
        public string Name { get; set; }
        public string Review { get; set; }
    }
}