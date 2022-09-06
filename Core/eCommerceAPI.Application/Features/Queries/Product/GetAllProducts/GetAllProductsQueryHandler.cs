using AutoMapper;
using eCommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IMapper _mapper;
        public GetAllProductsQueryHandler(IProductReadRepository productReadRepository, IMapper mapper, IProductImageFileReadRepository productImageFileReadRepository)
        {
            _productReadRepository = productReadRepository;
            _mapper = mapper;
            _productImageFileReadRepository = productImageFileReadRepository;
        }
        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var totalProductsCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate,
                p.ProductImageFiles,
                ShowcaseImagePath = _productImageFileReadRepository.Table.Include(x => x.Products).SelectMany(x => x.Products, (pif, pro) => new { pif, pro }).FirstOrDefault(x => x.pro.Id == p.Id && x.pif.Showcase == true).pif.Path
            }).ToList();

            return new()
            {
                Products = products,
                TotalProductsCount = totalProductsCount
            };
        }
    }
}
