namespace motorcycle_sales.Web.ViewModels;

public class ManageRolePermissionsViewModel
{
    public class RoleClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }

    public string RoleName { get; set; }
    public string RoleId { get; set; }
    public IList<RoleClaim> RoleClaims { get; set; }
}
