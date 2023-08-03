using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Domain;

namespace TaskManager.Core.Mapping
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<Domain.Task, TaskDto>();

            CreateMap<TaskCreationDto, Domain.Task>();

            CreateMap<TaskUpdateDto, Domain.Task>().ReverseMap();

            CreateMap<TaskDto, User>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CreateUser))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AssignUser))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.CreateUserLogin))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AssignUserLogin));

        }
    }
}
