namespace Aow.Units.Engine;

public partial class TEStorageStream
{
    // Delphi var parameters alias a descendant reference through a base-class cast.
    public void rwCreateEObject<T>(ref T value, TECustomNode owner, int id) where T : class
    {
        TEObject result = (TEObject)(object)value;
        try { rwCreateEObject(ref result, owner, id); }
        finally { value = (T)(object)result; }
    }
}
