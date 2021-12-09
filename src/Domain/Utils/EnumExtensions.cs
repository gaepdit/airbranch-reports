using System.ComponentModel;

namespace Domain.Utils;

public static class EnumExtensions
{
    private readonly static Dictionary<string, string> enumDescriptions = new();

    public static string GetDescription(this Enum e)
    {
        var enumType = e.GetType();
        var description = e.ToString();
        var fullName = string.Join('.', enumType.FullName, description);

        if (enumDescriptions.TryGetValue(fullName, out string? cachedDescription))
            return cachedDescription;

        var memberInfo = enumType.GetMember(e.ToString());
        if (memberInfo != null && memberInfo.Length > 0)
        {
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs != null && attrs.Length > 0)
                description = ((DescriptionAttribute)attrs[0]).Description;
        }

        enumDescriptions.Add(fullName, description);
        return description;
    }
}
