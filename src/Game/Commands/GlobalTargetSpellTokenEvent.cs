#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// Location: packed X/Y/layer coordinates
public class GlobalTargetSpellTokenEvent(CustomNode? owner) : SpellTokenEvent(owner)
{

    // GlobalTargetSpells.pas:401
    public new const int ClassId = 135_976;

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

    public void SetLocation(int x, int y, int layer)
        => SetLocation(new MapCoordinate((sbyte)x, (sbyte)y, (sbyte)layer));

    // GlobalTargetSpells.pas:419
    public void PlayDefaultEffects(int X, int Y, int layer)
    {
        GamePack gamePack = GetEngine().GamePack;
        if (gamePack.SpellControl[EventSpellId].CastImages.ImageSequences[0] is not null)
        {
            ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
            animation.SetImageSequence(gamePack.SpellControl[EventSpellId].CastImages.ImageSequences[0]);
            _ = GetEngine().StrategicMap.PlaceHex(animation, X, Y, L: layer);
        }

        _ = GetEngine().GamePack.SpellControl[EventSpellId].CastSounds.Play(0);
    }
}
