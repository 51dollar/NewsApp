using AutoMapper;
using WebNews.Data.UnitOfWork;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Auth;
using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IRoleService roleService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _roleService = roleService;
    }

    public async Task RegisterAsync(RegisterViewModel request, string role)
    {
        var user = _mapper.Map<User>(request);

        var createUserResult = await _unitOfWork.UserManager.CreateAsync(user, request.Password);
        if (!createUserResult.Succeeded)
        {
            throw new InvalidOperationException("Create user failed");
        }

        await _roleService.CreateRoleAsync(role);
        await _roleService.AddRoleAsync(user, role);

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