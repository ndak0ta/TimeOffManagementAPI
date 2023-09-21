using TimeOffManagementAPI.Business.TimeOffs.Commands;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Constants;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.UnitTests.TimeOffTests;

public class DeletePastTimeOffTests
{
    [Fact]
    public async Task DeletePastTimeOffCommandHandler_WhenTimeOffIsNotFound_DeletesTimeOffs()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        List<TimeOff> timeOffs = new()
        {
            new() {
                Id = 1,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(-1),
                Status = TimeOffStates.Approved
            },
            new() {
                Id = 2,
                StartDate = DateTime.Now.AddDays(-2),
                EndDate = DateTime.Now.AddDays(-2),
                Status = TimeOffStates.Approved
            },
        };

        DeletePastTimeOffCommand command = new();
        DeletePastTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object);

        timeOffRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(timeOffs);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        timeOffRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Exactly(timeOffs.Count));
    }
}