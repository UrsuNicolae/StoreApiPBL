using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApI.Helpers
{
    public class ProductParams
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        public int CategoryId { get; set; } = -1;

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
    }
}
