using MediatR;
using TimeOffManagementAPI.Data.Access.Interfaces;

namespace TimeOffManagementAPI.Business.TimeOffs.Commands;

public class DeleteTimeOffCommand : IRequest<bool>
{
    public DeleteTimeOffCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

public class DeleteTimeOffCommandHandler : IRequestHandler<DeleteTimeOffCommand, bool>
{
    private readonly ITimeOffRepository _timeOffRepository;

    public DeleteTimeOffCommandHandler(ITimeOffRepository timeOffRepository)
    {
        _timeOffRepository = timeOffRepository;
    }

    public async Task<bool> Handle(DeleteTimeOffCommand request, CancellationToken cancellationToken)
    {
        await _timeOffRepository.SoftDeleteAsync(request.Id);

        return true;
    }
}