#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PrdPlace.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Units;

namespace Aow.Game.World.Structures;

// available unit indexes
public class ProductionPlaceResource : PlayerStructureResource
{
    public ProductionPlaceResource(CustomNode owner) : base(owner)
    {
        Claimed = false;
        Units = [];
        Razeable = true;
    }

    // PrdPlace.pas:718
    public new const int ClassId = 132_027;

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
        persister.UnitIndexList(name: "Units", list: Units);
    }

    // PrdPlace.pas:724
    public override EditorClass Editor() => Engine.Editors.FindEditor(ClassId) ?? base.Editor();

    // PrdPlace.pas:721
    public override int ResourceUserClassId() => ProductionPlace.ClassId;

    // PrdPlace.pas:715
    public override string ResourceEditName() => "";

}
