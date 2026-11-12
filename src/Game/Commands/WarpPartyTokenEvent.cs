// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// army ID, packed destination
public partial class WarpPartyTokenEvent : SpellTokenEvent
{
    // GlobalSpells.pas:564
    public new const int ClassId = 135948;

    public ArmyId ArmyId;
    public MapCoordinate WarpLocation;

    // GlobalSpells.pas:558
    public override void Dispose()
    {
        ArmyId?.Dispose();
        base.Dispose();
    }

    // GlobalSpells.pas:569
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref WarpLocation.Packed, 30);
        stream.Created(ref ArmyId, null, 31);
    }

    // GlobalSpells.pas:552
    public WarpPartyTokenEvent(ECustomNode owner) : base(owner)
    {
        ArmyId = null;
    }

    // GlobalSpells.pas:576
    public override bool Validate(Hero source)
    {
        byte selection = default;
        ArmyHexSprite army = default;
        bool result = base.Validate(source) && ArmyId.Valid();
        if (result)
        {
            ArmyId.RetrieveArmy(ref army, ref selection);
            result = ((WarpParty)GamePack.Current.SpellControl.GetSpell(EventSpellId)).ValidTarget(army, WarpLocation.X, WarpLocation.Y, WarpLocation.MapLevel);
        }

        return result;
    }

    // GlobalSpells.pas:590
    public override bool Process(Troop source, int value)
    {
        bool showSource;
        bool showTarget;
        byte selection = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte layer = default;
        ArmyHexSprite army = default;
        MapField field;
        ImageSequenceAnimation animation;
        base.Process(source, value);
        source.GetLocation(out X, out Y, out layer);
        ArmyId.RetrieveArmy(ref army, ref selection);
        field = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(WarpLocation.Packed);
        army.MoveTo(field, 0);
        showSource = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.VisibleForPlayer(X, Y, layer, StrategicMap.Current.SeatedPlayer);
        showTarget = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.VisibleForPlayer(field.HexX, field.HexY, field.HexLevel, StrategicMap.Current.SeatedPlayer);
        if (showSource && GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] != null)
        {
            animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
            animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
            StrategicMap.Current.PlaceHex(animation, X, Y, layer);
        }

        if (showTarget && GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] != null)
        {
            animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
            animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
            StrategicMap.Current.PlaceHex(animation, field.HexX, field.HexY, field.HexLevel);
        }

        if (source.Player == StrategicMap.Current.SeatedPlayer)
        {
            StrategicMap.Current.CenterViewTo(field.HexX, field.HexY, field.HexLevel);
        }
        else
        {
            StrategicMap.Current.ValidateObservation(source.Player, field.HexX, field.HexY, field.HexLevel, 0);
        }

        if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(X, Y, layer, 0) || StrategicMap.Current.Watching(field.HexX, field.HexY, field.HexLevel, 0))
        {
            GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
        }

        var spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
        return false;
    }

    // GlobalSpells.pas:92

}
