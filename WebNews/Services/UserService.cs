using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebNews.Data.UnitOfWork;
using WebNews.Helpers.Auth;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Auth;
using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Hasher<User> _hasher;
    private readonly AuthHelper _authHelper;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, Hasher<User> hasher, AuthHelper authHelper, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _hasher = hasher;
        _authHelper = authHelper;
        _mapper = mapper;
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

    public async Task RegisterAsync(RegisterViewModel model)
    {
        var user = _mapper.Map<User>(model);
        
        user.Role = RoleType.User;
        user.PasswordHash = _hasher.HashPassword(
            user,
            model.Password);
        
        await CreateAsync(user);
        await _authHelper.SignInUserAsync(user);
    }

    public async Task LoginAsync(LoginViewModel model)
    {
        var userFromDb = await GetUserByEmailAsync(model.Email);
        if (userFromDb == null)
        {
            throw new Exception("Email is not exist");
        }
        
        var isPasswordValid = _hasher.VerifyPassword(
            userFromDb,
            userFromDb.PasswordHash,
            model.Password);

        if (!isPasswordValid)
        {
            throw new Exception("Password is not valid");
        }

        await _authHelper.SignInUserAsync(userFromDb);
    }

    public async Task LogoutAsync()
    {
        await _authHelper.SignOutUserAsync();
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

    public async Task<bool> IsUserExistsByEmailAsync(string email)
    {
        return await _unitOfWork.UserRepository.ExistsByEmailAsync(email);
    }
}