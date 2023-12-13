using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Specifications;

public class ProductWithFilterCountSpecification : BaseSpecification<Player>
{
    public ProductWithFilterCountSpecification(ProductSpecPrams productSpecPrams) :
        base
        (x =>
        (string.IsNullOrEmpty(productSpecPrams.Search) || x.Name == productSpecPrams.Search) &&
        (!productSpecPrams.brandId.HasValue || x.Id == productSpecPrams.brandId)
         && (!productSpecPrams.typeId.HasValue || x.Id == productSpecPrams.typeId))
    {

    }
}
