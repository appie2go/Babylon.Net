using Newtonsoft.Json.Linq;

namespace Babylon.Net.Json.Target.TypeConversion;

public abstract class TypeConverter
{
    public object? Convert(JToken? value)
    {
        try
        {
            return ConvertJToken(value);
        }
        catch (Exception e)
        {
            throw new TypeConversionException(value?.ToString(), TargetType, e);
        }
    }
    
    protected abstract Type TargetType { get; }
    
    protected abstract object? ConvertJToken(JToken? value);
}