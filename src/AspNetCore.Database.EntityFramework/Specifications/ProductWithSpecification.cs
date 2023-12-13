using AspNetCore.Database.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Database.EntityFramework.Specifications;
public class ProductWithSpecification : BaseSpecification<Weapon>
{
    public ProductWithSpecification(ProductSpecPrams productSpecPrams) : base
        (x =>
           (string.IsNullOrEmpty(productSpecPrams.Search) || x.Name.Contains(productSpecPrams.Search)) &&
           (!productSpecPrams.categoryId.HasValue || x.CharacterId == productSpecPrams.categoryId))
    {
        AddInclude(Y => Y.Include(c => c.Nemo));

        if (!string.IsNullOrEmpty(productSpecPrams.sort))
        {
            switch (productSpecPrams.sort)
            {
                case "name,asc":
                    AddOrderBy(p => p.Name);
                    break;
                case "name,desc":
                    AddOrderByDecending(p => p.Name);
                    break;
                default:
                    AddOrderBy(n => n.Id);
                    break;
            }
        }
    }
}
