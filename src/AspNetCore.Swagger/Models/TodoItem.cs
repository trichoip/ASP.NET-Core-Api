using System.ComponentModel;

namespace AspNetCore.Swagger.Models
{
    public class TodoItem
    {
        // cách cấu hình example khác là IExamplesProvider, xem class TodoItemExample

        /// <summary>
        /// Quantity left in stock
        /// </summary>
        /// <example>10</example>
        public long Id { get; set; }

        [DefaultValue("name 1")]
        public string? Name { get; set; }
        public bool IsComplete { get; set; }

        /// <summary>
        /// The sizes the product is available in
        /// </summary>
        /// <example>["Small", "Medium", "Large"]</example>
        public List<string> Sizes { get; set; }

        public string[] Cars { get; set; }

        public Model Model { get; set; }
    }

    public enum Model
    {
        Small,
        Medium,
        Large
    }
}
