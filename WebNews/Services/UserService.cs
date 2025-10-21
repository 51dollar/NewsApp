using Microsoft.EntityFrameworkCore;
using WebNews.Data.UnitOfWork;
using WebNews.Models.Entities;
using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IQueryable<User> GetAll()
    {
        return _unitOfWork.UserRepository.GetAll();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _unitOfWork.UserRepository.GetAllAsync();
    }

    public async Task<IEnumerable<User>> GetLatestAsync(int count)
    {
        return await GetAll()
            .OrderByDescending(u => u.DateCreate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.UserRepository.GetByIdAsync(id);
    }

    public async Task CreateAsync(User entity)
    {
        await _unitOfWork.UserRepository.AddAsync(entity);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(User entity)
    {
        _unitOfWork.UserRepository.Update(entity);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user != null)
        {
            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveAsync();
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
    }
}