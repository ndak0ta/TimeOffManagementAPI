using TimeOffManagementAPI.Business.Calendar.Commands;
using TimeOffManagementAPI.Business.TimeOffs.Commands;
using TimeOffManagementAPI.Business.Users.Queries;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.UnitTests.TimeOffTests;

public class TimeOffTests
{
    [Fact]
    public async Task CreateTimeOffCommandHandler_WhenTimeOffRequestIsNotNull_ReturnsTimeOffInfo()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();
        CreateTimeOffCommand command = new(new TimeOffRequest
        {
            StartDate = new DateTime(2023, 11, 1), // TODO add dynamic date
            EndDate = new DateTime(2023, 11, 2),
            UserId = Guid.NewGuid().ToString()
        });
        CreateTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object, mediatorMock.Object);

        mediatorMock.Setup(x => x.Send(It.IsAny<CountDaysExcludingHolidaysCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(5);
        mediatorMock.Setup(x => x.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new UserInfo
        {
            RemainingAnnualTimeOffs = 10
        });
        timeOffRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<TimeOff>())).ReturnsAsync(new TimeOff());
        mapperMock.Setup(x => x.Map<TimeOff>(It.IsAny<TimeOffRequest>())).Returns(new TimeOff
        {
            StartDate = new DateTime(2023, 11, 1), // TODO add dynamic date
            EndDate = new DateTime(2023, 11, 2),
            UserId = Guid.NewGuid().ToString()
        });
        mapperMock.Setup(x => x.Map<TimeOffInfo>(It.IsAny<TimeOff>())).Returns(new TimeOffInfo());

        // Act
        TimeOffInfo result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<TimeOffInfo>(result);
    }

    [Fact]
    public async Task CreateTimeOffCommandHandler_WhenTimeOffRequestIsNotNullAndStartDateIsBeforeCurrentDate_ThrowsArgumentException()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();
        CreateTimeOffCommand command = new(new TimeOffRequest());
        CreateTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object, mediatorMock.Object);

        mapperMock.Setup(x => x.Map<TimeOff>(It.IsAny<TimeOffRequest>())).Returns(new TimeOff
        {
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2021, 1, 2),
            UserId = Guid.NewGuid().ToString()
        });

        // Act
        async Task act() => await handler.Handle(command, CancellationToken.None);

        // Assert
        ArgumentException exc = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("Start date must be in the future", exc.Message);
    }

    [Fact]
    public async Task CreateTimeOffCommandHandler_WhenTimeOffRequestIsNotNullAndStartDateIsAfterEndDate_ThrowsArgumentException()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();
        CreateTimeOffCommand command = new(new TimeOffRequest());
        CreateTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object, mediatorMock.Object);

        mapperMock.Setup(x => x.Map<TimeOff>(It.IsAny<TimeOffRequest>())).Returns(new TimeOff
        {
            StartDate = new DateTime(2023, 11, 5),
            EndDate = new DateTime(2023, 11, 2),
            UserId = Guid.NewGuid().ToString()
        });

        // Act
        async Task act() => await handler.Handle(command, CancellationToken.None);

        // Assert
        ArgumentException exc = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("Start date must be before end date", exc.Message);
    }

    [Fact]
    public async Task CreateTimeOffCommandHandler_WhenTimeOffRequestIsNotNullAndStartDateAndEndDateAreNotInCurrentYear_ThrowsArgumentException()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();
        CreateTimeOffCommand command = new(new TimeOffRequest());
        CreateTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object, mediatorMock.Object);

        mapperMock.Setup(x => x.Map<TimeOff>(It.IsAny<TimeOffRequest>())).Returns(new TimeOff
        {
            StartDate = new DateTime(2023, 11, 11), // TODO add dynamic date
            EndDate = new DateTime(2024, 11, 12),
            UserId = Guid.NewGuid().ToString()
        });

        // Act
        async Task act() => await handler.Handle(command, CancellationToken.None);

        // Assert
        ArgumentException exc = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("Start date and end date must be in the same year", exc.Message);
    }

    [Fact]
    public async Task CreateTimeOffCommandHandler_WhenTimeOffRequestIsNotNullAndUserIdIsNull_ThrowsArgumentException()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();
        CreateTimeOffCommand command = new(new TimeOffRequest());
        CreateTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object, mediatorMock.Object);

        mapperMock.Setup(x => x.Map<TimeOff>(It.IsAny<TimeOffRequest>())).Returns(new TimeOff
        {
            StartDate = new DateTime(2023, 11, 1), // TODO add dynamic date
            EndDate = new DateTime(2023, 11, 2),
            UserId = null
        });

        // Act
        async Task act() => await handler.Handle(command, CancellationToken.None);

        // Assert
        ArgumentException exc = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("User id is required", exc.Message);
    }

    [Fact]
    public async Task CreateTimeOffCommandHandler_WhenTimeOffRequestIsNotNullAndTotalDaysIsGreaterThanRemainingAnnualTimeOffs_ThrowsUnprocessableEntityException()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMediator> mediatorMock = new();
        Mock<IMapper> mapperMock = new();
        CreateTimeOffCommand command = new(new TimeOffRequest());
        CreateTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object, mediatorMock.Object);
        mediatorMock.Setup(x => x.Send(It.IsAny<CountDaysExcludingHolidaysCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(15);
        mediatorMock.Setup(x => x.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new UserInfo
        {
            RemainingAnnualTimeOffs = 10
        });
        mapperMock.Setup(x => x.Map<TimeOff>(It.IsAny<TimeOffRequest>())).Returns(new TimeOff
        {
            StartDate = new DateTime(2023, 10, 1), // TODO add dynamic date
            EndDate = new DateTime(2023, 12, 2),
            UserId = Guid.NewGuid().ToString()
        });

        // Act
        async Task act() => await handler.Handle(command, CancellationToken.None);

        // Assert
        UnprocessableEntityException exc = await Assert.ThrowsAsync<UnprocessableEntityException>(act);
        Assert.Equal("You don't have enough time off left", exc.Message);
    }
}