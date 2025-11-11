using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebNews.Models.DTOs;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Admin.Role;
using WebNews.Services.Interfaces;

namespace WebNews.Services;

public class RoleService : IRoleService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IMapper _mapper;

    public RoleService(IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task<List<string>> GetUserRolesAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    public async Task<UserDto> GetUserWithRolesAsync(User user)
    {
        var roles = await GetUserRolesAsync(user);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Role = roles;
        return userDto;
    }
    
    public async Task<EditRolesViewModel?> GetRolesForUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var userRoles = await GetUserRolesAsync(user);
        var allRoles = await GetListAllRolesAsync();
        
        var viewModel = _mapper.Map<EditRolesViewModel>(allRoles);
        viewModel.UserId = userId;
        
        viewModel.AllRoles.ForEach(role =>
            role.IsSelected = userRoles.Contains(role.Name)
        );

        return viewModel;
    }

    public async Task AddRoleAsync(User user, string role)
    {
        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Role is not added to user");
        }
    }

    public async Task RemoveRoleAsync(User user, string role)
    {
        var result = await _userManager.RemoveFromRoleAsync(user, role);
        
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Role is not removed from user");
        }
    }

    public async Task UpdateRolesAsync(EditRolesViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId.ToString());
        if (user == null)
        {
            throw new InvalidOperationException("User is not found");
        }
        
        var currentRoles = await GetUserRolesAsync(user);
        var selectedRoles = model.AllRoles
            .Where(r => r.IsSelected)
            .Select(r => r.Name)
            .ToList();
        
        var rolesToRemove = currentRoles.Except(selectedRoles).ToList();
        foreach (var role in rolesToRemove)
        {
            await RemoveRoleAsync(user, role);
        }
        
        var rolesToAdd = selectedRoles.Except(currentRoles).ToList();
        foreach (var role in rolesToAdd)
        {
            await AddRoleAsync(user, role);
        }
    }
    
    public async Task CreateRoleAsync(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var role = new IdentityRole<Guid>(roleName);
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException();
            }
        }
    }

    public async Task<List<string?>> GetListAllRolesAsync()
    {
        var listRoles = await _roleManager.Roles
            .Select(n => n.Name)
            .ToListAsync();
        
        return listRoles;
    }
}