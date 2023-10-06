namespace AspNetCore.EntityFramework.Enums;

// enum có giá trị là int , nó bắt đầu từ 0 nên có thể đổi giá trị int thành số khác như bên dưới 
// lưu ý là nếu nhập số khác ngoài các giá int enum như bên dưới thì nó sẽ không có báo lỗi,
// khi lưu lên database thì nó vẫn lưu giá trị đó vào database, như vậy sẽ không ràng buộc được giá trị enum,
// vì vậy cần check giá trị enum trước khi lưu vào database, để check thì xem trong class Character

// để hiển thị và post enum dưới dạng string thì thêm attribute [JsonConverter(typeof(JsonStringEnumConverter))] vào enum, này chỉ áp dụng cho enum này
// hoăc thêm  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); nó sẽ áp dụng cho tất cả enum
//[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    CANCEL = 11,
    SUCCESS = 7,
    EXPIRED = 9,
}
