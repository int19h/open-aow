// Translated from Arena.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Commands;

// UnitIds
public partial class ArenaTokenEvent : StructureTokenEvent
{
    public ArenaTokenEvent(ECustomNode owner) : base(owner)
    {
        UnitIds = new Int32List();
    }

    // Arena.pas:86
    public new const int ClassId = 133294;

    public Int32List UnitIds { get; }

    // Arena.pas:80
    public override void Dispose()
    {
        UnitIds?.Dispose();
        base.Dispose();
    }

    // Arena.pas:89
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStreamProc(40, UnitIds.LoadFromStream, UnitIds.SaveToStream);
    }

    // Arena.pas:74

    // Arena.pas:23

}
