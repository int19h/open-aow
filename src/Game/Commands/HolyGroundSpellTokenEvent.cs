#nullable disable warnings
// Translated from HolyG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;
using Aow.Rtl;

namespace Aow.Game.Commands;

public class HolyGroundSpellTokenEvent(CustomNode? owner) : GlobalTargetSpellTokenEvent(owner)
{

    // HolyG.pas:282
    public new const int ClassId = 135_942;

    // HolyG.pas:329
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        CreateHolyGround(source);
        _ = StrategicMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, L: Location.MapLevel, radius: 1);
        GamePack.Current.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        return false;
    }

    // HolyG.pas:285
    public void CreateHolyGround(Troop caster)
    {
        MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(Location.MapLevel);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Location.X, Location.Y, radius: 0);
        HolyGround.Resource resource = GamePack.Current.MapObjectResourceList.FindChild<HolyGround.Resource>(HolyGround.Resource.ClassId);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField field = level.Field[walk.X, walk.Y];
                if (GamePack.Current.SpellControl.GetSpell<GlobalTargetSpell>(13).ValidTargetMapField(field))
                {
                    if (field.GetArmyHexSprite() is null)
                    {
                        field.RestoreTerrain();
                        HolyGround ground = resource.CreateResourceUser<HolyGround>();
                        ground.Duration = (((StrategicMap.Current.Random(11) - 5) / 5) + 3).TruncateToSByte();
                        ground.Player = caster.Player;
                        ground.OriginalOverlay = field.OverlayType;
                        StrategicMap.Current.ClearTerrain(walk.X, walk.Y, L: Location.MapLevel, radius: 0, terrainType: -1);
                        if (StrategicMap.Current.PlaceHex(ground, walk.X, walk.Y, L: Location.MapLevel))
                        {
                            ground.Mode = HolySummonings.Summoning;
                            ImageSequenceAnimation animation = new(null, show: ground.ShowSummonAnimation, newFrame: ground.NewFrameSummonAnimation) { Holder = CreationHolder() };
                            animation.SetImageSequence(GamePack.Current.SpellControl[13].CastImages.ImageSequences[0]);
                            animation.FrameCounter = -StrategicMap.Current.Random(15);
                            animation.Tag = caster.Player;
                            _ = StrategicMap.Current.PlaceHex(animation, walk.X, walk.Y, L: Location.MapLevel);
                        }
                        else
                        {
                            ground?.Dispose();
                        }
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }
}
