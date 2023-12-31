using AutoMapper;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Model.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserInfo>();
        CreateMap<UserRegistration, User>();
        CreateMap<UserUpdate, User>();
        CreateMap<User, UserUpdate>();
        CreateMap<UserUpdateContact, User>();
    }
}