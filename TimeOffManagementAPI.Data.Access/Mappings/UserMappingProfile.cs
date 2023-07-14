using AutoMapper;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Data.Access;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserRegistrationDto, User>();
    }
}