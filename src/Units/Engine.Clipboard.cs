#nullable enable
using Aow.Rtl;

namespace Aow.Units.Engine;

public static partial class Engine
{
    public static int CopyStreamToClipboard(TStream stream, AnsiString format) => 0;
    public static int PasteStreamFromClipboard(TStream stream, AnsiString format) => 0;
}

// The editor clipboard has no host in the Engine set.
public partial class TECopyComponent : TComponent
{
    private TEObjectList objects = null!;
    public override void Create__init(TComponent? owner)
    {
        base.Create__init(owner);
        objects = new TEObjectList();
    }
    public static TECopyComponent Create(TComponent? owner)
    {
        var result = new TECopyComponent();
        try { result.Create__init(owner); }
        catch { result.Destroy(); throw; }
        return result;
    }
    public override void Destroy() { objects.Free(); base.Destroy(); }
    public IndexedProperty<int, TEObject> EObjects => new(i => objects.EObjects[i], (i, value) => objects.EObjects[i] = value);
    public int Count => objects.Count;
    public TEObjectList EObjectList => objects;
    public void CopyToClipboard(TEngine engine, AnsiString name) { }
    public void PasteFromClipboard(TEngine engine, AnsiString name) { }
}
