namespace AspNetCore.Mapper.ViewModel
{
	public class ProductDTO
	{
		public int ProductID { get; set; }
		public string? ItemName { get; set; }
		public string? OptionalName { get; set; }
		public string? Moon { get; set; }
		public string? Igone { get; set; }
		public int ItemQuantity { get; set; }
		public int Amount { get; set; }

		public string CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
	}
}
