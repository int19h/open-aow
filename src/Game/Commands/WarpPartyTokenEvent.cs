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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref WarpLocation.Packed, 30);
        stream.rwCreateEObject(ref ArmyId, null, 31);
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
            result = ((WarpParty)AoWHSSet.Current.SpellControl.GetSpell(EventSpellId)).ValidTarget(army, WarpLocation.X, WarpLocation.Y, WarpLocation.MapLevel);
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
        source.GetLocation(ref X, ref Y, ref layer);
        ArmyId.RetrieveArmy(ref army, ref selection);
        field = AoWHSMap.Current.MapContainer.GetFieldMapCoordinate(WarpLocation.Packed);
        army.MoveTo(field, 0);
        showSource = AoWHSMap.Current.SeatedPlayer == source.Player || AoWHSMap.Current.VisibleForPlayer(X, Y, layer, AoWHSMap.Current.SeatedPlayer);
        showTarget = AoWHSMap.Current.SeatedPlayer == source.Player || AoWHSMap.Current.VisibleForPlayer(field.HexX, field.HexY, field.HexLevel, AoWHSMap.Current.SeatedPlayer);
        if (showSource && AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] != null)
        {
            animation = new ImageSequenceAnimation(null);
            animation.SetImageSequence(AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
            AoWHSMap.Current.PlaceHex(animation, X, Y, layer);
        }

        if (showTarget && AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] != null)
        {
            animation = new ImageSequenceAnimation(null);
            animation.SetImageSequence(AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
            AoWHSMap.Current.PlaceHex(animation, field.HexX, field.HexY, field.HexLevel);
        }

        if (source.Player == AoWHSMap.Current.SeatedPlayer)
        {
            AoWHSMap.Current.CenterViewTo(field.HexX, field.HexY, field.HexLevel);
        }
        else
        {
            AoWHSMap.Current.ValidateObservation(source.Player, field.HexX, field.HexY, field.HexLevel, 0);
        }

        if (source.Player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Watching(X, Y, layer, 0) || AoWHSMap.Current.Watching(field.HexX, field.HexY, field.HexLevel, 0))
        {
            AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
        }

        var spell = AoWHSSet.Current.SpellControl.GetSpell(EventSpellId);
        AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
        return false;
    }

    // GlobalSpells.pas:92

}
