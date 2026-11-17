#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// animation
public partial class HealingShowersTokenEvent(CustomNode owner) : GlobalTargetSpellTokenEvent(owner)
{

    // GlobalTargetSpells.pas:2138
    public new const int ClassId = 135975;

    private Animation _animation;

    // GlobalTargetSpells.pas:2190
    public override bool Process(Troop source, int value)
    {
        bool result = default;
        Troop troopObject = default;
        HexNumberWalkData walk = default;
        ArmyHexSprite army = default;
        MapLevel level = default;
        ImageSequenceAnimation animation = default;
        bool healed2 = default;
        bool playSound = default;
        HexSprite cloud = default;
        _ = base.Process(source, value);
        if (value == 0)
        {
            _ = StrategicMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 2);
            if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.WatchingTerrain(Location.X, Location.Y, Location.MapLevel, 2))
            {
                _ = GamePack.Current.SpellControl.GetSpell(73).CastSounds.Play(0);
            }

            // The original constructs a plain animation and stores it on the event. The later shower uses an image sequence.
            _animation = new Animation(null)
            {
                Holder = CreationHolder(),
                AnimationLevel = 27
            };
            _animation.SetShowHandler(ShowAnimation);
            _animation.AnimationMap = StrategicMap.Current;
            _animation.Tag = (StrategicMap.Current.ViewWidth & 65535) | (StrategicMap.Current.ViewHeight << 16);
            _animation.Activate();
            GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        }

        if (value == 50)
        {
            playSound = false;
            _ = StrategicMap.Current.ChangeTerrain(null, Location.X, Location.Y, Location.MapLevel, 1, ChangeTerrain, null);
            level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
            HexNumberWalkData.InitializeHexNumberWalk(out walk, Location.X, Location.Y, 0);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && level.Height > walk.Y)
                {
                    if (!level.GetField(walk.X, walk.Y).GetMagicShieldActive())
                    {
                        healed2 = false;
                        army = level.GetField(walk.X, walk.Y).GetArmyHexSprite();
                        if (army != null)
                        {
                            for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
                            {
                                troopObject = army.Army.Units.GetUnit(i);
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
                                healed = StrategicMap.Current.VisibleForPlayer(hexX, hexY, army.HexLevel, StrategicMap.Current.SeatedPlayer);
                            }

                            if (healed)
                            {
                                bool condition = playSound || (byte)army.Army.Player == (byte)StrategicMap.Current.SeatedPlayer;
                                if (!condition)
                                {
                                    sbyte hexY = army.HexY;
                                    sbyte hexX = army.HexX;
                                    condition = StrategicMap.Current.Watching(hexX, hexY, army.HexLevel, 1);
                                }

                                playSound = condition;
                                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                                animation.SetImageSequence(GamePack.Current.AbilityControl.GetAbility(47).ImageSequences.ImageSequences[0]);
                                animation.AnimationLevel = 27;
                                sbyte armyHexY = army.HexY;
                                sbyte armyHexX = army.HexX;
                                _ = StrategicMap.Current.PlaceHex(animation, armyHexX, armyHexY, army.HexLevel);
                            }
                        }

                        cloud = level.GetField(walk.X, walk.Y).FindOwnedChild<HexSprite>(132990);
                        if (cloud != null)
                        {
                            cloud?.Dispose();
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            if (playSound)
            {
                _ = GamePack.Current.AbilityControl.GetAbility(47).SfxLibrary.Play(0);
            }
        }

        if (value == 80)
        {
            _animation?.Dispose();
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // GlobalTargetSpells.pas:2141
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int x;
        int y;
        int xShift;
        int yShift;
        int speed;
        int count;
        int frame;
        if (StrategicMap.Current.ViewMapLevel == Location.MapLevel)
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out int pixelX, out int pixelY);
            pixelX -= pixelXSurface;
            pixelY -= pixelYSurface;
            switch (GetEngine().Registry.GetDetailLevel())
            {
                case DetailLevel.Low:
                    count = 50;
                    break;
                case DetailLevel.High:
                    count = 160;
                    break;
                default:
                    count = 100;
                    break;
            }

            RandSeed = StrategicMap.Current.InitialRandomSeed;
            for (int i = 0; i <= count; i++)
            {
                x = Random(100) - 50;
                y = Random(100) - 50;
                speed = Random(5) + 5;
                int random = Random(30);
                frame = animation.FrameCounter - random;
                if (frame is >= 0 and < 50)
                {
                    frame %= 30;
                    x -= Round(frame * 4 * (speed * 0.2));
                    y += Round(frame * 20 * (speed * 0.2));
                    if (y < 500)
                    {
                        for (int j = 0; j <= 10; j++)
                        {
                            xShift = x - Round(j * 2 * 0.1);
                            yShift = y + Round(j * 10 * 0.1);
                            _ = GamePack.Current.SpellControl.GetSpell(73).CastImages.Show(0, pixelX + xShift + 120, pixelY + yShift - 500 + 30, 0, lockedSurface);
                        }
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:2181
    public void ChangeTerrain(MapField mapField, ref sbyte terrainType, ref sbyte overlayType)
    {
        switch (terrainType)
        {
            case 5:
                terrainType = 4;
                break;
            case 2:
                terrainType = 4;
                break;
            case 4:
                terrainType = 1;
                break;
        }
    }

    // NetworkE.pas:230

}
