using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApI.Models
{
    public class Chart
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<Order> Orders { get; set; }

        public decimal Total { get; set; }
    }
}
