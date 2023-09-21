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
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMapper> mapperMock = new();
        Mock<IMediator> mediatorMock = new();

        TimeOff timeOff = new()
        {
            Id = 1,
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2021, 1, 5),
            Status = TimeOffStates.Pending
        };

        TimeOffUpdate timeOffUpdate = new()
        {
            Id = 1,
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2021, 1, 5),
        };

        TimeOffInfo timeOffInfo = new()
        {
            Id = 1,
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2021, 1, 5),
            Status = TimeOffStates.Pending
        };

        UpdateTimeOffCommand command = new(timeOffUpdate);

        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>())).ReturnsAsync(timeOff);
        mapperMock.Setup(x => x.Map<TimeOff>(It.IsAny<TimeOffUpdate>())).Returns(timeOff);
        mediatorMock.Setup(x => x.Send(It.IsAny<CountDaysExcludingHolidaysCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(5);
        mapperMock.Setup(x => x.Map<TimeOffInfo>(It.IsAny<TimeOff>())).Returns(timeOffInfo);

        UpdateTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object, mediatorMock.Object);

        // Act
        TimeOffInfo result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(timeOffInfo, result);
    }

    [Fact]
    public async Task UpdateTimeOffCommandHandler_WhenTimeOffIsNotPending_ShouldThrowUnprocessableEntityException()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMapper> mapperMock = new();
        Mock<IMediator> mediatorMock = new();

        TimeOff timeOff = new()
        {
            Id = 1,
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2021, 1, 5),
            Status = TimeOffStates.Approved
        };

        TimeOffUpdate timeOffUpdate = new()
        {
            Id = 1,
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2021, 1, 5),
        };

        UpdateTimeOffCommand command = new(timeOffUpdate);

        timeOffRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TimeOff>())).ReturnsAsync(timeOff);
        mapperMock.Setup(x => x.Map<TimeOff>(It.IsAny<TimeOffUpdate>())).Returns(timeOff);
        mediatorMock.Setup(x => x.Send(It.IsAny<CountDaysExcludingHolidaysCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(5);

        UpdateTimeOffCommandHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object, mediatorMock.Object);

        // Act
        UnprocessableEntityException result = await Assert.ThrowsAsync<UnprocessableEntityException>(() => handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Equal("You can't make changes on an approved or rejected time off request", result.Message);
    }
}