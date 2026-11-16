#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// Location: packed X/Y/layer coordinates
public partial class GlobalTargetSpellTokenEvent : SpellTokenEvent
{
    public GlobalTargetSpellTokenEvent(CustomNode owner) : base(owner)
    { }

    // GlobalTargetSpells.pas:401
    public new const int ClassId = 135976;

    public MapCoordinate Location { get; private set; }

    // The fourth byte of the packed location stored at id 20, 0 from every writer and in every
    // corpus file, kept so that a loaded value saves back unchanged.
    public byte UnknownBytes { get; set; }

    // GlobalTargetSpells.pas:406
    // packed coordinates
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int location = StoredCoordinateImage.From(Location, unknownBytes: UnknownBytes).ToInt32();
        location = persister.Int32("Location", value: location);
        (Location, UnknownBytes) = StoredCoordinateImage.FromInt32(location);
    }

    // GlobalTargetSpells.pas:412
    public void SetLocation(MapCoordinate location) => Location = location;

    public void SetLocation(sbyte x, sbyte y, sbyte layer) =>
        SetLocation(new MapCoordinate(x, y, layer));

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
