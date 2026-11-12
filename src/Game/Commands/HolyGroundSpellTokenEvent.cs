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

public partial class HolyGroundSpellTokenEvent : GlobalTargetSpellTokenEvent
{
    public HolyGroundSpellTokenEvent(ECustomNode owner) : base(owner)
    { }

    // HolyG.pas:282
    public new const int ClassId = 135942;

    // HolyG.pas:329
    public override bool Process(Troop source, int value)
    {
        base.Process(source, value);
        CreateHolyGround(source);
        StrategicMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 1);
        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        return false;
    }

    // HolyG.pas:285
    public void CreateHolyGround(Troop caster)
    {
        MapLevel level = default;
        HexNumberWalkData walk = default;
        HolyGround.Resource resource = default;
        MapField field = default;
        HolyGround ground = default;
        ImageSequenceAnimation animation = default;
        level = StrategicMap.Current.MapContainer.GetMapLevel(Location.MapLevel);
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, Location.X, Location.Y, 0);
        resource = (HolyGround.Resource)GamePack.Current.MapObjectResourceList.FindChild(131988);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                field = level.Field[walk.X, walk.Y];
                if (((GlobalTargetSpell)GamePack.Current.SpellControl.GetSpell(13)).ValidTargetMapField(field))
                {
                    if (field.GetArmyHexSprite() == null)
                    {
                        field.RestoreTerrain();
                        ground = (HolyGround)resource.CreateResourceUser();
                        ground.Duration = (sbyte)((StrategicMap.Current.Random(11) - 5) / 5 + 3);
                        ground.Player = caster.Player;
                        ground.OriginalOverlay = field.OverlayType;
                        StrategicMap.Current.ClearTerrain(walk.X, walk.Y, Location.MapLevel, 0, -1);
                        if (StrategicMap.Current.PlaceHex(ground, walk.X, walk.Y, Location.MapLevel))
                        {
                            ground.Mode = Set<HolySummoning>.Of(HolySummoning.Summoning);
                            animation = new ImageSequenceAnimation(null);
                            animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(13).CastImages.ImageSequences[0]);
                            animation.OnShow = ground.ShowSummonAnimation;
                            animation.OnNewFrame = ground.NewFrameSummonAnimation;
                            animation.FrameCounter = -StrategicMap.Current.Random(15);
                            animation.Tag = caster.Player;
                            StrategicMap.Current.PlaceHex(animation, walk.X, walk.Y, Location.MapLevel);
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

    // NetworkE.pas:230

}
