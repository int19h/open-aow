#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Aow.Rtl;

namespace Aow.Units.GFXE;

// The blend slot of an image's show info (ImageLib.pas TShowInfo): a percentage into the blend
// table collection of the blend mode, or the blend table itself for bmUser. The two cases are
// closed by the record itself. A nil table is the union's null, as the original slot holds 0 for it.
[Union]
public readonly struct BlendValue : IUnion
{
    private enum Case : byte { None, Percentage, Table }

    private readonly int percentage;
    private readonly TBlendTable? table;
    private readonly Case kind;

    public BlendValue(int value) { percentage = value; kind = Case.Percentage; }

    public BlendValue(TBlendTable? value) { table = value; kind = value is null ? Case.None : Case.Table; }

    // The compiler prefers a user-defined conversion to a union conversion. One operator per case
    // keeps the case chosen by the source type: a byte widens to Percentage, a table converts to
    // Table.
    public static implicit operator BlendValue(int value) => new(value);
    public static implicit operator BlendValue(TBlendTable? value) => new(value);

    // Patterns read the cases through TryGetValue. Value boxes and serves only reflection.
    public object? Value => kind switch
    {
        Case.Percentage => percentage,
        Case.Table => table,
        _ => null,
    };

    public bool HasValue => kind != Case.None;

    public bool TryGetValue(out int value)
    {
        value = kind == Case.Percentage ? percentage : 0;
        return kind == Case.Percentage;
    }

    public bool TryGetValue([MaybeNullWhen(false)] out TBlendTable value)
    {
        value = kind == Case.Table ? table : null;
        return kind == Case.Table;
    }

    // The percentage case. An empty slot reads 0, as the original word does. A table has no
    // percentage view: the original read its address, which no blend mode but bmUser uses.
    public int Percentage => kind switch
    {
        Case.Percentage => percentage,
        Case.None => 0,
        _ => throw new EInvalidCast("Blend value holds a table, not a percentage"),
    };
}
