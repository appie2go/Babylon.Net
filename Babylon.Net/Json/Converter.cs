using Babylon.Net.Config;
using Babylon.Net.Json.Converting;
using Babylon.Net.Json.Target;
using Newtonsoft.Json.Linq;

namespace Babylon.Net.Json;

public class Converter : IConverter
{
    private readonly IList<MappingConfiguration> _configuration = new List<MappingConfiguration>();
    
    public Converter(IList<MappingConfiguration> configuration)
    {
        foreach (var config in configuration)
        {
            config.Source = string.IsNullOrEmpty(config.Source)
                ? "$."
                : config.Source;
            
            config.Destination = string.IsNullOrEmpty(config.Destination)
                ? "$."
                : config.Destination;
            
            AppendSourceAndDestinationPath(config);
            _configuration.Add(config);       
        }

        return;

        void AppendSourceAndDestinationPath(MappingConfiguration config)
        {
            foreach (var field in config.Fields)
            {
                var destination = string.IsNullOrEmpty(config.Destination)
                    ? string.Empty
                    : $"{config.Destination}.";

                field.Destination = $"{destination}{field.Destination}";
                field.Source = $"{config.Source}.{field.Source}";
                
                AppendSourceAndDestinationPath(field);
            }
        }
    }
    
    public object? Convert(object? value)
    {
        if (value == null)
        {
            return null;
        }
        
        var properties = new List<Property>();
        var jObject = JObject.FromObject(value);
        foreach (var jToken in jObject)
        {
            var values = Convert(jToken.Value);
            properties.AddRange(values);
        }

        var mappedValues = ApplyMapping("$.", _configuration, properties);
        var result = new PropertyBag();
        foreach (var property in mappedValues)
        {
            result.SetValue(property.Destination, property.Value);       
        }
        
        return result.Serialize();
    }

    private static IList<Node> ApplyMapping(string path,
        IEnumerable<MappingConfiguration> mappings, 
        List<Property> properties)
    {
        var result = new List<Node>();
        
        foreach (var map in mappings)
        {
            var items = properties
                .Where(p => !map.Fields.Any() && p.Path == map.Source)
                .ToList()
                .Select(x => new Node(map.Destination, x.Value))
                .ToList();
            
            result.AddRange(items);

            if (!map.Fields.Any())
            {
                continue;
            }
            
            var moreItems = ApplyMapping($"{path}.{map.Source}", map.Fields, properties);
            result.AddRange(moreItems);
        }

        return result;
    }
    
    private static IEnumerable<Property> Convert(JToken? value)
    {
        if (value == null)
        {
            return [];
        }
        
        var converter = new SimpleObjectValueConverter();

        var properties = converter
            .Convert(value)
            .ToList();
        
        var nestedProperties = converter
            .GetChildren(value)
            .SelectMany(c => c.Convert(value));

        properties.AddRange(nestedProperties);
        return properties;
    }
}