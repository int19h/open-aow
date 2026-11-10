#nullable enable
using Aow.Game.Engine;
using Aow.Rtl;

namespace Aow.Game.Engine.Nodes;

// The editor clipboard has no host in the Engine set.
public partial class ECopyComponent : global::Aow.Rtl.Component
{
    private global::Aow.Game.Engine.Nodes.EObjectList objects = null!;

    public ECopyComponent(global::Aow.Rtl.Component? owner) : base(owner)
    {
        objects = new global::Aow.Game.Engine.Nodes.EObjectList();
    }

    public override void Dispose() { objects?.Dispose(); base.Dispose(); }

    public IndexedProperty<int, EObject> EObjects => new(i => objects.EObjects[i], (i, value) => objects.EObjects[i] = value);
    public int Count => objects.Count;
    public global::Aow.Game.Engine.Nodes.EObjectList EObjectList => objects;

    public void CopyToClipboard(global::Aow.Game.Engine.Engine engine, string name) { }

    public void PasteFromClipboard(global::Aow.Game.Engine.Engine engine, string name) { }
}
