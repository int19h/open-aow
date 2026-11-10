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

public partial class HolyGroundSpellTE : GlobalTargetSpellTE
{
    public HolyGroundSpellTE(ECustomNode owner) : base(owner)
    { }

    // HolyG.pas:282
    public new const int ClassID = 135942;

    // HolyG.pas:329
    public override bool Process(AbstractUnit source, int value)
    {
        base.Process(source, value);
        CreateHolyGround(source);
        AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
        return false;
    }

    // HolyG.pas:285
    public void CreateHolyGround(AbstractUnit Caster)
    {
        MapLevel Level = default;
        HNWalkData Walk = default;
        HolyGround.Resource Resource = default;
        AoWMapField Field = default;
        HolyGround Ground = default;
        ImageSequenceAnimation Anim = default;
        Level = AoWHSMap.Current.MapContainer.GetMapLevel(FLocation.L);
        HNWalkData.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
        Resource = (HolyGround.Resource)AoWHSSet.Current.MapObjectResourceList.FindChild(131988);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Field = (AoWMapField)Level.Field[Walk.X, Walk.Y];
                if (((GlobalTargetSpell)AoWHSSet.Current.FSpellControl.GetSpell(13)).ValidTargetMapF(Field))
                {
                    if (Field.GetArmyHS() == null)
                    {
                        Field.RestoreTerrain();
                        Ground = (HolyGround)Resource.CreateResourceUser();
                        Ground.FDuration = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
                        Ground.FPlayer = Caster.FPlayer;
                        Ground.FOriginalOverlay = Field.OverlayType;
                        AoWHSMap.Current.ClearTerrain(Walk.X, Walk.Y, FLocation.L, 0, -1);
                        if (AoWHSMap.Current.PlaceHX(Ground, Walk.X, Walk.Y, FLocation.L))
                        {
                            Ground.FMode = Set<HolySummoning>.Of(HolySummoning.Summoning);
                            Anim = new ImageSequenceAnimation(null);
                            Anim.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(13).FCastImages.ImageSequences[0]);
                            Anim.FOnShow = Ground.ShowSummonAnimation;
                            Anim.FOnNewFrame = Ground.NewFrameSummonAnimation;
                            Anim.FFrameCounter = -AoWHSMap.Current.Random(15);
                            Anim.FTag = Caster.FPlayer;
                            AoWHSMap.Current.PlaceHX(Anim, Walk.X, Walk.Y, FLocation.L);
                        }
                        else
                        {
                            Ground?.Dispose();
                        }
                    }
                }
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }
    }

    // NetworkE.pas:230

}
