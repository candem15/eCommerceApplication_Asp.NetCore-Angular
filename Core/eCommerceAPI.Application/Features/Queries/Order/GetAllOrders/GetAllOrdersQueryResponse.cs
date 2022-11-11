using eCommerceAPI.Application.Dtos;

namespace eCommerceAPI.Application.Features.Queries.Order.GetAllOrders
{
    public class GetAllOrdersQueryResponse
    {
        public int TotalOrderCount { get; set; }
        public List<ListOrder> Orders { get; set; }
    }
}