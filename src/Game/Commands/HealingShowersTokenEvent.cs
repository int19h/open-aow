#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Commands;

// animation
public class HealingShowersTokenEvent(CustomNode? owner) : GlobalTargetSpellTokenEvent(owner)
{

    // GlobalTargetSpells.pas:2138
    public new const int ClassId = 135_975;

    private Animation _animation;

    // GlobalTargetSpells.pas:2190
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        if (value == 0)
        {
            _ = StrategicMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, L: Location.MapLevel, radius: 2);
            if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.WatchingTerrain(Location.X, Location.Y, L: Location.MapLevel, radius: 2))
            {
                _ = GamePack.Current.SpellControl[73].CastSounds.Play(0);
            }

            // The original constructs a plain animation and stores it on the event. The later shower uses an image sequence.
            _animation = new Animation(null)
            {
                Holder = CreationHolder(),
                AnimationLevel = 27
            };
            _animation.SetShowHandler(ShowAnimation);
            _animation.AnimationMap = StrategicMap.Current;
            _animation.Tag = (StrategicMap.Current.ViewWidth & 0xFFFF) | (StrategicMap.Current.ViewHeight << 16);
            _animation.Activate();
            GamePack.Current.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        }

        if (value == 50)
        {
            bool playSound = false;
            _ = StrategicMap.Current.ChangeTerrain(null, Location.X, Location.Y, L: Location.MapLevel, radius: 1, changeTerrainProc: ChangeTerrain, terrainChangedProc: null);
            MapLevel level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Location.X, Location.Y, radius: 0);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && level.Height > walk.Y)
                {
                    if (!level.GetField(walk.X, walk.Y).GetMagicShieldActive())
                    {
                        ArmyHexSprite army = level.GetField(walk.X, walk.Y).GetArmyHexSprite();
                        if (army is not null)
                        {
                            bool healed2 = false;
                            for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
                            {
                                Troop? troopObject = army.Army.Units.GetUnit(i);
                                if (troopObject.GetHitPoints() < troopObject.GetHits())
                                {
                                    troopObject.SetHitPoints((sbyte)(troopObject.GetHitPoints() + 5));
                                    healed2 = true;
                                }

                                if (troopObject.GetAbilityEnabled(96))
                                {
                                    _ = troopObject.RemoveAbility(96);
                                    healed2 = true;
                                }
                            }

                            army.Army.Update();
                            bool healed = healed2;
                            if (healed)
                            {
                                sbyte hexY = army.HexY;
                                sbyte hexX = army.HexX;
                                healed = StrategicMap.Current.VisibleForPlayer(hexX, hexY, army.HexLevel, player: StrategicMap.Current.SeatedPlayer);
                            }

                            if (healed)
                            {
                                bool condition = playSound || (byte)army.Army.Player == (byte)StrategicMap.Current.SeatedPlayer;
                                if (!condition)
                                {
                                    sbyte hexY = army.HexY;
                                    sbyte hexX = army.HexX;
                                    condition = StrategicMap.Current.Watching(hexX, hexY, L: army.HexLevel, radius: 1);
                                }

                                playSound = condition;
                                ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                                animation.SetImageSequence(GamePack.Current.AbilityControl[47].ImageSequences.ImageSequences[0]);
                                animation.AnimationLevel = 27;
                                sbyte armyHexY = army.HexY;
                                sbyte armyHexX = army.HexX;
                                _ = StrategicMap.Current.PlaceHex(animation, armyHexX, armyHexY, L: army.HexLevel);
                            }
                        }

                        HexSprite cloud = level.GetField(walk.X, walk.Y).FindOwnedChild<HexSprite>(World.Terrain.Fire.ClassId);
                        cloud?.Dispose();
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            if (playSound)
            {
                _ = GamePack.Current.AbilityControl[47].SfxLibrary.Play(0);
            }
        }
        if (value == 80)
        {
            _animation?.Dispose();
            return false;
        }
        else
        {
            return true;
        }
    }

    // GlobalTargetSpells.pas:2141
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (StrategicMap.Current.ViewMapLevel == Location.MapLevel)
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out int pixelX, out int pixelY);
            pixelX -= pixelXSurface;
            pixelY -= pixelYSurface;
            int count = GetEngine().Registry.GetDetailLevel() switch
            {
                DetailLevel.Low => 50,
                DetailLevel.High => 160,
                DetailLevel.Normal or _ => 100,
            };
            RandSeed = StrategicMap.Current.InitialRandomSeed;
            for (int i = 0; i <= count; i++)
            {
                int x = Random(100) - 50;
                int y = Random(100) - 50;
                int speed = Random(5) + 5;
                int random = Random(30);
                int frame = animation.FrameCounter - random;
                if (frame is >= 0 and < 50)
                {
                    frame %= 30;
                    x -= Round(frame * 4 * (speed * 0.2));
                    y += Round(frame * 20 * (speed * 0.2));
                    if (y < 500)
                    {
                        for (int j = 0; j <= 10; j++)
                        {
                            int xShift = x - Round(j * 2 * 0.1);
                            int yShift = y + Round(j * 10 * 0.1);
                            _ = GamePack.Current.SpellControl[73].CastImages.Show(0, pixelX + xShift + 120, pixelY + yShift - 500 + 30, frame: 0, lockedSurface);
                        }
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:2181
    public void ChangeTerrain(MapField mapField, ref sbyte terrainType, in sbyte overlayType)
    {
        switch (terrainType)
        {
            case (sbyte)TerrainType.Wasteland:
                terrainType = 4;
                break;
            case (sbyte)TerrainType.Desert:
                terrainType = 4;
                break;
            case (sbyte)TerrainType.Steppe:
                terrainType = 1;
                break;
            default:
                break;
        }
    }
}
