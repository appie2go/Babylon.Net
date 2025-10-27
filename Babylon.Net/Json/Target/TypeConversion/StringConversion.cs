using Newtonsoft.Json.Linq;

namespace Babylon.Net.Json.Target.TypeConversion;

public sealed class StringConverter : TypeConverter
{
    protected override Type TargetType { get; } = typeof(string);
    
    protected override object? ConvertJToken(JToken? value)
    {
        return value?.ToString();
    }
}