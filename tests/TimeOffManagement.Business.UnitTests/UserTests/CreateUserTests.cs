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
        var userRegistration = new UserRegistration
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "mock@mock.com"
        };

        var user = new User
        {
            FirstName = userRegistration.FirstName,
            LastName = userRegistration.LastName,
            Email = userRegistration.Email,
            UserName = "JohnDoe"
        };

        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Verifiable();

        userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Verifiable();

        userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user)
            .Verifiable();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<User>(It.IsAny<UserRegistration>()))
            .Returns(user)
            .Verifiable();

        mapperMock.Setup(x => x.Map<UserInfo>(It.IsAny<User>()))
            .Returns(new UserInfo
            {
                FirstName = userRegistration.FirstName,
                LastName = userRegistration.LastName,
                Email = userRegistration.Email,
                UserName = "JohnDoe"
            })
            .Verifiable();

        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(x => x.Send(It.IsAny<SendEmailCommand>(), It.IsAny<CancellationToken>()))
            .Verifiable();

        var command = new CreateUserCommand(userRegistration);

        var handler = new CreateUserCommandHandler(userManagerMock.Object, mapperMock.Object, mediatorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userRegistration.FirstName, result.FirstName);
        Assert.Equal(userRegistration.LastName, result.LastName);
        Assert.Equal(userRegistration.Email, result.Email);
    }
}