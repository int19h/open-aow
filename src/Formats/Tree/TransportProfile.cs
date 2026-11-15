using System.Globalization;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Tree;

/// <summary>
/// The profile of an original image (persistence-tree.md 16 and 17). Packets and own documents
/// share <see cref="FormatSchema"/>. <see cref="Name"/> names the legacy packet image. Every
/// original image refuses a member it cannot carry and names the path.
/// <c>docs/specs/networking.md</c> replaces the adapter.
/// </summary>
public static class TransportProfile
{
    /// <summary>The profile name of a packet image. <c>FileProfile.All</c> holds the disk containers.</summary>
    public const string Name = "packet";

    /// <summary>
    /// The first stored id an original image does not carry. Port state runs from here through
    /// <c>0x4FFFFFFF</c>. Mod state starts at <c>0x50000000</c>.
    /// </summary>
    public const int PortMinimum = 0x40000000;

    /// <summary>
    /// Whether <paramref name="profile"/> carries <paramref name="property"/>. Every original image
    /// carries a member whose stored ids sit below <see cref="PortMinimum"/>.
    /// <paramref name="profile"/> names the use. The disk image uses the engine name and the packet
    /// uses <see cref="Name"/>.
    /// </summary>
    public static bool Carries(string profile, SchemaProperty property)
    {
        ArgumentNullException.ThrowIfNull(profile);
        return ReservedId(property) is null;
    }

    /// <summary>
    /// The refusal of a present member <see cref="Carries"/> rejects. The value is the reserved id.
    /// </summary>
    public static ConversionException Unrepresentable(EncodeContext context, SchemaClass classValue, SchemaProperty property, DocumentNodePath path)
    {
        int id = ReservedId(property) ?? PortMinimum;
        return context.Fail(
            Codes.Unrepresentable,
            message: "The original image cannot carry " + classValue.Name + "." + property.Name + ".",
            at: null,
            path: path,
            value: id.ToString(CultureInfo.InvariantCulture),
            constraint: "a stored id below 0x" + PortMinimum.ToString("X8", provider: CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// The stored id that puts <paramref name="property"/> in the port range or the mod range,
    /// or null when every stored id sits below <see cref="PortMinimum"/>.
    /// </summary>
    internal static int? ReservedId(SchemaProperty property)
    {
        if (Reserved(property.Id) is int id) return id;
        if (Reserved(property.DataId) is int dataId) return dataId;
        if (Reserved(property.DynamicMinimum) is int dynamic) return dynamic;
        return null;
    }

    private static int? Reserved(int? id) => id is int value && value >= PortMinimum ? value : null;
}
