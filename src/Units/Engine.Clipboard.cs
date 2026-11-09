#nullable enable
using Aow.Rtl;

namespace Aow.Units.Engine;

public static partial class EngineUnit
{
    public static int CopyStreamToClipboard(TStream stream, string format) => 0;
    public static int PasteStreamFromClipboard(TStream stream, string format) => 0;
}

// The editor clipboard has no host in the Engine set.
public partial class TECopyComponent : TComponent
{
    private TEObjectList objects = null!;

    public TECopyComponent(TComponent? owner) : base(owner)
    {
        objects = new TEObjectList();
    }

    public override void Dispose() { objects?.Dispose(); base.Dispose(); }

    public IndexedProperty<int, TEObject> EObjects => new(i => objects.EObjects[i], (i, value) => objects.EObjects[i] = value);
    public int Count => objects.Count;
    public TEObjectList EObjectList => objects;

    public void CopyToClipboard(TEngine engine, string name) { }

    public void PasteFromClipboard(TEngine engine, string name) { }
}
