using TimeOffManagementAPI.Business.TimeOffs.Queries;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.UnitTests.TimeOffTests;

public class GetAllTimeOffsTests
{
    [Fact]
    public async Task GetAllTimeOffs_ReturnsAllTimeOffs()
    {
        // Arrange
        Mock<ITimeOffRepository> timeOffRepositoryMock = new();
        Mock<IMapper> mapperMock = new();

        GetAllTimeOffsQuery query = new();
        GetAllTimeOffsQueryHandler handler = new(timeOffRepositoryMock.Object, mapperMock.Object);

        List<TimeOff> timeOffs = new();

        timeOffRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(timeOffs);
        mapperMock.Setup(x => x.Map<IEnumerable<TimeOffInfo>>(It.IsAny<IEnumerable<TimeOff>>()))
        .Returns(new List<TimeOffInfo> {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 },
        });

        // Act
        IEnumerable<TimeOffInfo> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<TimeOffInfo>>(result);
        Assert.Equal(3, result.Count());
    }
}