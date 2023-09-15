
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Business.Email.Commands;
using TimeOffManagementAPI.Business.TimeOffs.Commands;
using TimeOffManagementAPI.Business.Users.Commands;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Constants;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.UnitTests.TimeOffTests;

public class ApproveTimeOffTests
{
    [Fact]
    public async Task ApproveTimeOffCommandHandler_WhenTimeOffIsNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        var mediatorMock = new Mock<IMediator>();
        var mapperMock = new Mock<IMapper>();
        var command = new ApproveTimeOffCommand(1, true);
        var handler = new ApproveTimeOffCommandHandler(timeOffRepositoryMock.Object, userManagerMock.Object, mediatorMock.Object, mapperMock.Object);

        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TimeOff)null!);

        // Act
        async Task act() => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exc = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal("Time off not found", exc.Message);
    }

    [Fact]
    public async Task ApproveTimeOffCommandHandler_WhenTimeOffIsFoundAndIsApprovedIsTrue_UpdatesTimeOffAndSendsEmail()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        var mediatorMock = new Mock<IMediator>();
        var mapperMock = new Mock<IMapper>();
        var command = new ApproveTimeOffCommand(1, true);
        var handler = new ApproveTimeOffCommandHandler(timeOffRepositoryMock.Object, userManagerMock.Object, mediatorMock.Object, mapperMock.Object);
        var timeOff = new TimeOff { UserId = "1" };
        var user = new User { Id = "1", Email = "mock@mock.com" };

        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(timeOff);
        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>()))
        .Callback<TimeOff>(x => timeOff = x)
        .ReturnsAsync(timeOff);
        userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        timeOffRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TimeOff>()), Times.Once);
        mediatorMock.Verify(x => x.Send(It.IsAny<UpdateRemaningAnnualTimeOffCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Send(It.IsAny<SendEmailCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal(TimeOffStates.Approved, timeOff.Status);
    }

    [Fact]
    public async Task ApproveTimeOffCommandHandler_WhenTimeOffIsFoundAndIsApprovedIsFalse_UpdatesTimeOffAndSendsEmail()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        var mediatorMock = new Mock<IMediator>();
        var mapperMock = new Mock<IMapper>();
        var command = new ApproveTimeOffCommand(1, false);
        var handler = new ApproveTimeOffCommandHandler(timeOffRepositoryMock.Object, userManagerMock.Object, mediatorMock.Object, mapperMock.Object);
        var timeOff = new TimeOff { UserId = "1" };
        var user = new User { Id = "1", Email = "mock@mock.com" };

        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(timeOff);
        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>()))
        .Callback<TimeOff>(x => timeOff = x)
        .ReturnsAsync(timeOff);
        userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        timeOffRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TimeOff>()), Times.Once);
        mediatorMock.Verify(x => x.Send(It.IsAny<SendEmailCommand>(), It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal(TimeOffStates.Rejected, timeOff.Status);
    }
}