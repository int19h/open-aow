using System.Runtime.CompilerServices;

namespace Aow.Rtl;

public struct IntOrRef
{
    public int Value;
    public object? Ref;

    public IntOrRef(object? value)
    {
        Ref = value;
        Value = CaptureAddress.Of(value);
    }

    public static implicit operator IntOrRef(int value) => new() { Value = value };
    // A conversion from object is illegal. The constructor takes a reference. A string still
    // converts on its own so a string key keeps the gap the old counter left for it.
    public static implicit operator IntOrRef(string value) => new() { Ref = value, Value = CaptureAddress.Pinned ? CaptureAddress.Burn() : value is null ? 0 : RuntimeHelpers.GetHashCode(value) };
    public readonly T? As<T>() => Ref is T typed ? typed : typeof(T) == typeof(string) ? (T)(object)"" : default;
}
