using AutoMapper;
using WebNews.Models.DTOs;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Account;
using WebNews.Models.ViewModels.Admin.Role;
using WebNews.Models.ViewModels.Auth;

namespace WebNews.AutoMapper.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RegisterViewModel>().ReverseMap();
        CreateMap<User, AccountViewModel>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        
        CreateMap<string, RoleItemViewModel>()
            .ForMember(dest => 
                dest.Name, opt => 
                opt.MapFrom(src => src))
            .ForMember(dest => 
                dest.IsSelected, opt => 
                opt.Ignore());
        
        CreateMap<List<string>, EditRolesViewModel>()
            .ForMember(dest => 
                dest.AllRoles, opt => 
                opt.MapFrom(src => src));
    }
}