#nullable disable
// Translated from Storms.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// storm, X, Y, layer
public class StormSpellTokenEvent(CustomNode owner) : SpellTokenEvent(owner)
{

    // Storms.pas:730
    public new const int ClassId = 135937;

    private Storm _storm;

    public MapCoordinate Location { get; set; }

    // Storms.pas:735
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Location = PersistSupport.Coordinate(persister, name: "Location", value: Location, width: CoordinateWidth.Byte);
    }

    // Storms.pas:743
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        if (value == 0)
        {
            _storm = GamePack.Current.SpellControl.GetSpell<StormSpell>(EventSpellId).CreateStorm();
            _storm.InitializeStorm(source.Player, Location.X, Location.Y, L: Location.MapLevel);
            GamePack.Current.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        }
        if (!_storm.UpdateStorm())
        {
            _storm.FinalizeStorm();
            _storm?.Dispose();
            _storm = null;
            return false;
        }
        else
        {
            return true;
        }
    }
}
