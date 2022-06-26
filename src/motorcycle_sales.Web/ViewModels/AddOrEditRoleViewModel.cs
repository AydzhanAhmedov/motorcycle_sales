using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace motorcycle_sales.Web.ViewModels;

public class AddOrEditRoleViewModel
{
    public string ?RoleId { get; set; }
    [DisplayName("Role name")]
    public string RoleName { get; set; }
}
