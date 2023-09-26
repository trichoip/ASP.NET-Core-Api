using System;
using System.Collections.Generic;

#nullable disable

namespace AspNetCore.MVC.Models
{
    public partial class Model
    {
        public Model()
        {
            Cars = new HashSet<Car>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long BrandId { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
