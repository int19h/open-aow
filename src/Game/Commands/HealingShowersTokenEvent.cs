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
public partial class HealingShowersTokenEvent : GlobalTargetSpellTokenEvent
{
    public HealingShowersTokenEvent(ECustomNode owner) : base(owner)
    { }

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
        bool Healed = default;
        bool playSound = default;
        HexSprite cloud = default;
        base.Process(source, value);
        if (value == 0)
        {
            StrategicMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 2);
            if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.WatchingTerrain(Location.X, Location.Y, Location.MapLevel, 2))
            {
                GamePack.Current.SpellControl.GetSpell(73).CastSounds.Play(0);
            }

            animation = (ImageSequenceAnimation)new Animation(null);
            _animation = animation;
            animation.AnimationLevel = 27;
            animation.SetShowHandler(ShowAnimation);
            animation.AnimationMap = StrategicMap.Current;
            animation.Tag = (StrategicMap.Current.ViewWidth & 65535) | (StrategicMap.Current.ViewHeight << 16);
            animation.Activate();
            GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        }

        if (value == 50)
        {
            playSound = false;
            StrategicMap.Current.ChangeTerrain(null, Location.X, Location.Y, Location.MapLevel, 1, ChangeTerrain, null);
            level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
            HexNumberWalkData.InitializeHexNumberWalk(out walk, Location.X, Location.Y, 0);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && level.Height > walk.Y)
                {
                    if (!level.GetField(walk.X, walk.Y).GetMagicShieldActive())
                    {
                        Healed = false;
                        army = level.GetField(walk.X, walk.Y).GetArmyHexSprite();
                        if (army != null)
                        {
                            for (int I = 0, last = army.Army.Count - 1; I <= last; I++)
                            {
                                troopObject = army.Army.GetUnit(I);
                                if (troopObject.GetHitPoints() < troopObject.GetHits())
                                {
                                    troopObject.SetHitPoints((sbyte)(troopObject.GetHitPoints() + 5));
                                    Healed = true;
                                }

                                if (troopObject.GetAbilityEnabled(96))
                                {
                                    troopObject.RemoveAbility(96);
                                    Healed = true;
                                }
                            }

                            army.Army.Update();
                            bool healed = Healed;
                            if (healed)
                            {
                                var hexY = army.HexY;
                                var hexX = army.HexX;
                                healed = StrategicMap.Current.VisibleForPlayer(hexX, hexY, army.HexLevel, StrategicMap.Current.SeatedPlayer);
                            }

                            if (healed)
                            {
                                bool condition = playSound || (byte)army.Army.Player == (byte)StrategicMap.Current.SeatedPlayer;
                                if (!condition)
                                {
                                    var hexY = army.HexY;
                                    var hexX = army.HexX;
                                    condition = StrategicMap.Current.Watching(hexX, hexY, army.HexLevel, 1);
                                }

                                playSound = condition;
                                animation = new ImageSequenceAnimation(null);
                                animation.SetImageSequence(GamePack.Current.AbilityControl.GetAbility(47).ImageSequences.ImageSequences[0]);
                                animation.AnimationLevel = 27;
                                var armyHexY = army.HexY;
                                var armyHexX = army.HexX;
                                StrategicMap.Current.PlaceHex(animation, armyHexX, armyHexY, army.HexLevel);
                            }
                        }

                        cloud = (HexSprite)level.GetField(walk.X, walk.Y).FindOwnedChild(132990);
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
                GamePack.Current.AbilityControl.GetAbility(47).SfxLibrary.Play(0);
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
        int X;
        int Y;
        int xShift;
        int yShift;
        int speed;
        int pixelX = default;
        int pixelY = default;
        int count;
        int frame;
        if (StrategicMap.Current.ViewMapLevel == Location.MapLevel)
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out pixelX, out pixelY);
            pixelX -= pixelXSurface;
            pixelY -= pixelYSurface;
            switch (GameTables.GameEngine.Registry.GetDetailLevel())
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
            for (int I = 0; I <= count; I++)
            {
                X = Random(100) - 50;
                Y = Random(100) - 50;
                speed = Random(5) + 5;
                var random = Random(30);
                frame = animation.FrameCounter - random;
                if (frame >= 0 && frame < 50)
                {
                    frame = frame % 30;
                    X -= Round((double)(frame * 4) * ((double)speed * 0.2));
                    Y += Round((double)(frame * 20) * ((double)speed * 0.2));
                    if (Y < 500)
                    {
                        for (int J = 0; J <= 10; J++)
                        {
                            xShift = X - Round((double)(J * 2) * 0.1);
                            yShift = Y + Round((double)(J * 10) * 0.1);
                            GamePack.Current.SpellControl.GetSpell(73).CastImages.Show(0, pixelX + xShift + 120, pixelY + yShift - 500 + 30, 0, lockedSurface);
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
