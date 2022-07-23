using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Queries.ProductImage.GetProductImages
{
    public class GetProductImagesQueryRequest: IRequest<List<GetProductImagesQueryResponse>>
    {
        public string Id { get; set; }
    }
}
