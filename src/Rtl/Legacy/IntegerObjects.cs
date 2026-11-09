namespace Aow.Rtl;

public static partial class RtlUnit
{
    // Delphi code stored small integers in TObject slots (TStrings.Objects, a TNotifyEvent sender)
    // with TObject(I) and read them back with Integer(Obj). IntToObject gives one cached box per
    // value, so IndexOfObject finds it again, and ObjectToInt reads the box back. The values are
    // enum values, indices and small ids, so the cache stays bounded and holds no game object.
    private sealed class IntegerObject(int value) : TObject { public int Value { get; } = value; }

    private static readonly Dictionary<int, TObject> integerObjects = [];

    public static TObject? IntToObject(int value)
    {
        if (value == 0) return null;
        lock (integerObjects)
        {
            if (!integerObjects.TryGetValue(value, out var box)) integerObjects.Add(value, box = new IntegerObject(value));
            return box;
        }
    }

    // Integer(Obj): the value of an integer box, 0 for nil, and the address stand-in of any other object.
    // Product code stores integers in object slots and reads them back. It does not recover an object from the id.
    // A fixture process uses construction order. Every other process uses the identity hash.
    public static int ObjectToInt(TObject? value)
    {
        switch (value)
        {
            case null: return 0;
            case IntegerObject box: return box.Value;
            default: return CaptureAddress.Of(value);
        }
    }
}
