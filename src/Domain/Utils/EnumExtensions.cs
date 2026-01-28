using System.ComponentModel;
using System.Reflection;

namespace Domain.Utils;

/// <summary>
/// Enumeration type extension methods.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the enum description.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>
    /// Uses <see cref="DescriptionAttribute"/> if exists. Otherwise, uses the standard string representation.
    /// </returns>
    public static string GetDescription<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        var enumString = enumValue.ToString();
        var type = enumValue.GetType();
        var memInfo = type.GetMember(enumString)[0];
        var attributes = memInfo.GetCustomAttributes<DescriptionAttribute>(false);
        return attributes.FirstOrDefault()?.Description ?? enumString;
    }
}
