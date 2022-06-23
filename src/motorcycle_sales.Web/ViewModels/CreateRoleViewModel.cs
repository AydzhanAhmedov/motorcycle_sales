using System.ComponentModel;

namespace motorcycle_sales.Web.ViewModels;

public class CreateRoleViewModel
{
    [DisplayName("Role name")]
    public string RoleName { get; set; }
}
