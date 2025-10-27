using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Babylon.Net.Tests;

public class PropertyBagTests
{
    [Fact]
    public void ShouldMapProperties()
    {
        // Arrange
        var payload = new Structure1 { Value = "test" };

        // Act
        var node = PropertyBag.Create(payload);
        var actual = node.Serialize() as Dictionary<string, object?>;
        
        // Assert
        actual!["Value"].Should().Be("test");
    }
    
    [Fact]
    public void ShouldMapComplexProperties()
    {
        // Arrange
        var expected = Guid.NewGuid();
        var payload = new Structure1
        {
            Child = new SomeObject { Id = expected, Number = 1 },
        };

        // Act
        var node = PropertyBag.Create(payload);
        var actual = node.Serialize() as Dictionary<string, object?>;
        
        // Assert
        var child = actual!["Child"] as JObject;
        child.Should().NotBeNull();
        child.Property("Id")!.Value.ToString().Should().Be(expected.ToString());
    }
    
    [Fact]
    public void ShouldMapArrays()
    {
        // Arrange
        var payload = new Structure1
        {
            Children =
            [
                new Bar { Ids = [ Guid.Empty, Guid.Empty ] }, 
                new Bar { Ids = [] }
            ]
        };

        // Act
        var node = PropertyBag.Create(payload);
        var actual = node.Serialize() as Dictionary<string, object?>;
        
        // Assert
        var child = actual!["Children"] as JArray;
        child.Should().NotBeNull();
    }
}

public class Structure1
{
    public string? Value { get; set; }
    
    public SomeObject? Child { get; set; }
    
    public Bar[] Children { get; set; } = [];
}

public class SomeObject
{
    public Guid Id { get; set; }
    
    public int Number { get; set; }
}

public class Bar
{
    public Guid[] Ids { get; set; } = [];
}