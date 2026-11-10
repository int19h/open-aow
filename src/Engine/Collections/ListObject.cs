// Translated from EList.pas.

namespace Aow.Engine.Collections;

// no fields. TObjectList.LoadFromStream constructs items with their ordinary constructor. The
// stream methods are empty here and overridden by the items
public partial class ListObject : IDisposable
{
    // EList.pas:50
    public virtual void SaveToStream(Stream Stream)
    {
    }

    // EList.pas:54
    public virtual void LoadFromStream(Stream Stream)
    {
    }

    public virtual void Dispose()
    {
    }

}
