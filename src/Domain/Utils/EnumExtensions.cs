using System.ComponentModel;

namespace Domain.Utils;

public static class EnumExtensions
{
    private static readonly Dictionary<string, string> EnumDescriptions = new();

    public static string GetDescription(this Enum e)
    {
        var enumType = e.GetType();
        var description = e.ToString();
        var fullName = string.Join('.', enumType.FullName, description);

        if (EnumDescriptions.TryGetValue(fullName, out var cachedDescription))
            return cachedDescription;

        var memberInfo = enumType.GetMember(e.ToString());
        if (memberInfo.Length > 0)
        {
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
                description = ((DescriptionAttribute)attrs[0]).Description;
        }

        EnumDescriptions.Add(fullName, description);
        return description;
    }
}
