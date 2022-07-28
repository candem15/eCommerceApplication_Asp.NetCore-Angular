using AutoMapper;
using eCommerceAPI.Application.Features.Commands.AppUser.CreateUser;
using eCommerceAPI.Application.Features.Queries.Product.GetAllProducts;
using eCommerceAPI.Application.Features.Queries.Product.GetProductById;
using eCommerceAPI.Application.Features.Queries.ProductImage.GetProductImages;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Entities.Identity;

namespace eCommerceAPI.Application.Services.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Product, GetProductByIdQueryResponse>()
                .ReverseMap();
            CreateMap<Product, GetAllProductsQueryResponse>()
                .ReverseMap();
            CreateMap<AppUser, CreateUserCommandRequest>()
                .ReverseMap();

        }
    }
}
