# MiniMapper
A small framework for mapping the properties from one object to another

[![Build status](https://ci.appveyor.com/api/projects/status/rlj2jcytaro3b2lt?svg=true)](https://ci.appveyor.com/project/michaeldotknox/minimapper)
[![Build status](https://ci.appveyor.com/api/projects/status/rlj2jcytaro3b2lt/branch/master?svg=true)](https://ci.appveyor.com/project/michaeldotknox/minimapper/branch/master)

## What is MiniMapper?
MiniMapper is a small framework for mapping the properties from one object to the properties of another object.  It is lightweight and is designed to be lightweight and as performant as possible.  As
such, it does not have a large feature set.  Attributes are used to define the properties that are to be mapped and their destination properties.  This can be done in all mappings, or
if you want, it can be done for a specific destination type, with each type having its own set of destination property names.

## When would you use MiniMapper?
When I write APIs, I like to make data models that are tied closely to the database.  Typically, I name the properties in these data models the same as the fields in the database, or at least the same 
as the field names in the stored procedure.  It is also common for those data models to have attributes on the properties for such things as search criteria or stored procedure parameter names.

On the other end of the spectrum, I create data contracts which will be sent to the client.  These data contracts typically have field names that are more user friendly, or which may be translated to 
name that is more consistent with the actual use.  Very often these data models and data contracts are an easy map; the field on one object equals the field on the other object without any translation 
or type casting.  I don't want to type all of that.

MiniMapper makes it easy to map the properties of an object to the properties of another object without all of the typing.  Simply add some attributes to the properties on a class, create the map 
in MiniMapper during initialization, and then map the two objects.
  