using AutoMapper;
using eCommerceAPI.Application.Abstractions.Services;
using MediatR;

namespace eCommerceAPI.Application.Features.Queries.Order.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
    {
        readonly IOrderService _orderService;
        readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var datas = await _orderService.GetAllOrdersAsync(request.Page, request.Size);
            return _mapper.Map<GetAllOrdersQueryResponse>(datas);
        }
    }
}