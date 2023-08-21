using AutoMapper;
using MediatR;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Business.TimeOffs.Queries;

public record GetAllTimeOffsQuery : IRequest<IEnumerable<TimeOffInfo>>;

public class GetAllTimeOffsQueryHandler : IRequestHandler<GetAllTimeOffsQuery, IEnumerable<TimeOffInfo>>
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IMapper _mapper;

    public GetAllTimeOffsQueryHandler(ITimeOffRepository timeOffRepository, IMapper mapper)
    {
        _timeOffRepository = timeOffRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TimeOffInfo>> Handle(GetAllTimeOffsQuery request, CancellationToken cancellationToken)
    {
        var timeOffs = await _timeOffRepository.GetAllAsync();

        var timeOffInfos = new List<TimeOffInfo>();

        foreach (var timeOff in timeOffs)
        {
            var timeOffInfo = _mapper.Map<TimeOffInfo>(timeOff);

            timeOffInfos.Add(timeOffInfo);
        }

        return timeOffInfos;
    }
}
