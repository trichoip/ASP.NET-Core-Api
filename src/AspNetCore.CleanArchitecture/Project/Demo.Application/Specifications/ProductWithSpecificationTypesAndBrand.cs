using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Specifications
{
    public class ProductWithSpecificationTypesAndBrand : BaseSpecification<Player>
    {

        public ProductWithSpecificationTypesAndBrand([FromQuery] ProductSpecPrams productSpecPrams)
            : base
            (x =>
            (string.IsNullOrEmpty(productSpecPrams.Search) || x.Name == productSpecPrams.Search) &&
            (!productSpecPrams.brandId.HasValue || x.Id == productSpecPrams.brandId)
             && (!productSpecPrams.typeId.HasValue || x.Id == productSpecPrams.typeId))
        {
            AddInclude(Y => Y.Club);
            AddInclude(Y => Y.Club);
            AddInclude(Z => Z.Club);
            AddOrderBy(x => x.Name);
            ApplyPagging(productSpecPrams.pageSize * productSpecPrams.pageIndex, productSpecPrams.pageSize);
            if (string.IsNullOrEmpty(productSpecPrams.sort))
            {
                switch (productSpecPrams.sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Id);
                        break;
                    case "priceDesc":
                        AddOrderByDecending(p => p.Id);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }

        public ProductWithSpecificationTypesAndBrand(int Id)
            : base(x => x.Id == Id)
        {
            AddInclude(x => x.Club);
            AddInclude(x => x.Club);
        }
    }
}
