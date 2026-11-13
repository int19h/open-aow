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
public partial class TownGateTokenEvent : CityTargetSpellTokenEvent
{
    public TownGateTokenEvent(ECustomNode owner) : base(owner)
    { }

    // CitySpells.pas:397
    public new const int ClassId = 135950;

    public ArmyId ArmyId;
    public MapCoordinate TeleportLocation;

    // The fourth byte of the packed teleport location stored at id 30 (UnknownBytes keeps the one of
    // id 20), 0 from every writer and in every corpus file, kept so that a loaded value saves back
    // unchanged.
    public byte TeleportUnknownBytes { get; set; }

    // CitySpells.pas:391
    public override void Dispose()
    {
        ArmyId?.Dispose();
        base.Dispose();
    }

    // CitySpells.pas:400
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        int location = StoredCoordinateImage.From(TeleportLocation, unknownBytes: TeleportUnknownBytes).ToInt32();
        stream.Int32(ref location, 30);
        (TeleportLocation, TeleportUnknownBytes) = StoredCoordinateImage.FromInt32(location);
        stream.Created(ref ArmyId, null, 31);
    }

    // CitySpells.pas:407
    public override bool Validate(Hero source)
    {
        byte selection = default;
        ArmyHexSprite army = default;
        bool result = base.Validate(source) && ArmyId.Valid();
        if (result)
        {
            ArmyId.RetrieveArmy(ref army, ref selection);
            result = ((TownGate)GamePack.Current.SpellControl.GetSpell(EventSpellId)).ValidTeleportLocation(army, selection, TeleportLocation.X, TeleportLocation.Y, TeleportLocation.MapLevel);
        }

        return result;
    }

    // NetworkE.pas:230

}
