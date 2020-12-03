using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApI.Models
{
    public class Page
    {
        public int Id { get; set; }

        public string  Name { get; set; }

        public string Slug { get; set; }

        public string Content { get; set; }
    }
}
