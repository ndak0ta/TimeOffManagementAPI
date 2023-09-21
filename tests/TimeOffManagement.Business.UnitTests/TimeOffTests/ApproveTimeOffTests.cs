
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
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<UserManager<User>> userManagerMock = new(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();
        ApproveTimeOffCommand command = new(1, true);
        ApproveTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object, userManagerMock.Object, mediatorMock.Object, mapperMock.Object);

        timeOffRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TimeOff)null!);

        // Act
        async Task act() => await handler.Handle(command, CancellationToken.None);

        // Assert
        NotFoundException exc = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal("Time off not found", exc.Message);
    }

    [Fact]
    public async Task ApproveTimeOffCommandHandler_WhenTimeOffIsFoundAndIsApprovedIsTrue_UpdatesTimeOffAndSendsEmail()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<UserManager<User>> userManagerMock = new(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();
        ApproveTimeOffCommand command = new(1, true);
        ApproveTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object, userManagerMock.Object, mediatorMock.Object, mapperMock.Object);
        TimeOff timeOff = new() { UserId = "1" };
        User user = new() { Id = "1", Email = "mock@mock.com" };

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
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<UserManager<User>> userManagerMock = new(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();
        ApproveTimeOffCommand command = new(1, false);
        ApproveTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object, userManagerMock.Object, mediatorMock.Object, mapperMock.Object);
        TimeOff timeOff = new() { UserId = "1" };
        User user = new() { Id = "1", Email = "mock@mock.com" };

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