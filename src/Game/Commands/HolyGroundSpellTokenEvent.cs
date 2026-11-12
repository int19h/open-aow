// Translated from HolyG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
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
        AoWHSMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 1);
        AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        return false;
    }

    // HolyG.pas:285
    public void CreateHolyGround(Troop caster)
    {
        MapLevel level = default;
        HexNumberWalkData walk = default;
        HolyGround.Resource resource = default;
        AoWMapField field = default;
        HolyGround ground = default;
        ImageSequenceAnimation animation = default;
        level = AoWHSMap.Current.MapContainer.GetMapLevel(Location.MapLevel);
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, Location.X, Location.Y, 0);
        resource = (HolyGround.Resource)AoWHSSet.Current.MapObjectResourceList.FindChild(131988);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                field = (AoWMapField)level.Field[walk.X, walk.Y];
                if (((GlobalTargetSpell)AoWHSSet.Current.SpellControl.GetSpell(13)).ValidTargetMapField(field))
                {
                    if (field.GetArmyHexSprite() == null)
                    {
                        field.RestoreTerrain();
                        ground = (HolyGround)resource.CreateResourceUser();
                        ground.Duration = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
                        ground.Player = caster.Player;
                        ground.OriginalOverlay = field.OverlayType;
                        AoWHSMap.Current.ClearTerrain(walk.X, walk.Y, Location.MapLevel, 0, -1);
                        if (AoWHSMap.Current.PlaceHex(ground, walk.X, walk.Y, Location.MapLevel))
                        {
                            ground.Mode = Set<HolySummoning>.Of(HolySummoning.Summoning);
                            animation = new ImageSequenceAnimation(null);
                            animation.SetImageSequence(AoWHSSet.Current.SpellControl.GetSpell(13).CastImages.ImageSequences[0]);
                            animation.OnShow = ground.ShowSummonAnimation;
                            animation.OnNewFrame = ground.NewFrameSummonAnimation;
                            animation.FrameCounter = -AoWHSMap.Current.Random(15);
                            animation.Tag = caster.Player;
                            AoWHSMap.Current.PlaceHex(animation, walk.X, walk.Y, Location.MapLevel);
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
