using Newtonsoft.Json.Linq;

namespace Babylon.Net.Json.Converting;

internal abstract class ValueConverter
{
    public abstract IEnumerable<Property> Convert(JToken value);

    public IEnumerable<ValueConverter> GetChildren(JToken value)
    {
        return value
            .Children()
            .Select(ValueConverterFactory.Create);
    }
}