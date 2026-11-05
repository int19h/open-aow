using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Aow.Rtl;

// SYSTEM.PAS vtPointer: the raw address a %p conversion prints.
public readonly record struct PointerArg(uint Value);

// SYSTEM.PAS array of const, the arguments of Format: vtInteger, vtBoolean, vtChar, vtExtended,
// vtAnsiString, vtObject and vtPointer. The cases are closed by SysUtils.Format itself. A nil
// object is the union's null, as Format treats it like any argument of the wrong kind.
[Union]
public readonly struct FormatArg : IUnion
{
    private enum Case : byte { None, Integer, Boolean, Char, Extended, String, Object, Pointer }

    private readonly long bits;
    private readonly object? reference;
    private readonly Case kind;

    public FormatArg(int value) { bits = value; kind = Case.Integer; }
    public FormatArg(bool value) { bits = value ? 1 : 0; kind = Case.Boolean; }
    public FormatArg(char value) { bits = value; kind = Case.Char; }
    public FormatArg(double value) { bits = BitConverter.DoubleToInt64Bits(value); kind = Case.Extended; }
    public FormatArg(string value) { reference = Rtl.RequireString(value); kind = Case.String; }
    public FormatArg(TObject? value) { reference = value; kind = value is null ? Case.None : Case.Object; }
    public FormatArg(PointerArg value) { bits = value.Value; kind = Case.Pointer; }

    // The compiler prefers a user-defined conversion to a union conversion. Declaring one per
    // case keeps the choice the same as with one operator per source type: a constant 1 stays an
    // Integer, a short widens to Integer, a uint or a long widens to Extended.
    public static implicit operator FormatArg(int value) => new(value);
    // A Delphi Char is still a byte and enters through the frozen map. A string element is a char.
    public static implicit operator FormatArg(byte value) => new(Cp1252.Char(value));
    public static implicit operator FormatArg(char value) => new(value);
    public static implicit operator FormatArg(bool value) => new(value);
    public static implicit operator FormatArg(double value) => new(value);
    public static implicit operator FormatArg(string? value) => new(value!);
    public static implicit operator FormatArg(TObject? value) => new(value);
    public static implicit operator FormatArg(PointerArg value) => new(value);

    // Patterns read the cases through TryGetValue. Value boxes and serves only reflection.
    public object? Value => kind switch
    {
        Case.Integer => (int)bits,
        Case.Boolean => bits != 0,
        Case.Char => (char)bits,
        Case.Extended => BitConverter.Int64BitsToDouble(bits),
        Case.String or Case.Object => reference,
        Case.Pointer => new PointerArg((uint)bits),
        _ => null,
    };

    public bool HasValue => kind != Case.None;

    public bool TryGetValue(out int value)
    {
        value = kind == Case.Integer ? (int)bits : 0;
        return kind == Case.Integer;
    }

    public bool TryGetValue(out bool value)
    {
        value = kind == Case.Boolean && bits != 0;
        return kind == Case.Boolean;
    }

    public bool TryGetValue(out char value)
    {
        value = kind == Case.Char ? (char)bits : '\0';
        return kind == Case.Char;
    }

    public bool TryGetValue(out double value)
    {
        value = kind == Case.Extended ? BitConverter.Int64BitsToDouble(bits) : 0;
        return kind == Case.Extended;
    }

    public bool TryGetValue([MaybeNullWhen(false)] out string value)
    {
        value = kind == Case.String ? (string)reference! : null;
        return kind == Case.String;
    }

    public bool TryGetValue([MaybeNullWhen(false)] out TObject value)
    {
        value = kind == Case.Object ? (TObject)reference! : null;
        return kind == Case.Object;
    }

    public bool TryGetValue(out PointerArg value)
    {
        value = kind == Case.Pointer ? new PointerArg((uint)bits) : default;
        return kind == Case.Pointer;
    }
}
