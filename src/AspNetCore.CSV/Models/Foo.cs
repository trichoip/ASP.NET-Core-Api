namespace AspNetCore.CSV.Models
{
	public class Foo
	{
		// chức năng của index là chỉ định vị trí của cột trong file csv
		// nếu không có index thì sẽ lấy theo thứ tự của properties
		// nếu có index thì sẽ lấy theo thứ tự của index
		// chức năng name là chỉ định tên của cột trong file csv

		//[Index(0)]
		//[Name("id")]
		public int Id { get; set; }
		//[Index(2)]
		//[Name("licen")]
		public string Name { get; set; }
		//[Index(1)]
		//[Name("name")]
		public string Licen { get; set; }
	}
}
