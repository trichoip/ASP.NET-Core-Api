using Microsoft.AspNetCore.Authentication;
using System.Text.Json;

namespace AspNetCore.Identity.Helpers
{
    public class CustomDataSerializer<TData> : IDataSerializer<TData>
    {
        public static CustomDataSerializer<TData> Default { get; } = new CustomDataSerializer<TData>();

        public virtual byte[] Serialize(TData model)
        {
            using (var memory = new MemoryStream())
            {
                var jsonSerializer = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                JsonSerializer.Serialize(memory, model, jsonSerializer);
                return memory.ToArray();
            }
        }

        public virtual TData? Deserialize(byte[] data)
        {
            using (var memory = new MemoryStream(data))
            {
                var jsonSerializer = new JsonSerializerOptions
                {
                    // Add any other deserialization options you need
                };
                return JsonSerializer.Deserialize<TData>(memory, jsonSerializer);
            }
        }

    }
}
