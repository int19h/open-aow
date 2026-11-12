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
    public new const int ClassId = 135941;

    // PoisonP.pas:361
    public override bool Process(Troop source, int value)
    {
        base.Process(source, value);
        CreatePoisonPlant(source);
        AoWHSMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 1);
        AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        return false;
    }

    // PoisonP.pas:308
    public void CreatePoisonPlant(Troop source)
    {
        MapLevel level = default;
        HexNumberWalkData Walk = default;
        PoisonPlant.Resource resource = default;
        AoWMapField mapField = default;
        PoisonPlant plant = default;
        ImageSequenceAnimation animation = default;
        TwinkleAnimation twinkle = default;
        ref var walk = ref Walk;
        level = AoWHSMap.Current.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(ref Walk, Location.X, Location.Y, 0);
        resource = (PoisonPlant.Resource)AoWHSSet.Current.MapObjectResourceList.FindChild(131999);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                mapField = (AoWMapField)level.GetField(walk.X, walk.Y);
                if (((GlobalTargetSpell)AoWHSSet.Current.SpellControl.GetSpell(12)).ValidTargetMapField(mapField) && mapField.GetArmyHexSprite() == null)
                {
                    mapField.RestoreTerrain();
                    plant = (PoisonPlant)resource.CreateResourceUser();
                    plant.DaysRemaining = (sbyte)(3 + (AoWHSMap.Current.Random(11) - 5) / 5);
                    plant.CasterPlayer = source.Player;
                    plant.OriginalOverlay = mapField.OverlayType;
                    AoWHSMap.Current.ClearTerrain(walk.X, walk.Y, Location.MapLevel, 0, -1);
                    if (AoWHSMap.Current.PlaceHex(plant, walk.X, walk.Y, Location.MapLevel))
                    {
                        plant.PlantStatus = Set<PoisonPlantStatusElement>.Of(PoisonPlantStatusElement.Summoning);
                        animation = new ImageSequenceAnimation(null);
                        animation.SetImageSequence(AoWHSSet.Current.SpellControl.GetSpell(12).CastImages.ImageSequences[0]);
                        animation.OnShow = plant.ShowSummonAnimation;
                        animation.OnNewFrame = plant.NewFrameSummonAnimation;
                        animation.FrameCounter = -AoWHSMap.Current.Random(15);
                        animation.AnimationLevel = 27;
                        AoWHSMap.Current.PlaceHex(animation, walk.X, walk.Y, Location.MapLevel);
                        twinkle = new TwinkleAnimation(null);
                        twinkle.Setup(AoWHSSet.Current.SpellControl.GetSpell(2).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(), 2, 6, Math.Abs(animation.FrameCounter + 5), Math.Abs(animation.FrameCounter + 5) / 2);
                        AoWHSMap.Current.PlaceHex(twinkle, walk.X, walk.Y, Location.MapLevel);
                    }
                    else
                    {
                        plant?.Dispose();
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref Walk);
        }
    }

    // NetworkE.pas:230

}
