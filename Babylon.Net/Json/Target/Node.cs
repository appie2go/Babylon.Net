namespace Babylon.Net.Json.Target;

internal class Node(string destination, object value)
{
    public string Destination { get; } = destination;
    public object Value { get; } = value;
}