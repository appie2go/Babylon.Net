using Newtonsoft.Json.Linq;

namespace Babylon.Net.Json.Converting;

internal class ArrayValueConverter : ValueConverter
{
    public override IEnumerable<Property> Convert(JToken value)
    {
        var arr = value.ToArray();
        foreach (var jToken in arr)
        {
            var converter = ValueConverterFactory.Create(jToken);
            var values = converter.Convert(jToken);
            foreach (var property in values)
            {
                yield return property;
            }
        }
    }
}