#nullable disable warnings
// Translated from PoisonP.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;
using Aow.Rtl;

namespace Aow.Game.Commands;

public class PoisonPlantSpellTokenEvent(CustomNode? owner) : GlobalTargetSpellTokenEvent(owner)
{

    // PoisonP.pas:303
    public new const int ClassId = 135_941;

    // PoisonP.pas:361
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        CreatePoisonPlant(source);
        _ = GetEngine().StrategicMap.ValidateTerrainObservation(source.Player, Location.X, Location.Y, L: Location.MapLevel, radius: 1);
        GetEngine().GamePack.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        return false;
    }

    // PoisonP.pas:308
    public void CreatePoisonPlant(Troop source)
    {
        HexNumberWalkData hexNumberWalkData = default;
        ref HexNumberWalkData walk = ref hexNumberWalkData;
        MapLevel level = GetEngine().StrategicMap.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out hexNumberWalkData, Location.X, Location.Y, radius: 0);
        PoisonPlant.Resource resource = GetEngine().GamePack.MapObjectResourceList.FindChild<PoisonPlant.Resource>(PoisonPlant.Resource.ClassId);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField mapField = level.GetField(walk.X, walk.Y);
                if (GetEngine().GamePack.SpellControl.GetSpell<GlobalTargetSpell>(12).ValidTargetMapField(mapField) && mapField.GetArmyHexSprite() is null)
                {
                    mapField.RestoreTerrain();
                    PoisonPlant plant = resource.CreateResourceUser<PoisonPlant>();
                    plant.DaysRemaining = (3 + ((GetEngine().StrategicMap.Random(11) - 5) / 5)).TruncateToSByte();
                    plant.CasterPlayer = source.Player;
                    plant.OriginalOverlay = mapField.OverlayType;
                    GetEngine().StrategicMap.ClearTerrain(walk.X, walk.Y, L: Location.MapLevel, radius: 0, terrainType: -1);
                    if (GetEngine().StrategicMap.PlaceHex(plant, walk.X, walk.Y, L: Location.MapLevel))
                    {
                        plant.PlantStatus = PoisonPlantStatuses.Summoning;
                        ImageSequenceAnimation animation = new(null, show: plant.ShowSummonAnimation, newFrame: plant.NewFrameSummonAnimation) { Holder = CreationHolder() };
                        animation.SetImageSequence(GetEngine().GamePack.SpellControl[12].CastImages.ImageSequences[0]);
                        animation.FrameCounter = -GetEngine().StrategicMap.Random(15);
                        animation.AnimationLevel = 27;
                        _ = GetEngine().StrategicMap.PlaceHex(animation, walk.X, walk.Y, L: Location.MapLevel);
                        TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
                        twinkle.Setup(GetEngine().GamePack.SpellControl[2].CastImages.ImageSequences[0], mode: TwinkleModes.None, count: 2, radius: 6, duration: Math.Abs(animation.FrameCounter + 5), startDelay: Math.Abs(animation.FrameCounter + 5) / 2);
                        _ = GetEngine().StrategicMap.PlaceHex(twinkle, walk.X, walk.Y, L: Location.MapLevel);
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
