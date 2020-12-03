using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApI.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Cart { get; set; }

        public decimal Total { get; set; }
    }
}
