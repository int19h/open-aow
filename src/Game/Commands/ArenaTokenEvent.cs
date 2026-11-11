// Translated from Arena.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Commands;

// FUnitIDs
public partial class ArenaTokenEvent : StructureTokenEvent
{
    public ArenaTokenEvent(ECustomNode owner) : base(owner)
    {
        FUnitIDs = new Int32List();
    }

    // Arena.pas:86
    public new const int ClassID = 133294;

    public Int32List FUnitIDs;

    // Arena.pas:80
    public override void Dispose()
    {
        FUnitIDs?.Dispose();
        base.Dispose();
    }

    // Arena.pas:89
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(40, FUnitIDs.LoadFromStream, FUnitIDs.SaveToStream);
    }

    // Arena.pas:74

    // Arena.pas:23

}
