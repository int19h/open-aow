namespace Aow.Rtl;

public static partial class RtlUnit
{
    // Delphi code stored small integers in TObject slots (TStrings.Objects, a TNotifyEvent sender)
    // with TObject(I) and read them back with Integer(Obj). IntToObject gives one cached box per
    // value, so IndexOfObject finds it again, and ObjectToInt reads the box back. The values are
    // enum values, indices and small ids, so the cache stays bounded and holds no game object.
    internal sealed class IntegerObject(int value)
    {
        public int Value { get; } = value;
    }

    private static readonly Dictionary<int, object> integerObjects = [];

    public static object? IntToObject(int value)
    {
        if (value == 0)
        {
            return null;
        }

        lock (integerObjects)
        {
            if (!integerObjects.TryGetValue(value, out object? box))
            {
                integerObjects.Add(value, box = new IntegerObject(value));
            }

            return box;
        }
    }

    // Integer(Obj): the value of an integer box, 0 for nil, and the identity hash of any other object.
    // Product code stores integers in object slots and reads them back. It does not recover an object from the id.
    public static int ObjectToInt(object? value)
    {
        return value switch
        {
            null => 0,
            IntegerObject box => box.Value,
            _ => IdentityHash.Of(value),
        };
    }
}
