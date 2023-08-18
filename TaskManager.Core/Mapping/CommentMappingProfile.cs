using AutoMapper;
using TaskManager.Core.Domain;
using TaskManager.Core.DTOs;

namespace TaskManager.Core.Mapping
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile(){
            CreateMap<Comment, AddCommentDto>();

            CreateMap<AddCommentDto, Comment>();
        }
    }
}
