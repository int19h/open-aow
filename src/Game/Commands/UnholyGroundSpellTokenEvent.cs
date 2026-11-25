#nullable disable warnings
// Translated from UnHolyG.pas.
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

public class UnholyGroundSpellTokenEvent(CustomNode? owner) : GlobalTargetSpellTokenEvent(owner)
{

    // UnHolyG.pas:288
    public new const int ClassId = 135_943;

    // UnHolyG.pas:340
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        CreateUnholyGround(source);
        _ = GetEngine().StrategicMap.ValidateTerrainObservation(source.Player, Location.X, Location.Y, L: Location.MapLevel, radius: 1);
        GetEngine().GamePack.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        return false;
    }

    // UnHolyG.pas:293
    public void CreateUnholyGround(Troop source)
    {
        HexNumberWalkData[] walkCell = new HexNumberWalkData[1];
        ref HexNumberWalkData hexNumberWalkData = ref walkCell[0];
        HexNumberWalkData walk = hexNumberWalkData;
        MapLevel level = GetEngine().StrategicMap.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out hexNumberWalkData, Location.X, Location.Y, radius: 0);
        UnholyGround.Resource resource = GetEngine().GamePack.MapObjectResourceList.FindChild<UnholyGround.Resource>(UnholyGround.Resource.ClassId);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField mapField = level.GetField(walk.X, walk.Y);
                if (GetEngine().GamePack.SpellControl.GetSpell<GlobalTargetSpell>(14).ValidTargetMapField(mapField) && mapField.GetArmyHexSprite() is null)
                {
                    mapField.RestoreTerrain();
                    UnholyGround ground = resource.CreateResourceUser<UnholyGround>();
                    ground.Duration = (3 + ((GetEngine().StrategicMap.Random(11) - 5) / 5)).TruncateToSByte();
                    ground.Player = source.Player;
                    ground.OriginalOverlay = mapField.OverlayType;
                    GetEngine().StrategicMap.ClearTerrain(walk.X, walk.Y, L: Location.MapLevel, radius: 0, terrainType: -1);
                    if (GetEngine().StrategicMap.PlaceHex(ground, walk.X, walk.Y, L: Location.MapLevel))
                    {
                        ground.Mode = UnholySummonings.Summoning;
                        ImageSequenceAnimation animation = new(null, show: ground.ShowSummonAnimation, newFrame: ground.NewFrameSummonAnimation) { Holder = CreationHolder() };
                        animation.SetImageSequence(GetEngine().GamePack.SpellControl[14].CastImages.ImageSequences[0]);
                        animation.FrameCounter = -GetEngine().StrategicMap.Random(15);
                        animation.Tag = PlayerIdExtensions.ToOriginal(source.Player);
                        _ = GetEngine().StrategicMap.PlaceHex(animation, walk.X, walk.Y, L: Location.MapLevel);
                    }
                    else
                    {
                        ground?.Dispose();
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref hexNumberWalkData);
        }
    }
}
