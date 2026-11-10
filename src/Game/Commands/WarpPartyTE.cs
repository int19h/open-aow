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
public partial class WarpPartyTE : SpellTE
{
    // GlobalSpells.pas:564
    public new const int ClassID = 135948;

    public ArmyID FArmyID;
    public XYL FWarpLocation;

    // GlobalSpells.pas:558
    public override void Dispose()
    {
        FArmyID?.Dispose();
        base.Dispose();
    }

    // GlobalSpells.pas:569
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FWarpLocation.Packed, 30);
        Stream.rwCreateEObject(ref FArmyID, null, 31);
    }

    // GlobalSpells.pas:552
    public WarpPartyTE(ECustomNode owner) : base(owner)
    {
        FArmyID = null;
    }

    // GlobalSpells.pas:576
    public override bool Validate(Hero source)
    {
        byte Selection = default;
        ArmyHS Army = default;
        bool Result = base.Validate(source) && FArmyID.Valid();
        if (Result)
        {
            FArmyID.RetrieveArmy(ref Army, ref Selection);
            Result = ((WarpParty)AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID)).ValidTarget(Army, FWarpLocation.X, FWarpLocation.Y, FWarpLocation.L);
        }

        return Result;
    }

    // GlobalSpells.pas:590
    public override bool Process(AbstractUnit source, int value)
    {
        bool ShowSource;
        bool ShowTarget;
        byte Selection = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        ArmyHS Army = default;
        MapField Field;
        ImageSequenceAnimation Animation;
        base.Process(source, value);
        source.GetLocation(ref X, ref Y, ref Layer);
        FArmyID.RetrieveArmy(ref Army, ref Selection);
        Field = AoWHSMap.Current.MapContainer.GetFieldXYL(FWarpLocation.Packed);
        Army.MoveTo(Field, 0);
        ShowSource = AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.VisibleForPlayer(X, Y, Layer, AoWHSMap.Current.FSeatedPlayer);
        ShowTarget = AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.VisibleForPlayer(Field.Xhx, Field.Yhx, Field.Lhx, AoWHSMap.Current.FSeatedPlayer);
        if (ShowSource && AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
        {
            Animation = new ImageSequenceAnimation(null);
            Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
            AoWHSMap.Current.PlaceHX(Animation, X, Y, Layer);
        }

        if (ShowTarget && AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
        {
            Animation = new ImageSequenceAnimation(null);
            Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
            AoWHSMap.Current.PlaceHX(Animation, Field.Xhx, Field.Yhx, Field.Lhx);
        }

        if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer)
        {
            AoWHSMap.Current.CenterViewTo(Field.Xhx, Field.Yhx, Field.Lhx);
        }
        else
        {
            AoWHSMap.Current.ValidateObservation(source.FPlayer, Field.Xhx, Field.Yhx, Field.Lhx, 0);
        }

        if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(X, Y, Layer, 0) || AoWHSMap.Current.Watching(Field.Xhx, Field.Yhx, Field.Lhx, 0))
        {
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
        }

        var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
        return false;
    }

    // GlobalSpells.pas:92

}
