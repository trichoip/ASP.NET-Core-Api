namespace AspNetCore.Mapper.Models
{
	public class Order
	{
		public int OrderNo { get; set; }
		public int NumberOfItems { get; set; }
		public int TotalAmount { get; set; }
		public Customer Customer { get; set; }
	}
}
