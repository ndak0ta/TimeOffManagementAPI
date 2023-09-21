using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Business.Email.Commands;
using TimeOffManagementAPI.Business.Users.Commands;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.UnitTests.UserTests;

public class CreateUserTests
{
    [Fact]
    public async Task CreateUserCommandHandler_ShouldCreateUser()
    {
        // Arrange
        UserRegistration userRegistration = new()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "mock@mock.com"
        };

        User user = new()
        {
            FirstName = userRegistration.FirstName,
            LastName = userRegistration.LastName,
            Email = userRegistration.Email,
            UserName = "JohnDoe"
        };

        Mock<UserManager<User>> userManagerMock = new(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        Mock<IMapper> mapperMock = new();
        mapperMock.Setup(x => x.Map<User>(It.IsAny<UserRegistration>()))
            .Returns(user);

        mapperMock.Setup(x => x.Map<UserInfo>(It.IsAny<User>())).Returns(new UserInfo { FirstName = userRegistration.FirstName, LastName = userRegistration.LastName, Email = userRegistration.Email, UserName = "JohnDoe" });

        Mock<IMediator> mediatorMock = new();

        mediatorMock.Setup(x => x.Send(It.IsAny<SendEmailCommand>(), It.IsAny<CancellationToken>()))
            .Verifiable();

        CreateUserCommand command = new(userRegistration);

        CreateUserCommandHandler handler = new(userManagerMock.Object, mapperMock.Object, mediatorMock.Object);

        // Act
        UserInfo result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userRegistration.FirstName, result.FirstName);
        Assert.Equal(userRegistration.LastName, result.LastName);
        Assert.Equal(userRegistration.Email, result.Email);
    }
}