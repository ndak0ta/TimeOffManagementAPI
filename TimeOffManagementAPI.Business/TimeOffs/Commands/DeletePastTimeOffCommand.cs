using MediatR;
using TimeOffManagementAPI.Data.Access.Interfaces;

namespace TimeOffManagementAPI.Business.TimeOffs.Commands;

public record DeletePastTimeOffCommand : IRequest;

public class DeletePastTimeOffCommandHandler : IRequestHandler<DeletePastTimeOffCommand>
{
    private readonly ITimeOffRepository _timeOffRepository;

    public DeletePastTimeOffCommandHandler(ITimeOffRepository timeOffRepository)
    {
        _timeOffRepository = timeOffRepository;
    }

    public async Task Handle(DeletePastTimeOffCommand request, CancellationToken cancellationToken)
    {
        var timeOffs = await _timeOffRepository.GetAllAsync();

        foreach (var timeOff in timeOffs)
        {
            if (timeOff.EndDate < DateTime.Now)
            {
                await _timeOffRepository.DeleteAsync(timeOff.Id);
            }
        }
    }
}