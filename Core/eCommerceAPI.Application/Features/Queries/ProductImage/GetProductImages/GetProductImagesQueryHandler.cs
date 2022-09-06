using AutoMapper;
using eCommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Queries.ProductImage.GetProductImages
{
    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IConfiguration configuration;
        readonly IMapper _mapper;

        public GetProductImagesQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration, IMapper mapper)
        {
            _productReadRepository = productReadRepository;
            this.configuration = configuration;
            _mapper = mapper;
        }
        public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = await _productReadRepository.Table.Include(x => x.ProductImageFiles)
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.Id));

            return product?.ProductImageFiles.Select(p => new GetProductImagesQueryResponse
            {
                Path = $"{p.Path}",
                FileName = p.FileName,
                Id = p.Id,
                Showcase = p.Showcase
            }).ToList();
        }
    }
}
