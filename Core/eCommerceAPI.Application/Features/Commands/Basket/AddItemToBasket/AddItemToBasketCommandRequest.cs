using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.Basket
{
    public class AddItemToBasketCommandRequest:IRequest<AddItemToBasketCommandResponse>
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
