using AutoMapper;
using MediatR;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Business.TimeOffs.Queries;

public record GetTimeOffByIdQuery : IRequest<TimeOffInfo>
{
    public GetTimeOffByIdQuery(int id)
    {
        Id = id;
    }

    public int Id { get; init; }
}

public class GetTimeOffByIdQueryHandler : IRequestHandler<GetTimeOffByIdQuery, TimeOffInfo>
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IMapper _mapper;

    public GetTimeOffByIdQueryHandler(ITimeOffRepository timeOffRepository, IMapper mapper)
    {
        _timeOffRepository = timeOffRepository;
        _mapper = mapper;
    }

    public async Task<TimeOffInfo> Handle(GetTimeOffByIdQuery request, CancellationToken cancellationToken)
    {
        var timeOff = await _timeOffRepository.GetByIdAsync(request.Id);

        return _mapper.Map<TimeOffInfo>(timeOff);
    }
}