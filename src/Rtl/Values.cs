namespace Aow.Rtl;

public struct IntOrRef
{
    public int Value;
    public object? Ref;
    public static implicit operator IntOrRef(int value) => new() { Value = value };
    public static implicit operator IntOrRef(TObject? value) => new() { Ref = value, Value = value?.InstanceId ?? 0 };
    public static implicit operator IntOrRef(AnsiString value) => new() { Ref = value, Value = TObject.AllocateId() };
    public readonly T? As<T>() => Ref is T value ? value : default;
}

public enum VarKind { Integer, Boolean, Char, Extended, String, Object, Pointer, PChar }

public struct TVarRec
{
    public VarKind Kind;
    public long Int;
    public double Ext;
    public AnsiString Str;
    public object? Obj;
    public bool Bool;
    public byte Chr;
    public static implicit operator TVarRec(int value) => new() { Kind = VarKind.Integer, Int = value };
    // Char maps to byte. Numeric Byte arguments use the int conversion.
    public static implicit operator TVarRec(byte value) => new() { Kind = VarKind.Char, Chr = value };
    public static implicit operator TVarRec(bool value) => new() { Kind = VarKind.Boolean, Bool = value };
    public static implicit operator TVarRec(double value) => new() { Kind = VarKind.Extended, Ext = value };
    public static implicit operator TVarRec(AnsiString value) => new() { Kind = VarKind.String, Str = value };
    public static implicit operator TVarRec(string? value) => new() { Kind = VarKind.String, Str = AnsiString.FromBytes(Cp1252.Encode(value)) };
    public static implicit operator TVarRec(char value) => new() { Kind = VarKind.Char, Chr = Cp1252.Byte(value) };
    public static implicit operator TVarRec(TObject? value) => new() { Kind = VarKind.Object, Obj = value };
    public static implicit operator TVarRec(Ptr<byte> value) => new() { Kind = VarKind.PChar, Obj = value };
    public static TVarRec Pointer(uint value) => new() { Kind = VarKind.Pointer, Int = value };
}
