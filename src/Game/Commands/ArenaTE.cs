// Translated from Arena.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Commands;

// FUnitIDs
public partial class ArenaTE : StructureTE
{
    public ArenaTE(ECustomNode owner) : base(owner)
    {
        FUnitIDs = new IntegerList();
    }

    // Arena.pas:86
    public new const int ClassID = 133294;

    public IntegerList FUnitIDs;

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
