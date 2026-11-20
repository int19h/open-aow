#nullable disable
// Translated from UnHolyG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;
using Aow.Rtl;

namespace Aow.Game.Commands;

public class UnholyGroundSpellTokenEvent(CustomNode owner) : GlobalTargetSpellTokenEvent(owner)
{

    // UnHolyG.pas:288
    public new const int ClassId = 135943;

    // UnHolyG.pas:340
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        CreateUnholyGround(source);
        _ = StrategicMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, L: Location.MapLevel, radius: 1);
        GamePack.Current.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        return false;
    }

    // UnHolyG.pas:293
    public void CreateUnholyGround(Troop source)
    {
        HexNumberWalkData[] walkCell = new HexNumberWalkData[1];
        ref HexNumberWalkData hexNumberWalkData = ref walkCell[0];
        HexNumberWalkData walk = hexNumberWalkData;
        MapLevel level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out hexNumberWalkData, Location.X, Location.Y, radius: 0);
        UnholyGround.Resource resource = GamePack.Current.MapObjectResourceList.FindChild<UnholyGround.Resource>(131986);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField mapField = level.GetField(walk.X, walk.Y);
                if (GamePack.Current.SpellControl.GetSpell<GlobalTargetSpell>(14).ValidTargetMapField(mapField) && mapField.GetArmyHexSprite() is null)
                {
                    mapField.RestoreTerrain();
                    UnholyGround ground = resource.CreateResourceUser<UnholyGround>();
                    ground.Duration = (sbyte)(3 + ((StrategicMap.Current.Random(11) - 5) / 5));
                    ground.Player = source.Player;
                    ground.OriginalOverlay = mapField.OverlayType;
                    StrategicMap.Current.ClearTerrain(walk.X, walk.Y, L: Location.MapLevel, radius: 0, terrainType: -1);
                    if (StrategicMap.Current.PlaceHex(ground, walk.X, walk.Y, L: Location.MapLevel))
                    {
                        ground.Mode = Set<UnholySummoning>.Of(UnholySummoning.Summoning);
                        ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                        animation.SetImageSequence(GamePack.Current.SpellControl[14].CastImages.ImageSequences[0]);
                        animation.OnShow += ground.ShowSummonAnimation;
                        animation.OnNewFrame += ground.NewFrameSummonAnimation;
                        animation.FrameCounter = -StrategicMap.Current.Random(15);
                        animation.Tag = source.Player;
                        _ = StrategicMap.Current.PlaceHex(animation, walk.X, walk.Y, L: Location.MapLevel);
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
