using TimeOffManagementAPI.Business.TimeOffs.Commands;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Constants;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.UnitTests.TimeOffTests;

public class CancelTimeOffRequestTests
{
    [Fact]
    public async Task CancelTimeOffRequest_WhenTimeOffIsNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((TimeOff)null!);

        var mediatorMock = new Mock<IMediator>();
        var mapperMock = new Mock<IMapper>();

        var handler = new CancelTimeOffRequestCommandHandler(timeOffRepositoryMock.Object, mediatorMock.Object, mapperMock.Object);

        // Act
        async Task act() => await handler.Handle(new CancelTimeOffRequestCommand(1, "userId"), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task CancelTimeOffRequest_WhenTimeOffIsNotApproved_ThrowsUnprocessableEntityException()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.Pending });

        var mediatorMock = new Mock<IMediator>();
        var mapperMock = new Mock<IMapper>();

        var handler = new CancelTimeOffRequestCommandHandler(timeOffRepositoryMock.Object, mediatorMock.Object, mapperMock.Object);

        // Act
        async Task act() => await handler.Handle(new CancelTimeOffRequestCommand(1, "userId"), CancellationToken.None);

        // Assert
        var exc = await Assert.ThrowsAsync<UnprocessableEntityException>(act);

        Assert.Equal("You can only cancel an approved time off", exc.Message);
    }

    [Fact]
    public async Task CancelTimeOffRequest_WhenTimeOffIsNotOwnedByUser_ThrowsUnprocessableEntityException()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.Approved, UserId = "otherUserId" });

        var mediatorMock = new Mock<IMediator>();
        var mapperMock = new Mock<IMapper>();

        var handler = new CancelTimeOffRequestCommandHandler(timeOffRepositoryMock.Object, mediatorMock.Object, mapperMock.Object);

        // Act
        async Task act() => await handler.Handle(new CancelTimeOffRequestCommand(1, "userId"), CancellationToken.None);

        // Assert
        var exc = await Assert.ThrowsAsync<UnprocessableEntityException>(act);

        Assert.Equal("You can only send a cancel request for your own time off", exc.Message);
    }

    [Fact]
    public async Task CancelTimeOffRequest_WhenTimeOffIsApprovedAndOwnedByUser_ReturnsTimeOffInfo()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.Approved, UserId = "userId" });

        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.CancelRequested, UserId = "userId" });

        var mediatorMock = new Mock<IMediator>();
        var mapperMock = new Mock<IMapper>();

        mapperMock.Setup(x => x.Map<TimeOffInfo>(It.IsAny<TimeOff>()))
            .Returns(new TimeOffInfo { Status = TimeOffStates.CancelRequested, UserId = "userId" });

        var handler = new CancelTimeOffRequestCommandHandler(timeOffRepositoryMock.Object, mediatorMock.Object, mapperMock.Object);

        // Act
        var result = await handler.Handle(new CancelTimeOffRequestCommand(1, "userId"), CancellationToken.None);

        // Assert
        Assert.Equal(TimeOffStates.CancelRequested, result.Status);
        Assert.Equal("userId", result.UserId);
    }

    [Fact]
    public async Task CancelTimeOffRequest_WhenTimeOffIsApprovedAndOwnedByUser_UpdatesTimeOff()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.Approved, UserId = "userId" });

        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.CancelRequested, UserId = "userId" });

        var mediatorMock = new Mock<IMediator>();
        var mapperMock = new Mock<IMapper>();

        mapperMock.Setup(x => x.Map<TimeOffInfo>(It.IsAny<TimeOff>()))
            .Returns(new TimeOffInfo { Status = TimeOffStates.CancelRequested, UserId = "userId" });

        var handler = new CancelTimeOffRequestCommandHandler(timeOffRepositoryMock.Object, mediatorMock.Object, mapperMock.Object);

        // Act
        await handler.Handle(new CancelTimeOffRequestCommand(1, "userId"), CancellationToken.None);

        // Assert
        timeOffRepositoryMock.Verify(x => x.UpdateAsync(It.Is<TimeOff>(x => x.Status == TimeOffStates.CancelRequested)), Times.Once);
    }
}