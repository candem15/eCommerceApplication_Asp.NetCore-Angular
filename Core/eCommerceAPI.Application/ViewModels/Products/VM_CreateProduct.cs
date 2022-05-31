using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.ViewModels.Products
{
    public class VM_CreateProduct
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
    }
}
