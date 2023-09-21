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
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        timeOffRepositoryMock.Setup(x => x.SoftDeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(new TimeOff());

        DeleteTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object);

        // Act
        bool result = await handler.Handle(new DeleteTimeOffCommand(1), CancellationToken.None);

        // Assert
        Assert.True(result);
    }
}