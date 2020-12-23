using System.Collections.Generic;

namespace ApI.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}