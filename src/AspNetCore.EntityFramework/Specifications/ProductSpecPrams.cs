using System.ComponentModel;

namespace AspNetCore.EntityFramework.Specifications
{
    public class ProductSpecPrams
    {
        private const int MaxPageSize = 50;
        public int pageIndex { get; set; } = 1;
        private int _pageSize = 6;
        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public int? categoryId { get; set; }

        [DefaultValue("averageRating,desc")]
        public string? sort { get; set; }
        public string _search;
        public string? Search
        {
            get => _search;
            set => _search = value.ToLower();
        }

    }
}