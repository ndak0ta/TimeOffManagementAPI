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
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMapper> mapperMock = new();

        DrawCancelRequestCommand command = new(1, "1");
        DrawCancelRequestCommandHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object);

        TimeOff timeOff = new()
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
        TimeOffInfo result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<TimeOffInfo>(result);
    }

    [Fact]
    public async Task DrawCancelRequest_WhenStatusNotCancelRequested_ReturnsUnprocessableEntityException()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMapper> mapperMock = new();

        DrawCancelRequestCommand command = new(1, "1");
        DrawCancelRequestCommandHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object);

        TimeOff timeOff = new()
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
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMapper> mapperMock = new();

        DrawCancelRequestCommand command = new(1, "otherUserId");
        DrawCancelRequestCommandHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object);

        TimeOff timeOff = new()
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