
using TimeOffManagementAPI.Business.TimeOffs.Commands;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Constants;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.UnitTests.TimeOffTests;

public class ApproveTimeOffCancelTests
{
    [Fact]
    public async Task ApproveTimeOffCancelCommandHandler_WhenTimeOffIsNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var mapperMock = new Mock<IMapper>();
        var command = new ApproveTimeOffCancelCommand(1, true);
        var handler = new ApproveTimeOffCancelCommandHandler(timeOffRepositoryMock.Object, mapperMock.Object);

        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TimeOff)null!);

        // Act
        async Task act() => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exc = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal("Time off not found", exc.Message);
    }

    [Fact]
    public async Task ApproveTimeOffCancelCommandHandler_WhenTimeOffStatusIsCancelled_ThrowsUnprocessableEntityException()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var mapperMock = new Mock<IMapper>();
        var command = new ApproveTimeOffCancelCommand(1, true);
        var handler = new ApproveTimeOffCancelCommandHandler(timeOffRepositoryMock.Object, mapperMock.Object);

        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new TimeOff
        {
            Status = TimeOffStates.Cancelled
        });

        // Act
        async Task act() => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exc = await Assert.ThrowsAsync<UnprocessableEntityException>(act);
        Assert.Equal("You can only approve a cancel request", exc.Message);
    }

    [Fact]
    public async Task ApproveTimeOffCancelCommandHandler_WhenTimeOffStatusIsNotCancelled_ReturnsTimeOffInfo()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var mapperMock = new Mock<IMapper>();
        var command = new ApproveTimeOffCancelCommand(1, true);
        var handler = new ApproveTimeOffCancelCommandHandler(timeOffRepositoryMock.Object, mapperMock.Object);

        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new TimeOff
        {
            Status = TimeOffStates.Approved
        });

        string response = "";

        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>())).Callback<TimeOff>(x => response = x.Status!);
        mapperMock.Setup(x => x.Map<TimeOffInfo>(It.IsAny<TimeOff>())).Returns(new TimeOffInfo());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<TimeOffInfo>(result);
        Assert.Equal(TimeOffStates.Cancelled, response);
    }

    [Fact]
    public async Task ApproveTimeOffCancelCommandHandler_WhenTimeOffStatusIsNotCancelledAndIsApprovedIsFalse_ReturnsTimeOffInfo()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var mapperMock = new Mock<IMapper>();
        var command = new ApproveTimeOffCancelCommand(1, false);
        var handler = new ApproveTimeOffCancelCommandHandler(timeOffRepositoryMock.Object, mapperMock.Object);

        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new TimeOff
        {
            Status = TimeOffStates.Approved
        });

        string response = "";

        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>())).Callback<TimeOff>(x => response = x.Status!);

        mapperMock.Setup(x => x.Map<TimeOffInfo>(It.IsAny<TimeOff>())).Returns(new TimeOffInfo());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<TimeOffInfo>(result);
        Assert.Equal(TimeOffStates.CancelRejected, response);
    }
}
