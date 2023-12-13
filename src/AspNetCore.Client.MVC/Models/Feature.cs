using System.Collections.Generic;

#nullable disable

namespace AspNetCore.Client.MVC.Models;

public partial class Feature
{
    public Feature()
    {
        CarFeatures = new HashSet<CarFeature>();
    }

    public long Id { get; set; }
    public string Icon { get; set; }
    public string Name { get; set; }

    public virtual ICollection<CarFeature> CarFeatures { get; set; }
}
