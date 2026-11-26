// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

// A callback of GameMap.ChangeTerrain that sets a field's new terrain. The terrain it reads and writes is
// the original number, -1 (no terrain) or 0 to 15, and the overlay it reads is -1 (no overlay) or 0 to 11.
// ChangeTerrain checks every answer against that range before it locks the map or changes a field.
public delegate void ChangeTerrainProc(MapField MapF, ref sbyte TerrainType, in sbyte OverlayType);
