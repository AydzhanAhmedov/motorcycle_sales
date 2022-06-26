namespace motorcycle_sales.Web.ViewModels;

public class ManageUserRolesViewModel
{
    public class UserRole
    {
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }

    public string Username { get; set; }
    public string UserId { get; set; }
    public IList<UserRole> UserRoles { get; set; }
}
