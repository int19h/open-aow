// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw.Animations;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// Location: packed X/Y/layer coordinates
public partial class GlobalTargetSpellTokenEvent : SpellTokenEvent
{
    public GlobalTargetSpellTokenEvent(ECustomNode owner) : base(owner)
    { }

    // GlobalTargetSpells.pas:401
    public new const int ClassId = 135976;

    public MapCoordinate Location;

    // GlobalTargetSpells.pas:406
    // packed coordinates
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Location.Packed, 20);
    }

    // GlobalTargetSpells.pas:412
    public void SetLocation(sbyte X, sbyte Y, sbyte layer)
    {
        Location.X = X;
        Location.Y = Y;
        Location.MapLevel = layer;
    }

    // GlobalTargetSpells.pas:419
    public void PlayDefaultEffects(sbyte X, sbyte Y, sbyte layer)
    {
        ImageSequenceAnimation animation;
        if (GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] != null)
        {
            animation = new ImageSequenceAnimation(null);
            animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
            StrategicMap.Current.PlaceHex(animation, X, Y, layer);
        }

        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
    }

    // NetworkE.pas:230

}
