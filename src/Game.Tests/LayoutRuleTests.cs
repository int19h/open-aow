using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Game.Tests;

// The layout rule (issue 192): only a transient type that reads or writes a format (a file record,
// a native interop record, a wire message) has a fixed or explicit layout, and it carries
// [StoredLayout]. Game data uses natural layout and is mapped to such a type at the I/O boundary.
// The private layout census checks the overlay call sites (tools/rewrite/layout-census).
public sealed class LayoutRuleTests
{
    private static readonly string[] s_productAssemblies = ["Aow.Rtl", "Aow.Graphics", "Aow.Formats", "Aow.Game", "Aow.Platform"];

    // Structs that still have a layout without the marker. Each layout pull request removes the
    // entries it fixes, and the list is empty after LY-9 (the private layout-design.md, section 5).
    private static readonly HashSet<string> s_unmarkedLayouts =
    [
        // LY-8 and LY-9: the map coordinate.
        "Aow.Game.World.Map.MapCoordinate",
    ];

    // Fields of game types that hold a [StoredLayout] type. The property table is the transient
    // table of the storage streams, deleted with them in PE-4-a.
    private static readonly HashSet<string> s_heldStoredLayouts =
    [
        "Aow.Engine.PropertyTable._items",
    ];

    private static IEnumerable<Type> ProductTypes() =>
        s_productAssemblies.SelectMany(name => Assembly.Load(new AssemblyName(name)).GetTypes())
            .Where(type => !type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false) && !(type.FullName ?? type.Name).Contains('<'));

    private static string Name(Type type) => (type.FullName ?? type.Name).Replace('+', '.');

    private static bool IsStored(Type type) => type.IsDefined(typeof(StoredLayoutAttribute), inherit: false);

    internal static bool HasLayout(Type type)
    {
        if (!type.IsValueType || type.IsEnum || type.IsPrimitive || type.IsDefined(typeof(InlineArrayAttribute), inherit: false)) return false;
        if (type.StructLayoutAttribute is { } layout && (layout.Value == LayoutKind.Explicit || layout.Size != 0 || layout.Pack is not (0 or 8))) return true;
        const BindingFlags fields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        return type.GetFields(fields).Any(field => field.IsDefined(typeof(FieldOffsetAttribute), inherit: false));
    }

    private static Type ElementType(Type type)
    {
        while (type.HasElementType) type = type.GetElementType()!;
        return type;
    }

    [Fact]
    public void EveryLayoutTypeIsMarked()
    {
        var unmarked = ProductTypes().Where(type => HasLayout(type) && !IsStored(type)).Select(Name).ToHashSet();
        Assert.Empty(unmarked.Except(s_unmarkedLayouts).Order(StringComparer.Ordinal));
        // An entry whose type has lost its layout leaves the list.
        Assert.Empty(s_unmarkedLayouts.Except(unmarked).Order(StringComparer.Ordinal));
    }

    [Fact]
    public void MarkedTypesStayAtTheBoundary()
    {
        const BindingFlags all = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        var held = ProductTypes()
            .Where(type => !(type.IsValueType && IsStored(type)))
            .SelectMany(type => type.GetFields(all).Where(field => IsStored(ElementType(field.FieldType))).Select(field => Name(type) + "." + field.Name))
            .ToHashSet();
        Assert.Empty(held.Except(s_heldStoredLayouts).Order(StringComparer.Ordinal));
        Assert.Empty(s_heldStoredLayouts.Except(held).Order(StringComparer.Ordinal));
    }

    [Fact]
    public void TheGuardSeesALayout()
    {
        Assert.True(HasLayout(typeof(PlantedExplicit)));
        Assert.True(HasLayout(typeof(PlantedPacked)));
        Assert.False(HasLayout(typeof(PlantedNatural)));
        Assert.True(IsStored(typeof(Aow.Game.World.Map.HsmHeader)));
    }

    [StructLayout(LayoutKind.Explicit)]
    private readonly struct PlantedExplicit(int value)
    {
        [FieldOffset(0)] private readonly int _value = value;

        public int Value => _value;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private readonly record struct PlantedPacked(byte First, int Second);

    private readonly record struct PlantedNatural(byte First, int Second);
}
