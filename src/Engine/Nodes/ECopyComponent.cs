#nullable enable
using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// The editor clipboard has no host in the Engine set.
public partial class ECopyComponent : Component
{
    private EObjectList objects = null!;

    public ECopyComponent(Component? owner) : base(owner)
    {
        objects = new EObjectList();
    }

    public override void Dispose() { objects?.Dispose(); base.Dispose(); }

    public IndexedProperty<int, EObject> EObjects => new(i => objects.EObjects[i], (i, value) => objects.EObjects[i] = value);
    public int Count => objects.Count;
    public EObjectList EObjectList => objects;

    public void CopyToClipboard(GameEngine engine, string name) { }

    public void PasteFromClipboard(GameEngine engine, string name) { }
}
