using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.RequestParameters.Pagination
{
    public record Pagination // Record tipi class' a göre daha az maliyetlidir.
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
