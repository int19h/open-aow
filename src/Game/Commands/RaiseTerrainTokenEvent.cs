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

public partial class RaiseTerrainTokenEvent : GlobalTargetSpellTokenEvent
{
    public RaiseTerrainTokenEvent(ECustomNode owner) : base(owner)
    { }

    // Mountain.pas:258
    public new const int ClassId = 135946;

    // Mountain.pas:317
    public override bool Process(Troop source, int value)
    {
        base.Process(source, value);
        RaiseTerrain(source);
        StrategicMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 1);
        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        return false;
    }

    // Mountain.pas:261
    public void RaiseTerrain(Troop caster)
    {
        MapObjectResourceList resources = default;
        MapLevel level = default;
        HexNumberWalkData walk = default;
        MapField field = default;
        RaisedMountain mountain = default;
        MapObjectResource resource = default;
        int I = default;
        bool found = default;
        ImageSequenceAnimation animation = default;
        resources = GamePack.Current.MapObjectResourceList;
        level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out walk, Location.X, Location.Y, 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                field = level.Field[walk.X, walk.Y];
                resource = null;
                if (((GlobalTargetSpell)GamePack.Current.SpellControl.GetSpell(19)).ValidTargetMapField(field))
                {
                    found = false;
                    I = resources.Count;
                    while (!found && I != 0)
                    {
                        I -= 1;
                        resource = resources[I];
                        found = resource.EngineClass.ClassId == 131435 && resource.TerrainList.UsedTerrain() == 1;
                    }

                    if (found)
                    {
                        field.RestoreTerrain();
                        mountain = (RaisedMountain)resource.CreateResourceUser();
                        mountain.Duration = (sbyte)((StrategicMap.Current.Random(11) - 5) / 5 + 3);
                        mountain.Player = caster.Player;
                        mountain.OriginalOverlay = field.OverlayType;
                        StrategicMap.Current.ClearTerrain(walk.X, walk.Y, Location.MapLevel, 0, -1);
                        if (StrategicMap.Current.PlaceHex(mountain, walk.X, walk.Y, Location.MapLevel))
                        {
                            mountain.Mode = mountain.Mode + Set<RaisedMountainMode>.Of(RaisedMountainMode.Raising);
                            animation = new ImageSequenceAnimation(null);
                            animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(19).CastImages.ImageSequences[0]);
                            animation.OnShow += mountain.ShowRaiseTerrainAnimation;
                            animation.OnNewFrame += mountain.NewFrameRaiseTerrainAnimation;
                            animation.FrameCounter = -StrategicMap.Current.Random(15);
                            StrategicMap.Current.PlaceHex(animation, walk.X, walk.Y, Location.MapLevel);
                        }
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // NetworkE.pas:230

}
