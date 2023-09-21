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
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((TimeOff)null!);

        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();

        CancelTimeOffRequestCommandHandler handler = new(timeOffRepositoryMock.Object, mediatorMock.Object, mapperMock.Object);

        // Act
        async Task act() => await handler.Handle(new CancelTimeOffRequestCommand(1, "userId"), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task CancelTimeOffRequest_WhenTimeOffIsNotApproved_ThrowsUnprocessableEntityException()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.Pending });

        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();

        CancelTimeOffRequestCommandHandler handler = new(timeOffRepositoryMock.Object, mediatorMock.Object, mapperMock.Object);

        // Act
        async Task act() => await handler.Handle(new CancelTimeOffRequestCommand(1, "userId"), CancellationToken.None);

        // Assert
        UnprocessableEntityException exc = await Assert.ThrowsAsync<UnprocessableEntityException>(act);

        Assert.Equal("You can only cancel an approved time off", exc.Message);
    }

    [Fact]
    public async Task CancelTimeOffRequest_WhenTimeOffIsNotOwnedByUser_ThrowsUnprocessableEntityException()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.Approved, UserId = "otherUserId" });

        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();

        CancelTimeOffRequestCommandHandler handler = new(timeOffRepositoryMock.Object, mediatorMock.Object, mapperMock.Object);

        // Act
        async Task act() => await handler.Handle(new CancelTimeOffRequestCommand(1, "userId"), CancellationToken.None);

        // Assert
        UnprocessableEntityException exc = await Assert.ThrowsAsync<UnprocessableEntityException>(act);

        Assert.Equal("You can only send a cancel request for your own time off", exc.Message);
    }

    [Fact]
    public async Task CancelTimeOffRequest_WhenTimeOffIsApprovedAndOwnedByUser_ReturnsTimeOffInfo()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.Approved, UserId = "userId" });

        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.CancelRequested, UserId = "userId" });

        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();

        mapperMock.Setup(x => x.Map<TimeOffInfo>(It.IsAny<TimeOff>()))
            .Returns(new TimeOffInfo { Status = TimeOffStates.CancelRequested, UserId = "userId" });

        CancelTimeOffRequestCommandHandler handler = new(timeOffRepositoryMock.Object, mediatorMock.Object, mapperMock.Object);

        // Act
        TimeOffInfo result = await handler.Handle(new CancelTimeOffRequestCommand(1, "userId"), CancellationToken.None);

        // Assert
        Assert.Equal(TimeOffStates.CancelRequested, result.Status);
        Assert.Equal("userId", result.UserId);
    }

    [Fact]
    public async Task CancelTimeOffRequest_WhenTimeOffIsApprovedAndOwnedByUser_UpdatesTimeOff()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.Approved, UserId = "userId" });

        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>()))
            .ReturnsAsync(new TimeOff { Status = TimeOffStates.CancelRequested, UserId = "userId" });

        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();

        mapperMock.Setup(x => x.Map<TimeOffInfo>(It.IsAny<TimeOff>()))
            .Returns(new TimeOffInfo { Status = TimeOffStates.CancelRequested, UserId = "userId" });

        CancelTimeOffRequestCommandHandler handler = new(timeOffRepositoryMock.Object, mediatorMock.Object, mapperMock.Object);

        // Act
        await handler.Handle(new CancelTimeOffRequestCommand(1, "userId"), CancellationToken.None);

        // Assert
        timeOffRepositoryMock.Verify(x => x.UpdateAsync(It.Is<TimeOff>(x => x.Status == TimeOffStates.CancelRequested)), Times.Once);
    }
}