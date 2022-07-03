using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Core.Attributes;

public class ColorAttribute : Attribute
{
    public string Color { get; }
    public ColorAttribute(string color)
    {
        Color = color;
    }
}
public static class AdvertisementStatusExtension
{
    private static T GetAttribute<T>(this AdvertisementStatus status)
        where T : System.Attribute
    {
        return (status.GetType().GetMember(Enum.GetName(status.GetType(), status))[0].GetCustomAttributes(typeof(T), inherit: false)[0] as T);
    }
    public static string GetColor(this AdvertisementStatus status)
    {
        return status.GetAttribute<ColorAttribute>().Color;
    }
}
