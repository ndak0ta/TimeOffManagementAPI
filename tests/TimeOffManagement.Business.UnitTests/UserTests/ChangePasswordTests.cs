using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Business.Users.Commands;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.UnitTests.UserTests;

public class ChangePasswordTests
{
    [Fact]
    public async Task ChangePasswordCommandHandler_WhenUserNotFound_ThrowsArgumentNullException()
    {
        // Arrange
        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        var mediatorMock = new Mock<IMediator>();

        var command = new ChangePasswordCommand(new UserChangePassword
        {
            Id = "1",
            OldPassword = "oldPassword",
            NewPassword = "newPassword"
        });

        userManagerMock.Setup(x => x.FindByIdAsync(command.UserChangePassword!.Id))
            .ReturnsAsync((User)null!);

        var handler = new ChangePasswordCommandHandler(userManagerMock.Object);

        // Act
        var result = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Equal("User not found (Parameter 'UserChangePassword')", result.Message);
        userManagerMock.Verify(x => x.FindByIdAsync(command.UserChangePassword!.Id), Times.Once);
        userManagerMock.Verify(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        userManagerMock.Verify(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ChangePasswordCommandHandler_WhenOldPasswordIsIncorrect_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        var mediatorMock = new Mock<IMediator>();

        var command = new ChangePasswordCommand(new UserChangePassword
        {
            Id = "1",
            OldPassword = "oldPassword",
            NewPassword = "newPassword"
        });

        userManagerMock.Setup(x => x.FindByIdAsync(command.UserChangePassword!.Id))
            .ReturnsAsync(new User());

        userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var handler = new ChangePasswordCommandHandler(userManagerMock.Object);

        // Act
        var result = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Equal("Password is incorrect", result.Message);
        userManagerMock.Verify(x => x.FindByIdAsync(command.UserChangePassword!.Id), Times.Once);
        userManagerMock.Verify(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        userManagerMock.Verify(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ChangePasswordCommandHandler_WhenPasswordChangeFails_ReturnsFalse()
    {
        // Arrange
        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        var mediatorMock = new Mock<IMediator>();

        var command = new ChangePasswordCommand(new UserChangePassword
        {
            Id = "1",
            OldPassword = "oldPassword",
            NewPassword = "newPassword"
        });

        userManagerMock.Setup(x => x.FindByIdAsync(command.UserChangePassword!.Id))
            .ReturnsAsync(new User());

        userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        userManagerMock.Setup(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        var handler = new ChangePasswordCommandHandler(userManagerMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        userManagerMock.Verify(x => x.FindByIdAsync(command.UserChangePassword!.Id), Times.Once);
        userManagerMock.Verify(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        userManagerMock.Verify(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ChangePasswordCommandHandler_WhenPasswordChangeSucceeds_ReturnsTrue()
    {
        // Arrange
        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        var mediatorMock = new Mock<IMediator>();

        var command = new ChangePasswordCommand(new UserChangePassword
        {
            Id = "1",
            OldPassword = "oldPassword",
            NewPassword = "newPassword"
        });

        userManagerMock.Setup(x => x.FindByIdAsync(command.UserChangePassword!.Id))
            .ReturnsAsync(new User());

        userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        userManagerMock.Setup(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var handler = new ChangePasswordCommandHandler(userManagerMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        userManagerMock.Verify(x => x.FindByIdAsync(command.UserChangePassword!.Id), Times.Once);
        userManagerMock.Verify(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        userManagerMock.Verify(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}