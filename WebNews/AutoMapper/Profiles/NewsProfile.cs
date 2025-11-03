using AutoMapper;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.News;

namespace WebNews.AutoMapper.Profiles;

public class NewsProfile : Profile
{
    public NewsProfile()
    {
        CreateMap<News, CreateViewModel>().ReverseMap();
        CreateMap<News, EditViewModel>().ReverseMap();
    }
}