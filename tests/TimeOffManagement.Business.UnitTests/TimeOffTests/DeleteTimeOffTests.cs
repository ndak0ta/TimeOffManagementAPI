using TimeOffManagementAPI.Business.TimeOffs.Commands;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.UnitTests.TimeOffTests;

public class DeleteTimeOffTests
{
    [Fact]
    public async Task DeleteTimeOffCommandHandler_ShouldDeleteTimeOff()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        timeOffRepositoryMock.Setup(x => x.SoftDeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(new TimeOff());

        var handler = new DeleteTimeOffCommandHandler(timeOffRepositoryMock.Object);

        // Act
        var result = await handler.Handle(new DeleteTimeOffCommand(1), CancellationToken.None);

        // Assert
        Assert.True(result);
    }
}