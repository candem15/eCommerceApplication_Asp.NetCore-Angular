using eCommerceAPI.Application.Dtos;

namespace eCommerceAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrder createOrder);
        Task<List<ListOrder>> GetAllOrdersAsync(int page, int size);
    }
}
