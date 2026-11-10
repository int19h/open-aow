using System.Runtime.CompilerServices;

namespace Aow.Rtl;

public struct IntOrRef
{
    public int Value;
    public object? Ref;

    public IntOrRef(object? value)
    {
        Ref = value;
        Value = IdentityHash.Of(value);
    }

    public static implicit operator IntOrRef(int value) => new() { Value = value };
    // A conversion from object is illegal. The constructor takes a reference. A string still
    // converts on its own and stores the identity hash of the string.
    public static implicit operator IntOrRef(string value) => new() { Ref = value, Value = value is null ? 0 : RuntimeHelpers.GetHashCode(value) };
    public readonly T? As<T>() => Ref is T typed ? typed : typeof(T) == typeof(string) ? (T)(object)"" : default;
}
