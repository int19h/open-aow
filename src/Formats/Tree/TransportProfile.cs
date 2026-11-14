using Aow.Formats.Schema;

namespace Aow.Formats.Tree;

/// <summary>
/// The packet use of the disk schema (persistence-tree.md 16). Packets and own documents share
/// <see cref="FormatSchema"/>. This profile names the legacy packet image. A member that image
/// cannot carry is refused with its path. <c>docs/specs/networking.md</c> replaces the adapter.
/// </summary>
public static class TransportProfile
{
    /// <summary>The profile name of a packet image. <c>FileProfile.All</c> holds the disk containers.</summary>
    public const string Name = "packet";

    /// <summary>
    /// The first stored id the legacy packet does not carry. Port state runs from here through
    /// <c>0x4FFFFFFF</c>. Mod state starts at <c>0x50000000</c>.
    /// </summary>
    public const int PortMinimum = 0x40000000;

    /// <summary>
    /// Whether <paramref name="profile"/> carries <paramref name="property"/>. A profile other
    /// than <see cref="Name"/> carries every member the schema stores. The packet profile
    /// carries a member whose stored ids sit below <see cref="PortMinimum"/>.
    /// </summary>
    public static bool Carries(string profile, SchemaProperty property) =>
        profile != Name || ReservedId(property) is null;

    /// <summary>
    /// The stored id that puts <paramref name="property"/> in the port range or the mod range,
    /// or null when every stored id sits below <see cref="PortMinimum"/>.
    /// </summary>
    public static int? ReservedId(SchemaProperty property)
    {
        if (Reserved(property.Id) is int id) return id;
        if (Reserved(property.DataId) is int dataId) return dataId;
        if (Reserved(property.DynamicMinimum) is int dynamic) return dynamic;
        return null;
    }

    private static int? Reserved(int? id) => id is int value && value >= PortMinimum ? value : null;
}
