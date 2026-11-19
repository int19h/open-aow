#nullable disable
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
public partial class WarpPartyTokenEvent(CustomNode owner) : SpellTokenEvent(owner)
{

    // GlobalSpells.pas:564
    public new const int ClassId = 135948;

    public ArmyId ArmyId { get; set; }

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
        byte selection = default;
        ArmyHexSprite army = default;
        bool result = base.Validate(source) && ArmyId.Valid();
        if (result)
        {
            _ = ArmyId.RetrieveArmy(ref army, ref selection);
            result = GamePack.Current.SpellControl.GetSpell<WarpParty>(EventSpellId).ValidTarget(army, WarpLocation.X, WarpLocation.Y, WarpLocation.MapLevel);
        }

        return result;
    }

    // GlobalSpells.pas:590
    public override bool Process(Troop source, int value)
    {
        bool showSource;
        bool showTarget;
        byte selection = default;
        ArmyHexSprite army = default;
        MapField field;
        ImageSequenceAnimation animation;
        _ = base.Process(source, value);
        MapCoordinate coordinate15 = source.GetLocation();
        sbyte x = coordinate15.X;
        sbyte y = coordinate15.Y;
        sbyte layer = coordinate15.MapLevel;
        _ = ArmyId.RetrieveArmy(ref army, ref selection);
        field = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(WarpLocation);
        _ = army.MoveTo(field, 0);
        showSource = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.VisibleForPlayer(x, y, layer, StrategicMap.Current.SeatedPlayer);
        showTarget = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.VisibleForPlayer(field.HexX, field.HexY, field.HexLevel, StrategicMap.Current.SeatedPlayer);
        if (showSource && GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] != null)
        {
            animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
            animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
            _ = StrategicMap.Current.PlaceHex(animation, x, y, layer);
        }

        if (showTarget && GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] != null)
        {
            animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
            animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
            _ = StrategicMap.Current.PlaceHex(animation, field.HexX, field.HexY, field.HexLevel);
        }

        if (source.Player == StrategicMap.Current.SeatedPlayer)
        {
            StrategicMap.Current.CenterViewTo(field.HexX, field.HexY, field.HexLevel);
        }
        else
        {
            _ = StrategicMap.Current.ValidateObservation(source.Player, field.HexX, field.HexY, field.HexLevel, 0);
        }

        if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(x, y, layer, 0) || StrategicMap.Current.Watching(field.HexX, field.HexY, field.HexLevel, 0))
        {
            _ = GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
        }

        Spell spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
        return false;
    }

    // GlobalSpells.pas:92

}
