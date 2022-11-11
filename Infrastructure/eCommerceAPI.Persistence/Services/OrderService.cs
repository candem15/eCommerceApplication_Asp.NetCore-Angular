using AutoMapper;
using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Application.Dtos;
using eCommerceAPI.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Persistence.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IMapper _mapper;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, IMapper mapper)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _mapper = mapper;
        }

        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            await _orderWriteRepository.AddAsync(new()
            {
                Address = createOrder.Address,
                Id = Guid.Parse(createOrder.BasketId),
                Description = createOrder.Description,
                OrderCode = new Random().Next(10000000, 99999999).ToString()
            });
            await _orderWriteRepository.SaveChangesAsync();
        }

        public async Task<List<ListOrder>> GetAllOrdersAsync(int page, int size)
        {
            var query = _orderReadRepository.Table.Include(o => o.Basket)
                      .ThenInclude(b => b.User)
                      .Include(o => o.Basket)
                         .ThenInclude(b => b.BasketItems)
                         .ThenInclude(bi => bi.Product);

            var data = query.Skip(page * size).Take(size).ToList();
            
            return _mapper.Map<List<ListOrder>>(data);
        }
    }
}
