using AutoMapper;
using TaskManager.Core.Domain;
using TaskManager.Core.DTOs;

namespace TaskManager.Core.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRegistrationDto, User>();

            CreateMap<UserListDto, User>();

            CreateMap<UserUpdateDto, User>();
        }
    }
}
