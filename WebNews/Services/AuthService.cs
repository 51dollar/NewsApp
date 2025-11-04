using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebNews.Data.UnitOfWork;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Auth;
using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task RegisterAsync(RegisterViewModel request, string role)
    {
        var user = _mapper.Map<User>(request);
        
        var result = await _unitOfWork.UserManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }

        if (!await _unitOfWork.RoleManager.RoleExistsAsync(role))
        {
            await _unitOfWork.RoleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
        
        await _unitOfWork.UserManager.AddToRoleAsync(user, role);
        await _unitOfWork.SignInManager.SignInAsync(user, false);
    }

    public async Task LoginAsync(LoginViewModel request)
    {
        var user = await _unitOfWork.UserManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception("Email does not exist");
        }
        
        var result = await _unitOfWork.SignInManager.PasswordSignInAsync(
            user, request.Password, request.RememberMe, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            throw new Exception("Invalid password");
        }
    }

    public async Task LogoutAsync()
    {
        await _unitOfWork.SignInManager.SignOutAsync();
    }
}