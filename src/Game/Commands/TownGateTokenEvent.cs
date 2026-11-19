#nullable disable
// Translated from CitySpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// army ID. destination coordinates
public partial class TownGateTokenEvent(CustomNode owner) : CityTargetSpellTokenEvent(owner)
{

    // CitySpells.pas:397
    public new const int ClassId = 135950;

    public ArmyId ArmyId { get; set; }

    public MapCoordinate TeleportLocation { get; internal set; }

    // CitySpells.pas:391
    public override void Dispose()
    {
        ArmyId?.Dispose();
        base.Dispose();
    }

    // CitySpells.pas:400
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int location = persister.Int32("TeleportLocation", value: StoredCoordinateImage.From(TeleportLocation).ToInt32());
        TeleportLocation = StoredCoordinateImage.FromInt32(location).Location;
        ArmyId = persister.Created("ArmyId", objectValue: ArmyId);
    }

    // CitySpells.pas:407
    public override bool Validate(Hero source)
    {
        byte selection = default;
        ArmyHexSprite army = default;
        bool result = base.Validate(source) && ArmyId.Valid();
        if (result)
        {
            _ = ArmyId.RetrieveArmy(ref army, ref selection);
            result = GamePack.Current.SpellControl.GetSpell<TownGate>(EventSpellId).ValidTeleportLocation(army, selection, TeleportLocation.X, TeleportLocation.Y, TeleportLocation.MapLevel);
        }

        return result;
    }

    // NetworkE.pas:230

}
