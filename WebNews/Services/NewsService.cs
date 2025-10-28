using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebNews.Data.UnitOfWork;
using WebNews.Helpers.Image;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.News;
using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class NewsService : INewsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ImageHelper _imageHelper;

    public NewsService(IUnitOfWork unitOfWork, IMapper mapper, ImageHelper imageHelper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _imageHelper = imageHelper;
    }
    public IQueryable<News> GetAll()
    {
        return _unitOfWork.NewsRepository.GetAll();
    }

    public async Task<IEnumerable<News>> GetAllAsync()
    {
        return await _unitOfWork.NewsRepository.GetAllAsync();
    }

    public async Task<News?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.NewsRepository.GetByIdAsync(id);
    }

    public async Task<EditViewModel?> ReturnViewModelAsync(Guid id)
    {
        var model = await _unitOfWork.NewsRepository.GetByIdAsync(id);
        if (model == null)
        {
            throw new Exception($"News with id {id} not found");
        }
        
        return _mapper.Map<EditViewModel>(model);
    }

    public async Task UpdateNewsAsync(EditViewModel model, string userId, string username)
    {
        var modelDb = await _unitOfWork.NewsRepository.GetByIdAsync(model.Id);
        if (modelDb == null)
        {
            throw new Exception($"News with id {model.Id} not found");
        }
        
        var modelMap = _mapper.Map<News>(model);
        
        if (model.InputImage != null)
        {
            if (model.ImageNews != null && model.ImageNews != "/Image/default.png") 
            {
                _imageHelper.DeleteFile(model.ImageNews);
            }

            var newUrlImage = await _imageHelper.UploadFileAsync(model.InputImage);
            modelMap.ImagePath = newUrlImage;
        }
        
        modelMap.UserId = Guid.Parse(userId);
        modelMap.Author = username;
        modelMap.CreatedAtUtc = DateTime.UtcNow;

        await UpdateAsync(modelMap);
    }
    
    public async Task<IEnumerable<News>> GetLatestAsync(int count)
    {
        return await GetAll()
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(count)
            .ToListAsync();
    }
    public async Task CreateAsync(News entity)
    {
        await _unitOfWork.NewsRepository.AddAsync(entity);
        await _unitOfWork.SaveAsync();
    }

    public async Task CreateNewsAsync(CreateViewModel model, string userId, string username)
    {
        var news = _mapper.Map<News>(model);

        if (model.Image != null)
        {
            if (_imageHelper.ValidFileExtension(model.Image))
            {
                throw new Exception("Invalid file extension. Allowed format are .jpg, .jpeg, .png");
            }

            news.ImagePath = await _imageHelper.UploadFileAsync(model.Image);
        }
        else
        {
            news.ImagePath = "/Image/default.png";
        }
        
        news.Author = username;
        news.UserId = Guid.Parse(userId);

        await CreateAsync(news);
    }

    public async Task UpdateAsync(News news)
    {
        _unitOfWork.NewsRepository.Update(news);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var getNews = await GetByIdAsync(id);

        if (getNews != null)
        {
            _unitOfWork.NewsRepository.Delete(getNews);
        }

        await _unitOfWork.SaveAsync();
    }
}