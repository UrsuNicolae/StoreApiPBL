using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApI.DTOs
{
    public class CategoryDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }
    }
}
