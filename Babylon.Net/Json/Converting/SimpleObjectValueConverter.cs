using Newtonsoft.Json.Linq;

namespace Babylon.Net.Json.Converting;

internal class SimpleObjectValueConverter : ValueConverter
{
    public override IEnumerable<Property> Convert(JToken value)
    {
        var stringValue = value.ToString();

        return
        [
            new Property
            {
                Path = $"$.{value.Path}",
                Value = stringValue
            }
        ];
    }
}