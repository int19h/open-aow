#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// Location: packed X/Y/layer coordinates
public partial class GlobalTargetSpellTokenEvent(CustomNode owner) : SpellTokenEvent(owner)
{

    // GlobalTargetSpells.pas:401
    public new const int ClassId = 135976;

    public MapCoordinate Location { get; private set; }

    // GlobalTargetSpells.pas:406
    // packed coordinates
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int location = persister.Int32("Location", value: StoredCoordinateImage.From(Location).ToInt32());
        Location = StoredCoordinateImage.FromInt32(location).Location;
    }

    // GlobalTargetSpells.pas:412
    public void SetLocation(MapCoordinate location) => Location = location;

    public void SetLocation(sbyte x, sbyte y, sbyte layer) =>
        SetLocation(new MapCoordinate(x, y, layer));

    // GlobalTargetSpells.pas:419
    public void PlayDefaultEffects(sbyte X, sbyte Y, sbyte layer)
    {
        ImageSequenceAnimation animation;
        if (GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] is not null)
        {
            animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
            animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
            _ = StrategicMap.Current.PlaceHex(animation, X, Y, layer);
        }

        _ = GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
    }

    // NetworkE.pas:230

}
