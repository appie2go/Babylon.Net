using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Babylon.Net;


[DebuggerDisplay("{Path} = {Value}")]
internal class Property
{
    public string Path { get; set; } = string.Empty;

    public JToken Value { get; set; } = null!;
}