// Translated from GlobalTargetSpells.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Spells.GlobalTarget;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// FLocation: packed X/Y/layer coordinates
public partial class GlobalTargetSpellTE : SpellTE
{
    public GlobalTargetSpellTE(ECustomNode owner) : base(owner)
    { }

    // GlobalTargetSpells.pas:401
    public new const int ClassID = 135976;

    public XYL FLocation;

    // GlobalTargetSpells.pas:406
    // packed coordinates
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FLocation.Packed, 20);
    }

    // GlobalTargetSpells.pas:412
    public void SetLocation(sbyte X, sbyte Y, sbyte Layer)
    {
        Unsafe.As<XYL, SpellLocation>(ref FLocation).X = X;
        Unsafe.As<XYL, SpellLocation>(ref FLocation).Y = Y;
        Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer = Layer;
    }

    // GlobalTargetSpells.pas:419
    public void PlayDefaultEffects(sbyte X, sbyte Y, sbyte Layer)
    {
        ImageSequenceAnimation Animation;
        if (AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
        {
            Animation = new ImageSequenceAnimation(null);
            Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
            AoWHSMap.Current.PlaceHX(Animation, X, Y, Layer);
        }

        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
    }

    // NetworkE.pas:230

}
