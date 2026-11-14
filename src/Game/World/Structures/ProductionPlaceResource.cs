// Translated from PrdPlace.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Units;

namespace Aow.Game.World.Structures;

// available unit indexes
public partial class ProductionPlaceResource : PlayerStructureResource
{
    public ProductionPlaceResource(ECustomNode owner) : base(owner)
    {
        Claimed = false;
        Units = new UnitIndexList();
        Razeable = true;
    }

    // PrdPlace.pas:718
    public new const int ClassId = 132027;

    public UnitIndexList Units { get; }

    // PrdPlace.pas:709
    public override void Dispose()
    {
        Units?.Dispose();
        base.Dispose();
    }

    // PrdPlace.pas:730
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.UnitIndexList(persister, name: "Units", list: Units);
    }

    // PrdPlace.pas:701

    // PrdPlace.pas:724
    public override EditorClass Editor()
    {
        EditorClass result = Engine.Editors.FindEditor(132027);
        if (result == null)
        {
            result = base.Editor();
        }

        return result;
    }

    // PrdPlace.pas:721
    public override int ResourceUserClassId() => 132026;

    // PrdPlace.pas:100

    // PrdPlace.pas:715
    public override string ResourceEditName() => "";

}
