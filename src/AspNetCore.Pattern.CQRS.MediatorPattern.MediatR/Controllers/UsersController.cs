using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Commands.CreateUser;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Commands.UpdateUser;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Queries.GetUserById;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Queries.GetUsers;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery();

        var users = await _sender.Send(query, cancellationToken);

        return Ok(users);
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserById(int userId, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(userId);

        var user = await _sender.Send(query, cancellationToken);

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateUserCommand>();

        var user = await _sender.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, user);
    }

    [HttpPut("{userId:int}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateUserCommand>() with
        {
            UserId = userId
        };

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}
