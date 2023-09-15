using TimeOffManagementAPI.Business.Calendar.Commands;
using TimeOffManagementAPI.Business.TimeOffs.Commands;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Constants;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Business.UnitTests.TimeOffTests;

public class UpdateTimeOffTests
{

    [Fact]
    public async Task UpdateTimeOffCommandHandler_ShouldReturnUpdatedTimeOff()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var mapperMock = new Mock<IMapper>();
        var mediatorMock = new Mock<IMediator>();

        var timeOff = new TimeOff
        {
            Id = 1,
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2021, 1, 5),
            Status = TimeOffStates.Pending
        };

        var timeOffUpdate = new TimeOffUpdate
        {
            Id = 1,
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2021, 1, 5),
        };

        var timeOffInfo = new TimeOffInfo
        {
            Id = 1,
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2021, 1, 5),
            Status = TimeOffStates.Pending
        };

        var command = new UpdateTimeOffCommand(timeOffUpdate);

        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>())).ReturnsAsync(timeOff);
        mapperMock.Setup(x => x.Map<TimeOff>(It.IsAny<TimeOffUpdate>())).Returns(timeOff);
        mediatorMock.Setup(x => x.Send(It.IsAny<CountDaysExcludingHolidaysCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(5);
        mapperMock.Setup(x => x.Map<TimeOffInfo>(It.IsAny<TimeOff>())).Returns(timeOffInfo);

        var handler = new UpdateTimeOffCommandHandler(timeOffRepositoryMock.Object, mapperMock.Object, mediatorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(timeOffInfo, result);
    }

    [Fact]
    public async Task UpdateTimeOffCommandHandler_WhenTimeOffIsNotPending_ShouldThrowUnprocessableEntityException()
    {
        // Arrange
        var timeOffRepositoryMock = new Mock<ITimeOffRepository>();
        var mapperMock = new Mock<IMapper>();
        var mediatorMock = new Mock<IMediator>();

        var timeOff = new TimeOff
        {
            Id = 1,
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2021, 1, 5),
            Status = TimeOffStates.Approved
        };

        var timeOffUpdate = new TimeOffUpdate
        {
            Id = 1,
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2021, 1, 5),
        };

        var command = new UpdateTimeOffCommand(timeOffUpdate);

        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>())).ReturnsAsync(timeOff);
        mapperMock.Setup(x => x.Map<TimeOff>(It.IsAny<TimeOffUpdate>())).Returns(timeOff);
        mediatorMock.Setup(x => x.Send(It.IsAny<CountDaysExcludingHolidaysCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(5);

        var handler = new UpdateTimeOffCommandHandler(timeOffRepositoryMock.Object, mapperMock.Object, mediatorMock.Object);

        // Act
        var result = await Assert.ThrowsAsync<UnprocessableEntityException>(() => handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Equal("You can't make changes on an approved or rejected time off request", result.Message);
    }
}