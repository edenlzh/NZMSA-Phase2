using AutoMapper;
using HandInHand.Dtos;
using HandInHand.Models;

namespace HandInHand.Mappings;

public class DomainProfile : Profile
{
    public DomainProfile()
    {
        // User
        CreateMap<User, UserDto>().ReverseMap().ForMember(u => u.PasswordHash, opt => opt.Ignore());

        // Skill
        CreateMap<Skill, SkillDto>()
            .ForMember(d => d.UserName,
                opt => opt.MapFrom(s => s.User != null ? s.User.UserName : null))
            .ReverseMap()
            .ForMember(s => s.User, opt => opt.Ignore());

        // HelpRequest
        CreateMap<HelpRequest, HelpRequestDto>()
            .ForMember(d => d.RequesterName, opt => opt.MapFrom(h => h.Requester!.UserName))
            .ReverseMap()
            .ForMember(h => h.Requester, opt => opt.Ignore());

        // Comment
        CreateMap<Comment, CommentDto>().ForMember(d => d.AuthorName, c => c.MapFrom(s => s.Author!.UserName));

        CreateMap<CommentCreateDto, Comment>();
    }
}
