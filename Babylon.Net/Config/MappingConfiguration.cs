namespace Babylon.Net.Config;

public class MappingConfiguration
{
    public string Source { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string? Converter { get; set; }    
    public List<MappingConfiguration> Fields { get; set; } = new();
}