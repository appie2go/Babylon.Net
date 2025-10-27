namespace Babylon.Net.Json.Target.TypeConversion;

public sealed class TypeConversionException(string? value, Type type, Exception? innerException = null) 
    : Exception($"Failed to convert value `{value}` to type `{type.FullName}`", innerException)
{
    public string? Value { get; } = value;
    public Type Type { get; } = type;
}