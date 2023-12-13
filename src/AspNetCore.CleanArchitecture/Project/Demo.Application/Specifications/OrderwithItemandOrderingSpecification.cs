using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Specifications;

public class OrderwithItemandOrderingSpecification : BaseSpecification<Club>
{
    public OrderwithItemandOrderingSpecification(string Email) :
        base(x => x.Name == Email)
    {
        AddInclude(o => o.Players);
        AddInclude(d => d.Players);
        AddOrderByDecending(od => od.Id);
    }

    public OrderwithItemandOrderingSpecification(int id, string emial)
        : base(o => o.Id == id && o.Name == emial)
    {

    }
}
