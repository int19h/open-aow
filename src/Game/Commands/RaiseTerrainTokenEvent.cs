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
        _ = StrategicMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, L: Location.MapLevel, radius: 1);
        GamePack.Current.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        return false;
    }

    // Mountain.pas:261
    public void RaiseTerrain(Troop caster)
    {
        MapObjectResourceList resources = GamePack.Current.MapObjectResourceList;
        MapLevel level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Location.X, Location.Y, radius: 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField field = level.Field[walk.X, walk.Y];
                if (GamePack.Current.SpellControl.GetSpell<GlobalTargetSpell>(19).ValidTargetMapField(field))
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
                        mountain.Duration = (sbyte)(((StrategicMap.Current.Random(11) - 5) / 5) + 3);
                        mountain.Player = caster.Player;
                        mountain.OriginalOverlay = field.OverlayType;
                        StrategicMap.Current.ClearTerrain(walk.X, walk.Y, L: Location.MapLevel, radius: 0, terrainType: -1);
                        if (StrategicMap.Current.PlaceHex(mountain, walk.X, walk.Y, L: Location.MapLevel))
                        {
                            mountain.Mode |= RaisedMountainModes.Raising;
                            ImageSequenceAnimation animation = new(null, show: mountain.ShowRaiseTerrainAnimation, newFrame: mountain.NewFrameRaiseTerrainAnimation) { Holder = CreationHolder() };
                            animation.SetImageSequence(GamePack.Current.SpellControl[19].CastImages.ImageSequences[0]);
                            animation.FrameCounter = -StrategicMap.Current.Random(15);
                            _ = StrategicMap.Current.PlaceHex(animation, walk.X, walk.Y, L: Location.MapLevel);
                        }
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }
}
