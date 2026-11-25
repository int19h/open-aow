#nullable disable warnings
// Translated from Mountain.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;
using Aow.Rtl;

namespace Aow.Game.Commands;

public class RaiseTerrainTokenEvent(CustomNode? owner) : GlobalTargetSpellTokenEvent(owner)
{

    // Mountain.pas:258
    public new const int ClassId = 135_946;

    // Mountain.pas:317
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        RaiseTerrain(source);
        _ = GetEngine().StrategicMap.ValidateTerrainObservation(source.Player, Location.X, Location.Y, L: Location.MapLevel, radius: 1);
        GetEngine().GamePack.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        return false;
    }

    // Mountain.pas:261
    public void RaiseTerrain(Troop caster)
    {
        MapObjectResourceList resources = GetEngine().GamePack.MapObjectResourceList;
        MapLevel level = GetEngine().StrategicMap.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Location.X, Location.Y, radius: 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField field = level.Field[walk.X, walk.Y];
                if (GetEngine().GamePack.SpellControl.GetSpell<GlobalTargetSpell>(19).ValidTargetMapField(field))
                {
                    bool found = false;
                    int i = resources.Count;
                    MapObjectResource? resource = null;
                    while (!found && i != 0)
                    {
                        i -= 1;
                        resource = resources[i];
                        found = resource.EngineClass.ClassId == RaisedMountain.Resource.ClassId && resource.TerrainList.UsedTerrain() == 1;
                    }

                    if (found)
                    {
                        field.RestoreTerrain();
                        RaisedMountain mountain = resource.CreateResourceUser<RaisedMountain>();
                        mountain.Duration = (((GetEngine().StrategicMap.Random(11) - 5) / 5) + 3).TruncateToSByte();
                        mountain.Player = caster.Player;
                        mountain.OriginalOverlay = field.OverlayType;
                        GetEngine().StrategicMap.ClearTerrain(walk.X, walk.Y, L: Location.MapLevel, radius: 0, terrainType: -1);
                        if (GetEngine().StrategicMap.PlaceHex(mountain, walk.X, walk.Y, L: Location.MapLevel))
                        {
                            mountain.Mode |= RaisedMountainModes.Raising;
                            ImageSequenceAnimation animation = new(null, show: mountain.ShowRaiseTerrainAnimation, newFrame: mountain.NewFrameRaiseTerrainAnimation) { Holder = CreationHolder() };
                            animation.SetImageSequence(GetEngine().GamePack.SpellControl[19].CastImages.ImageSequences[0]);
                            animation.FrameCounter = -GetEngine().StrategicMap.Random(15);
                            _ = GetEngine().StrategicMap.PlaceHex(animation, walk.X, walk.Y, L: Location.MapLevel);
                        }
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }
}
