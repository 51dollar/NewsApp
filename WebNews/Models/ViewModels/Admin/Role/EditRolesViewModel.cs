namespace WebNews.Models.ViewModels.Admin.Role;

public class EditRolesViewModel
{
    public Guid UserId { get; set; }
    public List<RoleItemViewModel> AllRoles { get; set; } = new ();
}