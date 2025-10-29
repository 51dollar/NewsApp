using AutoMapper;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Account;
using WebNews.Models.ViewModels.Auth;

namespace WebNews.Helpers.AutoMapper.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RegisterViewModel>();
        CreateMap<User, AccountViewModel>();
    }
}