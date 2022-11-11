namespace eCommerceAPI.Application.Dtos
{
    public class ListOrder
    {
        public string OrderCode { get; set; }
        public string UserName { get; set; }
        public float TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalOrderCount { get; set; }
    }
}