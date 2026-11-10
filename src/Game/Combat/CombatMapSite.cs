
namespace Aow.Game.Combat;

// The combat hex as the tactical map generator sees it when it picks the map file
// (AoWTC.pas:4156). A kind reads the combat state and the neighbor fields, and reports back the
// terrain mode, whether the map lays out the combat's items and which neighbor tags it fixes.
public sealed class CombatMapSite(bool siteCombat, bool randomTerrain, Func<int, Type, bool> neighborHolds)
{
    // The combat is an exploration site fight.
    public bool SiteCombat => siteCombat;

    // The generator randomizes the terrain around the map. A kind turns it on or off.
    public bool RandomTerrain { get; set; } = randomTerrain;

    // The map lays out the items of the combat.
    public bool TakesCombatItems { get; set; }

    // Bit I set: the generator fixes the terrain tag of neighbor I (1 to 6).
    public int FixedTags { get; private set; }

    // Neighbor Direction (1 to 6) of the combat hex holds a sprite of Kind that is not transparent.
    public bool NeighborHolds(int Direction, Type Kind) => neighborHolds(Direction, Kind);

    public void FixTag(int Direction) => FixedTags |= 1 << Direction;
}
