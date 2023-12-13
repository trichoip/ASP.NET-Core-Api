using System.Collections.Generic;

#nullable disable

namespace AspNetCore.Client.MVC.Models;

public partial class Brand
{
    public Brand()
    {
        Models = new HashSet<Model>();
    }

    public long Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Model> Models { get; set; }
}
