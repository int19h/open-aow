using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Aow.Rtl;

// A slot of an engine message (Engine.pas TEMsg, HSEngine.pas TMapFieldMsg): a 32-bit word that
// holds either an integer or an object reference. The two cases are closed by the message
// protocol itself. A nil reference is the union's null, as the original slot holds 0 for it.
[Union]
public readonly struct MsgArg : IUnion, IEquatable<MsgArg>
{
    private enum Case : byte { None, Integer, Object }

    private readonly int bits;
    private readonly object? reference;
    private readonly Case kind;

    public MsgArg(int value) { bits = value; kind = Case.Integer; }

    public MsgArg(object? value) { reference = value; kind = value is null ? Case.None : Case.Object; }

    // The compiler prefers a user-defined conversion to a union conversion. The integer operator
    // keeps a byte or a short on the integer case. A conversion from object is illegal, so the
    // reference case is the constructor.
    public static implicit operator MsgArg(int value) => new(value);

    // Patterns read the cases through TryGetValue. Value boxes and serves only reflection.
    public object? Value => kind switch
    {
        Case.Integer => bits,
        Case.Object => reference,
        _ => null,
    };

    public bool HasValue => kind != Case.None;

    public bool TryGetValue(out int value)
    {
        value = kind == Case.Integer ? bits : 0;
        return kind == Case.Integer;
    }

    public bool TryGetValue([MaybeNullWhen(false)] out object value)
    {
        value = kind == Case.Object ? reference : null;
        return kind == Case.Object;
    }

    // The integer case. An empty slot reads 0, as the original word does. A reference has no
    // integer view: the original read its address, which no reader of the port uses.
    public int Int => kind switch
    {
        Case.Integer => bits,
        Case.None => 0,
        _ => throw new InvalidCastException("Message argument holds a reference, not an integer"),
    };

    // The reference case as T, null when the slot holds an integer, nothing or another type.
    public T? As<T>() where T : class => reference as T;

    // Equality is the equality of the original word. An integer equals an integer of the same
    // value, a reference equals the same object, and the empty slot, a nil reference and the
    // integer 0 are all the word 0. A nonzero integer never equals a reference.
    public bool Equals(MsgArg other)
    {
        if (IsZero) return other.IsZero;
        if (kind != other.kind) return false;
        return kind == Case.Integer ? bits == other.bits : ReferenceEquals(reference, other.reference);
    }

    private bool IsZero => kind == Case.None || (kind == Case.Integer && bits == 0);

    // A type pattern on this union builds the reference case from any object, so the override unboxes.
    public override bool Equals(object? obj) => obj is not null && obj.GetType() == typeof(MsgArg) && Equals((MsgArg)obj);

    public override int GetHashCode() => IsZero ? 0 : kind == Case.Integer ? bits : RuntimeHelpers.GetHashCode(reference);

    public static bool operator ==(MsgArg left, MsgArg right) => left.Equals(right);

    public static bool operator !=(MsgArg left, MsgArg right) => !left.Equals(right);
}
