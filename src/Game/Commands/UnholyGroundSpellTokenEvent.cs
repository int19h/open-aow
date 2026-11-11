// Translated from UnHolyG.pas.
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

public partial class UnholyGroundSpellTokenEvent : GlobalTargetSpellTokenEvent
{
    public UnholyGroundSpellTokenEvent(ECustomNode owner) : base(owner)
    { }

    // UnHolyG.pas:288
    public new const int ClassID = 135943;

    // UnHolyG.pas:340
    public override bool Process(Troop source, int value)
    {
        base.Process(source, value);
        CreateUnholyGround(source);
        AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
        return false;
    }

    // UnHolyG.pas:293
    public void CreateUnholyGround(Troop Source)
    {
        MapLevel Level = default;
        HexNumberWalkData[] Walk__cell = new HexNumberWalkData[1];
        ref HexNumberWalkData Walk = ref Walk__cell[0];
        UnholyGround.Resource Resource = default;
        AoWMapField MapF = default;
        UnholyGround Ground = default;
        ImageSequenceAnimation Anim = default;
        var walk = Walk;
        Level = AoWHSMap.Current.MapContainer.MapLevels[FLocation.L];
        HexNumberWalkData.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
        Resource = (UnholyGround.Resource)AoWHSSet.Current.MapObjectResourceList.FindChild(131986);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < Level.Width && walk.Y < Level.Height)
            {
                MapF = (AoWMapField)Level.GetField(walk.X, walk.Y);
                if (((GlobalTargetSpell)AoWHSSet.Current.FSpellControl.GetSpell(14)).ValidTargetMapF(MapF) && MapF.GetArmyHS() == null)
                {
                    MapF.RestoreTerrain();
                    Ground = (UnholyGround)Resource.CreateResourceUser();
                    Ground.FDuration = (sbyte)(3 + (AoWHSMap.Current.Random(11) - 5) / 5);
                    Ground.FPlayer = Source.FPlayer;
                    Ground.FOriginalOverlay = MapF.OverlayType;
                    AoWHSMap.Current.ClearTerrain(walk.X, walk.Y, FLocation.L, 0, -1);
                    if (AoWHSMap.Current.PlaceHX(Ground, walk.X, walk.Y, FLocation.L))
                    {
                        Ground.FMode = Set<UnholySummoning>.Of(UnholySummoning.Summoning);
                        Anim = new ImageSequenceAnimation(null);
                        Anim.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(14).FCastImages.ImageSequences[0]);
                        Anim.FOnShow = Ground.ShowSummonAnimation;
                        Anim.FOnNewFrame = Ground.NewFrameSummonAnimation;
                        Anim.FFrameCounter = -AoWHSMap.Current.Random(15);
                        Anim.FTag = Source.FPlayer;
                        AoWHSMap.Current.PlaceHX(Anim, walk.X, walk.Y, FLocation.L);
                    }
                    else
                    {
                        Ground?.Dispose();
                    }
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }
    }

    // NetworkE.pas:230

}
