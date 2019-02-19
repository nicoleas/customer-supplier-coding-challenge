using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerSupplierCodingChallenge.Models
{
    class Supplier
    {

        public Supplier(double x, double y, int id)
        {
            this.x = x;
            this.y = y;

            this.id = id;
        }
        public int id { get; set; }

        public double x { get; set; }

        public double y { get; set; }

        public System.Windows.Media.SolidColorBrush colour { get; set; }
    }
}
