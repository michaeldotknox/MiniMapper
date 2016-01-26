# MiniMapper
A small framework for mapping the properties from one object to another

[![Build status](https://ci.appveyor.com/api/projects/status/rlj2jcytaro3b2lt?svg=true)](https://ci.appveyor.com/project/michaeldotknox/minimapper)
[![Build status](https://ci.appveyor.com/api/projects/status/rlj2jcytaro3b2lt/branch/master?svg=true)](https://ci.appveyor.com/project/michaeldotknox/minimapper/branch/master)

## What is MiniMapper?
MiniMapper is a small framework for mapping the properties from one object to the properties of another object.  It is lightweight and is designed to be lightweight and as performant as possible.  As
such, it does not have a large feature set.  Attributes are used to define the properties that are to be mapped and their destination properties.  This can be done in all mappings, or
if you want, it can be done for a specific destination type, with each type having its own set of destination property names.  New in version 1.1, MiniMapper will map properties with the same name without using attributes.

## When would you use MiniMapper?
When I write APIs, I like to make data models that are tied closely to the database.  Typically, I name the properties in these data models the same as the fields in the database, or at least the same 
as the field names in the stored procedure.  It is also common for those data models to have attributes on the properties for such things as search criteria or stored procedure parameter names.

On the other end of the spectrum, I create data contracts which will be sent to the client.  These data contracts typically have field names that are more user friendly, or which may be translated to 
name that is more consistent with the actual use.  Very often these data models and data contracts are an easy map; the field on one object equals the field on the other object without any translation 
or type casting.  I don't want to type all of that.

MiniMapper makes it easy to map the properties of an object to the properties of another object without all of the typing.  Simply add some attributes to the properties on a class, create the map 
in MiniMapper during initialization, and then map the two objects.

## Examples

### Adding attributes to the properties of a class
You can map the properties of one object to the properties of any object by simply adding an attribute and specifying the name of the destination property:
```
public class SourceClass
{
    [MapsTo("DestinationProperty")]
    public string SourceProperty { get; set; }
}

public class DestinationClass
{
    public string DestinationProperty { get; set; }
}
```

You can also specify that a property maps to another property depending on the destination type.  For instance:

```
public class SourceClass
{
    [MapsTo("DestinationProperty1", DestinationType = typeof(DestinationClass1)]
    [MapsTo("DestinationProperty2", DestinationType = typeof(DestinationClass2)]
    public string SourceProperty { get; set; }
}

public class DestinationClass1
{
    public string DestinationProperty1 { get; set; }
}

public class DestinationClass2
{
    public string DestinationProperty2 { get; set; }
}
```

### Creating the maps
After you've added the attributes to your classes, you just need to create the maps during initialization.

```
using MiniMapper.Core;

namespace MiniMapper.Example
{
    class program
    {
        static void Main(string[] args)
        {
            Mapper.CreateMap<SourceClass, DestinationClass>();
        }
    }
}
```

### Mapping one object to another
When you need to map one object to another, you simply do this:

```
var source = new SourceClass { SourceProperty = "Value" };
var destination = new DestinationClass();

destination = Mapper.Map<SourceClass, DestinationClass>(source, destination); 
```

`destination` will have the `DestinationProperty` set to "Value"

If you don't have any initialization to do for `DestinationClass`, you can use this simplified syntax:

```
var source = new SourceClass { SourceProperty = "Value" };

var destination = Mapper.Map<SourceClass, DestinationClass>(source); 
```

In this case, MiniMapper will create the DestinationClass object for you.

### Mapping a list of objects to another list of objects
MiniMapper will now map a list of objects to a list of destination objects.  Any list or array will work.  Just define your classes as you normally would:

```
public class SourceClass
{
    [MapsTo("DestinationProperty")]
    public string SourceProperty { get; set; }
}

public class DestinationClass
{
    public string DestinationProperty { get; set; }
}
```

Then create the map:

```
using MiniMapper.Core;

{
    namespace MiniMapper.Example
    {
        class Program
        {
            static void Main(string[] args)
            {
                Mapper.CreateMap<SourceClass, DestinationClass);
            }
        }
    }
}
```

And, finally, map the source list to a destination list:

```
var sourceList = GetSourceList();

var destinationList = Mapper.Map<SourceClass, DestinationClass>(sourceList);
```

## Getting MiniMapper
MiniMapper is available on NuGet as two packages:
* MiniMapper.Attributes - This contains just the attributes and should be used in the project containing the classes you want to map from
* MiniMapper.Core - This contains everything that you need to map and references MiniMapper.Attributes
