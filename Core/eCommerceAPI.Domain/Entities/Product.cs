using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerceAPI.Domain.Entities.Common;

namespace eCommerceAPI.Domain.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
