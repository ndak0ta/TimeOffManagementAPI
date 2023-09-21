using MediatR;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;

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
        IEnumerable<TimeOff> timeOffs = await _timeOffRepository.GetAllAsync();

        foreach (TimeOff timeOff in timeOffs)
        {
            if (timeOff.EndDate < DateTime.Now)
            {
                await _timeOffRepository.DeleteAsync(timeOff.Id);
            }
        }
    }
}