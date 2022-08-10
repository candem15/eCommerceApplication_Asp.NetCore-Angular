using AutoMapper;
using eCommerceAPI.Application.Dtos;
using eCommerceAPI.Application.Dtos.User;
using eCommerceAPI.Application.Features.Commands.AppUser.CreateUser;
using eCommerceAPI.Application.Features.Commands.AppUser.FacebookLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.GoogleLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.LoginUser;
using eCommerceAPI.Application.Features.Commands.AppUser.VkLogin;
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
            CreateMap<AppUser, CreateUser>()
                .ReverseMap();
            CreateMap<CreateUser, CreateUserCommandRequest>()
                .ReverseMap();
            CreateMap<CreateUserCommandResponse, CreateUserResponse>()
                .ReverseMap();
            CreateMap<FacebookLoginCommandResponse, Token>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token.AccessToken))
                 .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Token.Expiration))
                 .ReverseMap();
            CreateMap<VkLoginCommandResponse, Token>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token.AccessToken))
                 .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Token.Expiration))
                 .ReverseMap();
            CreateMap<GoogleLoginCommandResponse, Token>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token.AccessToken))
                 .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Token.Expiration))
                 .ReverseMap();
            CreateMap<LoginUserCommandResponse, Token>()
                 .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token.AccessToken))
                 .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Token.Expiration))
                 .ReverseMap();

        }
    }
}
