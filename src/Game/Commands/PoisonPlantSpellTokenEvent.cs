#nullable disable
// Translated from PoisonP.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;

namespace Aow.Game.Commands;

public class PoisonPlantSpellTokenEvent(CustomNode owner) : GlobalTargetSpellTokenEvent(owner)
{

    // PoisonP.pas:303
    public new const int ClassId = 135_941;

    // PoisonP.pas:361
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        CreatePoisonPlant(source);
        _ = StrategicMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, L: Location.MapLevel, radius: 1);
        GamePack.Current.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        return false;
    }

    // PoisonP.pas:308
    public void CreatePoisonPlant(Troop source)
    {
        HexNumberWalkData hexNumberWalkData = default;
        ref HexNumberWalkData walk = ref hexNumberWalkData;
        MapLevel level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out hexNumberWalkData, Location.X, Location.Y, radius: 0);
        PoisonPlant.Resource resource = GamePack.Current.MapObjectResourceList.FindChild<PoisonPlant.Resource>(PoisonPlant.Resource.ClassId);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField mapField = level.GetField(walk.X, walk.Y);
                if (GamePack.Current.SpellControl.GetSpell<GlobalTargetSpell>(12).ValidTargetMapField(mapField) && mapField.GetArmyHexSprite() is null)
                {
                    mapField.RestoreTerrain();
                    PoisonPlant plant = resource.CreateResourceUser<PoisonPlant>();
                    plant.DaysRemaining = (sbyte)(3 + ((StrategicMap.Current.Random(11) - 5) / 5));
                    plant.CasterPlayer = source.Player;
                    plant.OriginalOverlay = mapField.OverlayType;
                    StrategicMap.Current.ClearTerrain(walk.X, walk.Y, L: Location.MapLevel, radius: 0, terrainType: -1);
                    if (StrategicMap.Current.PlaceHex(plant, walk.X, walk.Y, L: Location.MapLevel))
                    {
                        plant.PlantStatus = PoisonPlantStatuses.Summoning;
                        ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                        animation.SetImageSequence(GamePack.Current.SpellControl[12].CastImages.ImageSequences[0]);
                        animation.SetShowHandler(plant.ShowSummonAnimation);
                        animation.SetNewFrameHandler(plant.NewFrameSummonAnimation);
                        animation.FrameCounter = -StrategicMap.Current.Random(15);
                        animation.AnimationLevel = 27;
                        _ = StrategicMap.Current.PlaceHex(animation, walk.X, walk.Y, L: Location.MapLevel);
                        TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
                        twinkle.Setup(GamePack.Current.SpellControl[2].CastImages.ImageSequences[0], mode: TwinkleModes.None, count: 2, radius: 6, duration: Math.Abs(animation.FrameCounter + 5), startDelay: Math.Abs(animation.FrameCounter + 5) / 2);
                        _ = StrategicMap.Current.PlaceHex(twinkle, walk.X, walk.Y, L: Location.MapLevel);
                    }
                    else
                    {
                        plant?.Dispose();
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref hexNumberWalkData);
        }
    }
}
