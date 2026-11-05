namespace Aow.Rtl;

public struct IntOrRef
{
    public int Value;
    public object? Ref;
    public static implicit operator IntOrRef(int value) => new() { Value = value };
    public static implicit operator IntOrRef(TObject? value) => new() { Ref = value, Value = value?.InstanceId ?? 0 };
    public static implicit operator IntOrRef(string value) => new() { Ref = value, Value = TObject.AllocateId() };
    public readonly T? As<T>() => Ref is T value ? value : typeof(T) == typeof(string) ? (T)(object)"" : default;
}
