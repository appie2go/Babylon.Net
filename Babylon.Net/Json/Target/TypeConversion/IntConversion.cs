using Newtonsoft.Json.Linq;

namespace Babylon.Net.Json.Target.TypeConversion;

public sealed class IntConversion : TypeConverter
{
    protected override Type TargetType { get; } = typeof(int);
    
    protected override object? ConvertJToken(JToken? value)
    {
        if (value?.Type == JTokenType.Null)
        {
            return 0;
        }
        
        return value?.ToObject<int>();
    }
}