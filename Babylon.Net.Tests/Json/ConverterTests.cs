using Babylon.Net.Config;
using Babylon.Net.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Babylon.Net.Tests.Json;

public class ConverterTests
{
    [Fact]
    public void ShouldConvert()
    {
        // Arrange
        var payload = new Structure1
        {
            Value = "test",
            Child = new SomeObject
            {
                Id = Guid.NewGuid()
            }
        };

        var sut = new Converter([
            new MappingConfiguration
            {
                Source = "$.Value",
                Destination = "$.Banana"
            }
        ]);

        // Act
        var actual = sut.Convert(payload) as Dictionary<string, object?>;
        
        // Assert
        actual!["Banana"].Should().Be("test");
    }
    
    [Fact]
    public void WhenNested_ShouldConvert()
    {
        // Arrange
        var expected = Guid.NewGuid();
        var payload = new Structure1
        {
            Child = new SomeObject
            {
                Id = expected, 
                Number = 3
            }
        };

        var sut = new Converter([
            new MappingConfiguration
            {
                Source = "$.Child",
                Destination = "$.Foo.Bar",
                Fields = [
                    new MappingConfiguration
                    {
                        Source = "Id",
                        Destination = "Banana"
                    },
                    new MappingConfiguration
                    {
                        Source = "Number",
                        Destination = "Apple"
                    }
                ]
            }
        ]);

        // Act
        var actual = sut.Convert(payload) as Dictionary<string, object?>;
        
        // Assert
        var foo = actual!["Foo"] as JObject;
        
        var banana = foo!.SelectToken("$.Bar.Banana")!.ToString();
        banana.Should().Be(expected.ToString());
        
        var apple = foo!.SelectToken("$.Bar.Apple")!.ToString();
        apple.Should().Be("3");
        
        var json = JsonConvert.SerializeObject(actual);
    }
}