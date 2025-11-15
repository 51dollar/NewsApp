using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebNews.Data.UnitOfWork;
using WebNews.Models.DTOs;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Account;
using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IRoleService roleService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _roleService = roleService;
    }

    private IQueryable<User> GetAll()
    {
        return _unitOfWork.UserManager.Users.AsNoTracking();
    }

    public async Task<IReadOnlyList<User>> GetLatestAsync(int count)
    {
        return await GetAll()
            .OrderByDescending(u => u.DateCreate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.UserManager.FindByIdAsync(id.ToString());
    }

    public async Task UpdateAsync(User entity)
    {
        var result = await _unitOfWork.UserManager.UpdateAsync(entity);
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            var result = await _unitOfWork.UserManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
        }
    }

    public async Task<AccountViewModel> GetAccountByIdAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User is not exist");
        }

        return _mapper.Map<AccountViewModel>(user);
    }

    public async Task<IReadOnlyList<UserDto>> GetLatestWithRolesAsync(int count)
    {
        var users = await GetLatestAsync(10);

        var userDtos = new List<UserDto>();
        foreach (var user in users)
        {
            var userDto = await _roleService.GetUserWithRolesAsync(user);
            userDtos.Add(userDto);
        }

        return userDtos;
    }

    public async Task UpdateAccountAsync(Guid userId, AccountViewModel model)
    {
        var user = await GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User is not exist");

        user.UserName = model.UserName;
        user.Email = model.Email;

        await UpdateAsync(user);
    }

    public async Task UpdatePasswordAsync(Guid userId, ChangePasswordViewModel model)
    {
        var user = await GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User does not exist");

        var token = await _unitOfWork.UserManager.GeneratePasswordResetTokenAsync(user);
        var result = await _unitOfWork.UserManager.ResetPasswordAsync(user, token, model.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception(errors);
        }
    }
}