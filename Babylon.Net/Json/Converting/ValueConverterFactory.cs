using Newtonsoft.Json.Linq;

namespace Babylon.Net.Json.Converting;

internal static class ValueConverterFactory
{
    public static ValueConverter Create(JToken jObject)
    {
        return jObject.Type switch
        {
            JTokenType.Array => new ArrayValueConverter(),
            JTokenType.Property => new ComplexObjectValueConverter(),
            _ => new SimpleObjectValueConverter()
        };
    }
}