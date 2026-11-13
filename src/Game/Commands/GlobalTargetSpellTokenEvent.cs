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

    // The fourth byte of the packed location stored at id 20, 0 from every writer and in every
    // corpus file, kept so that a loaded value saves back unchanged.
    public byte UnknownBytes { get; set; }

    // GlobalTargetSpells.pas:406
    // packed coordinates
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        int location = StoredCoordinateImage.From(Location, unknownBytes: UnknownBytes).ToInt32();
        stream.Int32(ref location, 20);
        (Location, UnknownBytes) = StoredCoordinateImage.FromInt32(location);
    }

    // GlobalTargetSpells.pas:412
    public void SetLocation(sbyte X, sbyte Y, sbyte layer)
    {
        Location = new MapCoordinate(X: X, Y: Y, MapLevel: layer);
    }

    // GlobalTargetSpells.pas:419
    public void PlayDefaultEffects(sbyte X, sbyte Y, sbyte layer)
    {
        ImageSequenceAnimation animation;
        if (GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] != null)
        {
            animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
            animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
            StrategicMap.Current.PlaceHex(animation, X, Y, layer);
        }

        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
    }

    // NetworkE.pas:230

}
