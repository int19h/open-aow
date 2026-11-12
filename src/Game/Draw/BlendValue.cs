#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Aow.Game.Draw;

// The blend slot of an image's show info (ImageLib.pas TShowInfo): a percentage into the blend
// table collection of the blend mode, or the blend table itself for bmUser. The two cases are
// closed by the record itself. A nil table is the union's null, as the original slot holds 0 for it.
[Union]
public readonly struct BlendValue : IUnion
{
    private enum Case : byte { None, Percentage, Table }

    private readonly int _percentage;
    private readonly BlendTable? _table;
    private readonly Case _kind;

    public BlendValue(int value) { _percentage = value; _kind = Case.Percentage; }

    public BlendValue(BlendTable? value) { _table = value; _kind = value is null ? Case.None : Case.Table; }

    // The compiler prefers a user-defined conversion to a union conversion. One operator per case
    // keeps the case chosen by the source type: a byte widens to Percentage, a table converts to
    // Table.
    public static implicit operator BlendValue(int value) => new(value);
    public static implicit operator BlendValue(BlendTable? value) => new(value);

    // Patterns read the cases through TryGetValue. Value boxes and serves only reflection.
    public object? Value => _kind switch
    {
        Case.Percentage => _percentage,
        Case.Table => _table,
        _ => null,
    };

    public bool HasValue => _kind != Case.None;

    public bool TryGetValue(out int value)
    {
        value = _kind == Case.Percentage ? _percentage : 0;
        return _kind == Case.Percentage;
    }

    public bool TryGetValue([MaybeNullWhen(false)] out BlendTable value)
    {
        value = _kind == Case.Table ? _table : null;
        return _kind == Case.Table;
    }

    // The percentage case. An empty slot reads 0, as the original word does. A table has no
    // percentage view: the original read its address, which no blend mode but bmUser uses.
    public int Percentage => _kind switch
    {
        Case.Percentage => _percentage,
        Case.None => 0,
        _ => throw new InvalidCastException("Blend value holds a table, not a percentage"),
    };
}
