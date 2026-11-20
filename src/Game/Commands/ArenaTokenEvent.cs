// Translated from Arena.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Commands;

// UnitIds
public class ArenaTokenEvent(CustomNode owner) : StructureTokenEvent(owner)
{

    // Arena.pas:86
    public new const int ClassId = 133294;

    public Int32List UnitIds { get; } = new Int32List();

    // Arena.pas:80
    public override void Dispose()
    {
        UnitIds?.Dispose();
        base.Dispose();
    }

    // Arena.pas:89
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Int32List(persister, name: "UnitIds", list: UnitIds);
    }
}
