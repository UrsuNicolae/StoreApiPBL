using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApI.DTOs
{
    public class CreateOrderDto
    {
        
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int Quantity { get; set; } = 0;

        public int? ChartId { get; set; }
    }
}
