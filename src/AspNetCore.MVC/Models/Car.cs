using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class Car
    {
        public Car()
        {
            Books = new HashSet<Book>();
            CarFeatures = new HashSet<CarFeature>();
            CarImages = new List<CarImage>();
        }

        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string Fuel { get; set; }
        public double? Latitude { get; set; }
        public string LicensePlates { get; set; }
        public double? Longitude { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public double? Price { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int? SaleMonth { get; set; }
        public int? SaleWeek { get; set; }
        public int? Seats { get; set; }
        public string Status { get; set; }
        public string Transmission { get; set; }
        public int? YearOfManufacture { get; set; }
        public long AccountSupplierId { get; set; }
        public long? ModelId { get; set; }

        public virtual Account AccountSupplier { get; set; }
        public virtual Address IdNavigation { get; set; }
        public virtual Model Model { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<CarFeature> CarFeatures { get; set; }
        public virtual List<CarImage> CarImages { get; set; }
    }
}
