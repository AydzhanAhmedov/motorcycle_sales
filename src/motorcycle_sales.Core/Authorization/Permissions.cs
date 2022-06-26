using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motorcycle_sales.Core.Authorization;

public class Permissions
{
    public static List<string> GetAllPermisions()
    {
        return new List<string>()
        {
            "Permission.Advertisement.Edit",
            "Permission.Advertisement.Delete",
            "Permission.Advertisement.Approve"
        };
    }
}
