using System.Text.Json.Serialization;

namespace AspNetCore.HttpClient.Record;

public class ODataResponse<T>
{
    [JsonPropertyName("value")]
    public IList<T> Value { get; set; }

    // Các thuộc tính khác có thể cần thiết
}