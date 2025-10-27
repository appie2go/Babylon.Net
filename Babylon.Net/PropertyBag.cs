using Newtonsoft.Json.Linq;

namespace Babylon.Net;

internal class PropertyBag : Dictionary<string, object?>
{
    public static PropertyBag Create(object payload)
    {
        var result = new PropertyBag();
        var jObject = JObject.FromObject(payload);
        Extract(jObject, result);
        return result;
    }

    private static void Extract(JObject jObject, PropertyBag result)
    {
        foreach (var property in jObject.Properties())
        {
            result.Add($"$.{property.Name}", property.Value);
        }

        foreach (var property in jObject.Children<JObject>())
        {
            Extract(property, result);
        }
    }

    public object Serialize()
    {
        JObject jObject = new();

        foreach (var (key, value) in this)
        {
            if (value == null)
            {
                continue;
            }
            
            var lastDot = key.LastIndexOf('.');
            if (lastDot == -1)
            {
                jObject.Add(key, JToken.FromObject(value));;
                continue;
            }
            
            var path = key.Substring(0, lastDot);
            var property = key.Substring(lastDot + 1);

            var field = jObject.SelectTokens(path).FirstOrDefault() ?? new JObject();
            var destination = (JObject)field;
            destination[property] = JToken.FromObject(value);
        }

        return jObject.ToObject<Dictionary<string, object?>>() ?? new Dictionary<string, object?>();
    }

    public void SetValue(string propertyDestination, object propertyValue)
    {
        var path = propertyDestination
            .TrimStart('$')
            .Split(".", StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, object?> current = this;
        var limit = path.Length > 1 
            ? path.Length - 1 
            : 0;
        
        for (var i = 0; i < limit; i++)
        {
            var indexer = path[i];
            if (!current.ContainsKey(indexer))
            {
                current[indexer] = new Dictionary<string, object?>();
            }

            var value = current[indexer] as Dictionary<string, object?>;
            if (value == null)
            {
                return;
            }
            
            current = value;
        }

        current[path.Last()] = propertyValue;
    }
}