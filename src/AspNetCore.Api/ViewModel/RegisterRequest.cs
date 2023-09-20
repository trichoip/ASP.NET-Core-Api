namespace AspNetCore.Api.ViewModel
{
    // sealed: Đây là một từ khóa khác, nó cho biết rằng lớp RegisterRequest không thể được kế thừa bởi các lớp con khác. Lớp con không thể mở rộng hoặc thừa kế từ lớp RegisterRequest.
    // internal: Đây là một từ khóa truy cập(access modifier) cho biết lớp RegisterRequest chỉ có thể được truy cập từ trong cùng một assembly. Trong ngữ cảnh này, "assembly" thường tượng trưng cho một tệp DLL hoặc EXE, và lớp này chỉ có thể được sử dụng trong phạm vi của assembly đó.
    internal sealed class RegisterRequest
    {
        // required: Đây là một từ khóa khác, cho biết rằng khi tạo một đối tượng của lớp RegisterRequest, bạn phải cung cấp một giá trị cho thuộc tính Email.
        public required string Email { get; init; } // (init có thể thay đổi giá trị trong hàm ctor, nhưng sau đó không thể thay đổi nữa
        public required string Password { get; init; }
    }
}
