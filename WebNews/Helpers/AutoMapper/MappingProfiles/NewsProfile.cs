using AutoMapper;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.News;

namespace WebNews.Helpers.AutoMapper.MappingProfiles;

public class NewsProfile : Profile
{
    public NewsProfile()
    {
        CreateMap<News, CreateViewModel>().ReverseMap();
    }
}