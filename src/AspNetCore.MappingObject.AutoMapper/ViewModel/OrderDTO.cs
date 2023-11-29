namespace AspNetCore.MappingObject.AutoMapper.ViewModel
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int NumberOfItems { get; set; }
        public int TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Postcode { get; set; }
        public string MobileNo { get; set; }
    }
}
