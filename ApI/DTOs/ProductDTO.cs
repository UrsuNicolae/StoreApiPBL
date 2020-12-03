using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApI.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public IFormFile ImageUpload { get; set; }

        public int CategoryId { get; set; }
    }
}
