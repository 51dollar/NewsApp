using WebNews.Models.DTOs;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Admin.Role;

namespace WebNews.Services.Interfaces;

public interface IRoleService
{
    Task CreateRoleAsync(string roleName);
    Task<List<string>> GetUserRolesAsync(User user);
    Task<UserDto> GetUserWithRolesAsync(User user);
    Task<EditRolesViewModel?> GetRolesForUserAsync(Guid userId);
    Task AddRoleAsync(User user, string role);
    Task RemoveRoleAsync(User user, string role);
    Task UpdateRolesAsync(EditRolesViewModel model);
    Task<List<string?>> GetListAllRolesAsync();
}