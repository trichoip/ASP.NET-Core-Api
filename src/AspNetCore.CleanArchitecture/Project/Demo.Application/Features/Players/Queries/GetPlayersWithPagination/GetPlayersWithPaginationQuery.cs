using AspNetCore.CleanArchitecture.Project.Demo.Application.Extensions;
using AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Repositories;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;
using AspNetCore.CleanArchitecture.Project.Demo.Shared;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Features.Players.Queries.GetPlayersWithPagination;

public record GetPlayersWithPaginationQuery : IRequest<PaginatedResult<GetPlayersWithPaginationDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetPlayersWithPaginationQuery() { }

    public GetPlayersWithPaginationQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

internal class GetPlayersWithPaginationQueryHandler : IRequestHandler<GetPlayersWithPaginationQuery, PaginatedResult<GetPlayersWithPaginationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPlayersWithPaginationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<GetPlayersWithPaginationDto>> Handle(GetPlayersWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<Player>().Entities
               .OrderBy(x => x.Name)
               .ProjectTo<GetPlayersWithPaginationDto>(_mapper.ConfigurationProvider)
               .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
}
