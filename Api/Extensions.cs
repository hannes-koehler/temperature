using Microsoft.Extensions.Configuration;

internal static class Extensions
{
    internal static T? GetValue<T>(
        this IConfigurationRoot cfg,
        string key,
        T defaultValue)
    {
        var sValue = cfg[key];
        if (sValue != null)
        {
            var ret = Convert.ChangeType(sValue, typeof(T));
            return (T)ret;
        }
        return defaultValue;
    }
}