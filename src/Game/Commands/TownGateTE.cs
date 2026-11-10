// Translated from CitySpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// army ID. destination coordinates
public partial class TownGateTE : CityTargetSpellTE
{
    public TownGateTE(ECustomNode owner) : base(owner)
    { }

    // CitySpells.pas:397
    public new const int ClassID = 135950;

    public ArmyID FArmyID;
    public XYL FTeleportLocation;

    // CitySpells.pas:391
    public override void Dispose()
    {
        FArmyID?.Dispose();
        base.Dispose();
    }

    // CitySpells.pas:400
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FTeleportLocation.Packed, 30);
        Stream.rwCreateEObject(ref FArmyID, null, 31);
    }

    // CitySpells.pas:407
    public override bool Validate(Hero source)
    {
        byte Selection = default;
        ArmyHS Army = default;
        bool Result = base.Validate(source) && FArmyID.Valid();
        if (Result)
        {
            FArmyID.RetrieveArmy(ref Army, ref Selection);
            Result = ((TownGate)AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID)).ValidTeleportLocation(Army, Selection, FTeleportLocation.X, FTeleportLocation.Y, FTeleportLocation.L);
        }

        return Result;
    }

    // NetworkE.pas:230

}
