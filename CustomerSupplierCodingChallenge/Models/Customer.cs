using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerSupplierCodingChallenge.Models
{
    class Customer
    {
        public Customer (double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public double x { get; set; }

        public double y { get; set; }

        public Supplier supplier { get; set; }

    }
}
