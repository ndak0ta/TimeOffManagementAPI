using AutoMapper;
using MediatR;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.TimeOffs.Queries;

public record GetTimeOffByUserIdQuery : IRequest<IEnumerable<TimeOffInfo>>
{
    public GetTimeOffByUserIdQuery(string id)
    {
        Id = id;
    }

    public string Id { get; init; }
}

public class GetTimeOffByUserIdQueryHandler : IRequestHandler<GetTimeOffByUserIdQuery, IEnumerable<TimeOffInfo>>
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IMapper _mapper;

    public GetTimeOffByUserIdQueryHandler(ITimeOffRepository timeOffRepository, IMapper mapper)
    {
        _timeOffRepository = timeOffRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TimeOffInfo>> Handle(GetTimeOffByUserIdQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TimeOff> timeOffs = (await _timeOffRepository.GetByUserIdAsync(request.Id)).Where(t => t.IsActive);

        return _mapper.Map<IEnumerable<TimeOffInfo>>(timeOffs);
    }
}