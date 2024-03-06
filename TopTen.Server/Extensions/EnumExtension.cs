using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TopTen.Server.Extensions
{
    public static class EnumExtension
    {
        public static string GetEnumDisplayName(this Enum enumType)
        {
            return enumType.GetType().GetMember(enumType.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .Name;
        }
    }
}
