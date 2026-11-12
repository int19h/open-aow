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

public partial class UnholyGroundSpellTokenEvent : GlobalTargetSpellTokenEvent
{
    public UnholyGroundSpellTokenEvent(ECustomNode owner) : base(owner)
    { }

    // UnHolyG.pas:288
    public new const int ClassId = 135943;

    // UnHolyG.pas:340
    public override bool Process(Troop source, int value)
    {
        base.Process(source, value);
        CreateUnholyGround(source);
        StrategicMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 1);
        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        return false;
    }

    // UnHolyG.pas:293
    public void CreateUnholyGround(Troop source)
    {
        MapLevel level = default;
        HexNumberWalkData[] walkCell = new HexNumberWalkData[1];
        ref HexNumberWalkData Walk = ref walkCell[0];
        UnholyGround.Resource resource = default;
        MapField mapField = default;
        UnholyGround ground = default;
        ImageSequenceAnimation animation = default;
        var walk = Walk;
        level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(ref Walk, Location.X, Location.Y, 0);
        resource = (UnholyGround.Resource)GamePack.Current.MapObjectResourceList.FindChild(131986);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                mapField = level.GetField(walk.X, walk.Y);
                if (((GlobalTargetSpell)GamePack.Current.SpellControl.GetSpell(14)).ValidTargetMapField(mapField) && mapField.GetArmyHexSprite() == null)
                {
                    mapField.RestoreTerrain();
                    ground = (UnholyGround)resource.CreateResourceUser();
                    ground.Duration = (sbyte)(3 + (StrategicMap.Current.Random(11) - 5) / 5);
                    ground.Player = source.Player;
                    ground.OriginalOverlay = mapField.OverlayType;
                    StrategicMap.Current.ClearTerrain(walk.X, walk.Y, Location.MapLevel, 0, -1);
                    if (StrategicMap.Current.PlaceHex(ground, walk.X, walk.Y, Location.MapLevel))
                    {
                        ground.Mode = Set<UnholySummoning>.Of(UnholySummoning.Summoning);
                        animation = new ImageSequenceAnimation(null);
                        animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(14).CastImages.ImageSequences[0]);
                        animation.OnShow = ground.ShowSummonAnimation;
                        animation.OnNewFrame = ground.NewFrameSummonAnimation;
                        animation.FrameCounter = -StrategicMap.Current.Random(15);
                        animation.Tag = source.Player;
                        StrategicMap.Current.PlaceHex(animation, walk.X, walk.Y, Location.MapLevel);
                    }
                    else
                    {
                        ground?.Dispose();
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref Walk);
        }
    }

    // NetworkE.pas:230

}
