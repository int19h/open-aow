#nullable disable warnings
// Translated from HolyG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
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
        _ = GetEngine().StrategicMap.ValidateTerrainObservation(source.Player, Location.X, Location.Y, L: Location.MapLevel, radius: 1);
        GetEngine().GamePack.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        return false;
    }

    // HolyG.pas:285
    public void CreateHolyGround(Troop caster)
    {
        MapLevel level = GetEngine().StrategicMap.MapContainer.GetMapLevel(Location.MapLevel);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Location.X, Location.Y, radius: 0);
        HolyGround.Resource resource = GetEngine().GamePack.MapObjectResourceList.FindChild<HolyGround.Resource>(HolyGround.Resource.ClassId);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField field = level.Field[walk.X, walk.Y];
                if (GetEngine().GamePack.SpellControl.GetSpell<GlobalTargetSpell>(13).ValidTargetMapField(field))
                {
                    if (field.GetArmyHexSprite() is null)
                    {
                        field.RestoreTerrain();
                        HolyGround ground = resource.CreateResourceUser<HolyGround>();
                        ground.Duration = (((GetEngine().StrategicMap.Random(11) - 5) / 5) + 3).TruncateToSByte();
                        ground.Player = caster.Player;
                        ground.OriginalOverlay = field.OverlayType.ToOriginal();
                        GetEngine().StrategicMap.ClearTerrain(walk.X, walk.Y, L: Location.MapLevel, radius: 0, terrainType: -1);
                        if (GetEngine().StrategicMap.PlaceHex(ground, walk.X, walk.Y, L: Location.MapLevel))
                        {
                            ground.Mode = HolySummonings.Summoning;
                            ImageSequenceAnimation animation = new(null, show: ground.ShowSummonAnimation, newFrame: ground.NewFrameSummonAnimation) { Holder = CreationHolder() };
                            animation.SetImageSequence(GetEngine().GamePack.SpellControl[13].CastImages.ImageSequences[0]);
                            animation.FrameCounter = -GetEngine().StrategicMap.Random(15);
                            animation.Tag = PlayerIdExtensions.ToOriginal(caster.Player);
                            _ = GetEngine().StrategicMap.PlaceHex(animation, walk.X, walk.Y, L: Location.MapLevel);
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
