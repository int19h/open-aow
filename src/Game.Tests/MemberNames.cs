using System.Reflection;

namespace Aow.Game.Tests;

// The state walkers read fields by reflection. An auto-property keeps its value in a backing field named
// <Name>k__BackingField, and the walkers record it under the property name, so a field that becomes a property
// keeps its recorded name.
internal static class MemberNames
{
    private const BindingFlags Instance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    internal static string Of(FieldInfo field) =>
        field.Name is ['<', .. var rest] && rest.IndexOf(">k__BackingField", StringComparison.Ordinal) is int end and >= 0
            ? rest[..end]
            : field.Name;

    // The field of that name, or the backing field of the auto-property of that name, in the type or its bases.
    internal static FieldInfo? Field(Type? type, string name, BindingFlags flags = Instance)
    {
        for (; type is not null; type = type.BaseType)
            if ((type.GetField(name, flags | BindingFlags.DeclaredOnly) ?? type.GetField("<" + name + ">k__BackingField", flags | BindingFlags.DeclaredOnly)) is { } field)
                return field;
        return null;
    }
}
