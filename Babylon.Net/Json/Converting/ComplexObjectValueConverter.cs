using Newtonsoft.Json.Linq;

namespace Babylon.Net.Json.Converting;

internal class ComplexObjectValueConverter : ValueConverter
{
    public override IEnumerable<Property> Convert(JToken value)
    {
        foreach (var item in value)
        {
            var converter = ValueConverterFactory.Create(item);
            var values = converter.Convert(item);
            foreach (var property in values)
            {
                yield return property;
            }
        }
    }
}