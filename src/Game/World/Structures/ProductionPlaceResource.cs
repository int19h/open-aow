// Translated from PrdPlace.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Units;

namespace Aow.Game.World.Structures;

// available unit indexes
public partial class ProductionPlaceResource : PlayerStructureResource
{
    public ProductionPlaceResource(ECustomNode owner) : base(owner)
    {
        FClaimed = false;
        FUnits = new UnitIndexList();
        FRazeable = true;
    }

    // PrdPlace.pas:718
    public new const int ClassID = 132027;

    public UnitIndexList FUnits;

    // PrdPlace.pas:709
    public override void Dispose()
    {
        FUnits?.Dispose();
        base.Dispose();
    }

    // PrdPlace.pas:730
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(18, FUnits.LoadFromStream, FUnits.SaveToStream);
    }

    // PrdPlace.pas:701

    // PrdPlace.pas:724
    public override EditorClass Editor()
    {
        EditorClass Result = Engine.Editors.FindEditor(132027);
        if (Result == null)
        {
            Result = base.Editor();
        }

        return Result;
    }

    // PrdPlace.pas:721
    public override int ResourceUserClassID() => 132026;

    // PrdPlace.pas:100

    // PrdPlace.pas:715
    public override string ResourceEditName() => "";

}
