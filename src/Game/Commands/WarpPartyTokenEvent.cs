#nullable disable warnings
// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// army ID, packed destination
public class WarpPartyTokenEvent(CustomNode? owner) : SpellTokenEvent(owner)
{

    // GlobalSpells.pas:564
    public new const int ClassId = 135_948;

    public ArmyId? ArmyId { get; set; }

    public MapCoordinate WarpLocation { get; internal set; }

    // GlobalSpells.pas:558
    public override void Dispose()
    {
        ArmyId?.Dispose();
        base.Dispose();
    }

    // GlobalSpells.pas:569
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int location = persister.Int32("WarpLocation", value: StoredCoordinateImage.From(WarpLocation).ToInt32());
        WarpLocation = StoredCoordinateImage.FromInt32(location).Location;
        ArmyId = persister.Created("ArmyId", objectValue: ArmyId);
    }

    // GlobalSpells.pas:576
    public override bool Validate(Hero source)
    {
        bool result = base.Validate(source) && ArmyId.Valid();
        if (result)
        {
            int selection = default;
            ArmyHexSprite? army = default;
            _ = ArmyId.RetrieveArmy(ref army, ref selection);
            return GetEngine().GamePack.SpellControl.GetSpell<WarpParty>(EventSpellId).ValidTarget(army, WarpLocation.X, WarpLocation.Y, layer: WarpLocation.MapLevel);
        }

        return result;
    }

    // GlobalSpells.pas:590
    public override bool Process(Troop source, int value)
    {
        int selection = default;
        ArmyHexSprite? army = default;
        _ = base.Process(source, value);
        MapCoordinate coordinate15 = source.GetLocation();
        int x = coordinate15.X;
        int y = coordinate15.Y;
        int layer = coordinate15.MapLevel;
        _ = ArmyId.RetrieveArmy(ref army, ref selection);
        MapField field = GetEngine().StrategicMap.MapContainer.GetFieldMapCoordinate(WarpLocation);
        _ = army.MoveTo(field, moveCost: 0);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        bool showSource = strategicMap.SeatedPlayer == source.Player || strategicMap.VisibleForPlayer(x, y, layer, player: strategicMap.SeatedPlayer);
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        bool showTarget = engineStrategicMap.SeatedPlayer == source.Player || engineStrategicMap.VisibleForPlayer(field.HexX, field.HexY, field.HexLevel, player: engineStrategicMap.SeatedPlayer);
        ImageSequenceAnimation animation;
        if (showSource && GetEngine().GamePack.SpellControl[EventSpellId].CastImages.ImageSequences[0] is not null)
        {
            animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
            animation.SetImageSequence(GetEngine().GamePack.SpellControl[EventSpellId].CastImages.ImageSequences[0]);
            _ = GetEngine().StrategicMap.PlaceHex(animation, x, y, L: layer);
        }

        if (showTarget && GetEngine().GamePack.SpellControl[EventSpellId].CastImages.ImageSequences[0] is not null)
        {
            animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
            animation.SetImageSequence(GetEngine().GamePack.SpellControl[EventSpellId].CastImages.ImageSequences[0]);
            _ = GetEngine().StrategicMap.PlaceHex(animation, field.HexX, field.HexY, L: field.HexLevel);
        }

        StrategicMap centerViewToStrategicMap = GetEngine().StrategicMap;
        if (source.Player == centerViewToStrategicMap.SeatedPlayer)
        {
            centerViewToStrategicMap.CenterViewTo(field.HexX, field.HexY, field.HexLevel);
        }
        else
        {
            _ = GetEngine().StrategicMap.ValidateObservation(source.Player, field.HexX, field.HexY, field.HexLevel, radius: 0);
        }

        StrategicMap watchingStrategicMap = GetEngine().StrategicMap;
        if (source.Player == watchingStrategicMap.SeatedPlayer || watchingStrategicMap.Watching(x, y, L: layer, radius: 0) || watchingStrategicMap.Watching(field.HexX, field.HexY, L: field.HexLevel, radius: 0))
        {
            _ = GetEngine().GamePack.SpellControl[EventSpellId].CastSounds.Play(0);
        }

        GamePack gamePack = GetEngine().GamePack;
        Spell? spell = gamePack.SpellControl[EventSpellId];
        gamePack.SpellControl[EventSpellId].CastingDone(source, mana: spell.BaseManaCost);
        return false;
    }
}
