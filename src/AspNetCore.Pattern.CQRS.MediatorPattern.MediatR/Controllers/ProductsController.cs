using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Commands.CreateProduct;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Commands.DeleteProduct;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Commands.UpdateProduct;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Queries.GetAllProducts;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAllProductsQuery(), cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetProductByIdQuery(id), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest productRequest, CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(new CreateProductCommand(productRequest), cancellationToken);

        //await _mediator.Publish(new ProductCreatedEvent(product), cancellationToken);

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteProductCommand { Id = id }, cancellationToken);
        return NoContent();
    }
}
