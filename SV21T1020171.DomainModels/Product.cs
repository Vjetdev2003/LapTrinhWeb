using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020171.DomainModels
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public double Price { get; set; }
    }
}
