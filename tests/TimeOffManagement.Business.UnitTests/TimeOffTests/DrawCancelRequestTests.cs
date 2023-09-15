using TimeOffManagementAPI.Business.TimeOffs.Commands;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Constants;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.UnitTests.TimeOffTests;

public class DrawCancelRequestTests
{
    [Fact]
    public async Task DrawCancelRequest_WhenCalled_ReturnsTimeOffInfo()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var mapperMock = new Mock<IMapper>();

        var command = new DrawCancelRequestCommand(1, "1");
        var handler = new DrawCancelRequestCommandHandler(timeOffRepositoryMock.Object, mapperMock.Object);

        var timeOff = new TimeOff
        {
            Id = 1,
            UserId = "1",
            Status = TimeOffStates.CancelRequested
        };

        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(timeOff);

        mapperMock.Setup(x => x.Map<TimeOffInfo>(It.IsAny<TimeOff>()))
            .Returns(new TimeOffInfo());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<TimeOffInfo>(result);
    }

    [Fact]
    public async Task DrawCancelRequest_WhenStatusNotCancelRequested_ReturnsUnprocessableEntityException()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var mapperMock = new Mock<IMapper>();

        var command = new DrawCancelRequestCommand(1, "1");
        var handler = new DrawCancelRequestCommandHandler(timeOffRepositoryMock.Object, mapperMock.Object);

        var timeOff = new TimeOff
        {
            Id = 1,
            UserId = "2",
        };

        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(timeOff);

        // Act
        // Assert
        await Assert.ThrowsAsync<UnprocessableEntityException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task DrawCancelRequest_WhenUserIdNotMatch_ReturnsUnprocessableEntityException()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var mapperMock = new Mock<IMapper>();

        var command = new DrawCancelRequestCommand(1, "otherUserId");
        var handler = new DrawCancelRequestCommandHandler(timeOffRepositoryMock.Object, mapperMock.Object);

        var timeOff = new TimeOff
        {
            Id = 1,
            UserId = "1",
            Status = TimeOffStates.CancelRequested
        };

        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(timeOff);

        // Act
        // Assert
        await Assert.ThrowsAsync<UnprocessableEntityException>(() => handler.Handle(command, CancellationToken.None));
    }
}