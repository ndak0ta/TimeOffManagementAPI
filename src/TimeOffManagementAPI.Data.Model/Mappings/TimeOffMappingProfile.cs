using AutoMapper;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Model.Mappings;

public class TimeOffMappingProfile : Profile
{
    public TimeOffMappingProfile()
    {
        CreateMap<TimeOffRequest, TimeOff>();
        CreateMap<TimeOff, TimeOffInfo>();
        CreateMap<TimeOffUpdate, TimeOff>();
    }
}