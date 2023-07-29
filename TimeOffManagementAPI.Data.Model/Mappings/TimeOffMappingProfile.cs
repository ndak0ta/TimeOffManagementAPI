using AutoMapper;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Data.Model.Mappings;

public class TimeOffMappingProfile : Profile
{
    public TimeOffMappingProfile()
    {
        CreateMap<TimeOffRequest, TimeOff>();
        CreateMap<TimeOff, TimeOffInfo>();
    }
}