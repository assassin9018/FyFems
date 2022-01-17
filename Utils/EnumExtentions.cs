using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Utils;

public static class EnumExtentions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .FirstOrDefault()
                        ?.GetCustomAttribute<DisplayAttribute>()
                        ?.GetName() ?? "Не определено.";
    }
}
