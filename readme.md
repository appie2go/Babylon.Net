# Babylon.Net

Babylon.Net is a C# library for declarative object mapping. It allows you to describe how entities should be transformed without writing repetitive boilerplate code.

You simply define mappings, and Babylon.Net takes care of the rest. It accepts any object and maps it seamlessly to another.

Mappings can be declared using YAML, like this:

```yaml
fields:
  - source: id
    destination: $.userId
  - source: address
    destination: $.address
    fields: 
    - source: city
      destination: $.address.city
    - source: streetAddress
      destination: $.address.street
      converter: MyProject.Converts.StreetConverter
    - source: streetAddress
      destination: $.address.houseNo
      converter: MyProject.Converts.HouseNoConverter
```

Or defined programmatically:
```c#
var converter = new Converter([
    new MappingConfiguration
    {
        Source = "id",
        Destination = "$.userId",
        Fields = [
            new MappingConfiguration
            {
                Source = "city",
                Destination = "$.address.city"
            },
            new MappingConfiguration
            {
                Source = "streetAddress",
                Destination = "$.address.street",
                Converter = typeof(StreetConverter)
            },
            new MappingConfiguration
            {
                Source = "streetAddress",
                Destination = "$.address.houseNo",
                Converter = typeof(HouseNoConverter)
            }
        ]
    }
]);

converter.Convert(someObject);
```

## Vision

The long-term goal of Babylon.Net is to serve as a foundational component for event-driven architectures.
It will act as a lightweight bridge that propagates entities and events across distributed microservices.

Imagine a central layer that understands your data structure and routes messages intelligently, powered by simple YAML mapping definitions. Babylon.Net aims to make that possible. Cleanly, declaratively, and without coupling your services.

In essence, Babylon.Net will enable you to describe what should be forwarded to which service, leaving the how to the library itself.


## Getting started

Prerequisites:
- .NET SDK 9.0

To start building:
- Clone the repository
- Type dotnet restore
- Type dotnet build

## Contributing

Babylon.Net is just a concept now, and contributions from you are more than welcome. Whether you want to shape its design, add features, or fix issues, you can help build something meaningful.

You can:

- Open issues for ideas, improvements, or bugs you discover.
- Submit pull requests with new features, docs, or examples.

Let’s make data transformation cleaner, simpler, and more declarative—together.