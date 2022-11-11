using AutoMapper;
using eCommerceAPI.Application.Dtos;
using eCommerceAPI.Application.Dtos.Twitter;
using eCommerceAPI.Application.Dtos.User;
using eCommerceAPI.Application.Features.Commands.AppUser.CreateUser;
using eCommerceAPI.Application.Features.Commands.AppUser.FacebookLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.GoogleLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.LoginUser;
using eCommerceAPI.Application.Features.Commands.AppUser.MicrosoftLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.RefreshTokenLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.TwitterLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.VkLogin;
using eCommerceAPI.Application.Features.Queries.Authentication.Twitter.GetRequestToken;
using eCommerceAPI.Application.Features.Queries.Order.GetAllOrders;
using eCommerceAPI.Application.Features.Queries.Product.GetAllProducts;
using eCommerceAPI.Application.Features.Queries.Product.GetProductById;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Entities.Identity;

namespace eCommerceAPI.Application.Services.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {

            //Product
            CreateMap<Product, GetProductByIdQueryResponse>()
                .ReverseMap();
            CreateMap<Product, GetAllProductsQueryResponse>()
                .ReverseMap();

            //User
            CreateMap<AppUser, CreateUserCommandRequest>()
                .ReverseMap();
            CreateMap<AppUser, CreateUser>()
                .ReverseMap();
            CreateMap<CreateUser, CreateUserCommandRequest>()
                .ReverseMap();
            CreateMap<CreateUserCommandResponse, CreateUserResponse>()
                .ReverseMap();

            //Token
            CreateMap<FacebookLoginCommandResponse, Token>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token.AccessToken))
                 .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Token.Expiration))
                 .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.Token.RefreshToken))
                 .ReverseMap();
            CreateMap<VkLoginCommandResponse, Token>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token.AccessToken))
                 .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Token.Expiration))
                 .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.Token.RefreshToken))
                 .ReverseMap();
            CreateMap<RefreshTokenLoginCommandResponse, Token>()
               .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token.AccessToken))
                .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Token.Expiration))
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.Token.RefreshToken))
                .ReverseMap();
            CreateMap<GoogleLoginCommandResponse, Token>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token.AccessToken))
                 .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Token.Expiration))
                 .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.Token.RefreshToken))
                 .ReverseMap();
            CreateMap<LoginUserCommandResponse, Token>()
                 .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token.AccessToken))
                 .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Token.Expiration))
                 .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.Token.RefreshToken))
                 .ReverseMap();
            CreateMap<MicrosoftLoginCommandResponse, Token>()
                 .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token.AccessToken))
                 .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Token.Expiration))
                 .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.Token.RefreshToken))
                 .ReverseMap();
            CreateMap<TwitterLoginCommandResponse, Token>()
                 .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token.AccessToken))
                 .ForMember(dest => dest.Expiration, opt => opt.MapFrom(src => src.Token.Expiration))
                 .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.Token.RefreshToken))
                 .ReverseMap();
            CreateMap<RequestTokenResponse, GetRequestTokenQueryResponse>()
                .ReverseMap();

            //Order
            CreateMap<Order, ListOrder>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Basket.User.UserName))
                .ForMember(dest => dest.OrderCode, opt => opt.MapFrom(src => src.OrderCode))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Basket.BasketItems.Sum(bi => bi.Quantity * bi.Product.Price)))
                .ForMember(dest => dest.TotalOrderCount, opt => opt.MapFrom(src => src.OrderCode.Count()));
            CreateMap<List<ListOrder>, GetAllOrdersQueryResponse>()
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Select(x => x)))
                .ForMember(dest => dest.TotalOrderCount, opt => opt.MapFrom(src => src.First().TotalOrderCount));

        }
    }
}
