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

        var memberInfo = enumType.GetMember(description);
        if (memberInfo.Length > 0)
        {
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
                description = ((DescriptionAttribute)attrs[0]).Description;
        }

        // Use TryAdd() instead of Add() to avoid an ArgumentException when
        // the key exists. (It shouldn't exist because of the TryGetValue()
        // call above, but the exception is occurring nonetheless.)
        // This shouldn't affect the functionality of this method or the 
        // cache dictionary.
        // See https://github.com/gaepdit/airbranch-reports/issues/53
        EnumDescriptions.TryAdd(fullName, description);
        return description;
    }
}
