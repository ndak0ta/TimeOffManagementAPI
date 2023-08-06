using AutoMapper;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Data.Model.Mappings;

public class TimeOffCancelMappingProfile : Profile
{
    public TimeOffCancelMappingProfile()
    {
        CreateMap<TimeOffCancelRequest, TimeOffCancel>();
    }
}