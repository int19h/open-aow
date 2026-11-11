// Translated from PoisonP.pas.
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

public partial class PoisonPlantSpellTokenEvent : GlobalTargetSpellTokenEvent
{
    public PoisonPlantSpellTokenEvent(ECustomNode owner) : base(owner)
    { }

    // PoisonP.pas:303
    public new const int ClassID = 135941;

    // PoisonP.pas:361
    public override bool Process(Troop source, int value)
    {
        base.Process(source, value);
        CreatePoisonPlant(source);
        AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
        return false;
    }

    // PoisonP.pas:308
    public void CreatePoisonPlant(Troop Source)
    {
        MapLevel Level = default;
        HexNumberWalkData Walk = default;
        PoisonPlant.Resource Resource = default;
        AoWMapField MapF = default;
        PoisonPlant Plant = default;
        ImageSequenceAnimation Animation = default;
        TwinkleAnimation Twinkle = default;
        ref var walk = ref Walk;
        Level = AoWHSMap.Current.MapContainer.MapLevels[FLocation.L];
        HexNumberWalkData.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
        Resource = (PoisonPlant.Resource)AoWHSSet.Current.MapObjectResourceList.FindChild(131999);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < Level.Width && walk.Y < Level.Height)
            {
                MapF = (AoWMapField)Level.GetField(walk.X, walk.Y);
                if (((GlobalTargetSpell)AoWHSSet.Current.FSpellControl.GetSpell(12)).ValidTargetMapF(MapF) && MapF.GetArmyHS() == null)
                {
                    MapF.RestoreTerrain();
                    Plant = (PoisonPlant)Resource.CreateResourceUser();
                    Plant.FDaysRemaining = (sbyte)(3 + (AoWHSMap.Current.Random(11) - 5) / 5);
                    Plant.FCasterPlayer = Source.FPlayer;
                    Plant.FOriginalOverlay = MapF.OverlayType;
                    AoWHSMap.Current.ClearTerrain(walk.X, walk.Y, FLocation.L, 0, -1);
                    if (AoWHSMap.Current.PlaceHX(Plant, walk.X, walk.Y, FLocation.L))
                    {
                        Plant.FPlantStatus = Set<PoisonPlantStatusElement>.Of(PoisonPlantStatusElement.Summoning);
                        Animation = new ImageSequenceAnimation(null);
                        Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(12).FCastImages.ImageSequences[0]);
                        Animation.FOnShow = Plant.ShowSummonAnimation;
                        Animation.FOnNewFrame = Plant.NewFrameSummonAnimation;
                        Animation.FFrameCounter = -AoWHSMap.Current.Random(15);
                        Animation.FAnimationLevel = 27;
                        AoWHSMap.Current.PlaceHX(Animation, walk.X, walk.Y, FLocation.L);
                        Twinkle = new TwinkleAnimation(null);
                        Twinkle.Setup(AoWHSSet.Current.FSpellControl.GetSpell(2).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(), 2, 6, Math.Abs(Animation.FFrameCounter + 5), Math.Abs(Animation.FFrameCounter + 5) / 2);
                        AoWHSMap.Current.PlaceHX(Twinkle, walk.X, walk.Y, FLocation.L);
                    }
                    else
                    {
                        Plant?.Dispose();
                    }
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }
    }

    // NetworkE.pas:230

}
