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
public partial class HealingShowersTE : GlobalTargetSpellTE
{
    public HealingShowersTE(ECustomNode owner) : base(owner)
    { }

    // GlobalTargetSpells.pas:2138
    public new const int ClassID = 135975;

    public Animation FAnimation;

    // GlobalTargetSpells.pas:2190
    public override bool Process(AbstractUnit source, int value)
    {
        bool Result = default;
        AbstractUnit UnitObj = default;
        HNWalkData Walk = default;
        ArmyHS Army = default;
        MapLevel Level = default;
        ImageSequenceAnimation Anim = default;
        bool Healed = default;
        bool PlaySound = default;
        HexagonSprite Cloud = default;
        base.Process(source, value);
        if (value == 0)
        {
            AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 2);
            if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.WatchingTerrain(FLocation.X, FLocation.Y, FLocation.L, 2))
            {
                AoWHSSet.Current.FSpellControl.GetSpell(73).FCastSounds.Play(0);
            }

            Anim = (ImageSequenceAnimation)new Animation(null);
            FAnimation = Anim;
            Anim.FAnimationLevel = 27;
            Anim.FOnShow = ShowAnimation;
            Anim.FAnimationMap = AoWHSMap.Current;
            Anim.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
            Anim.Activate();
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
        }

        if (value == 50)
        {
            PlaySound = false;
            AoWHSMap.Current.ChangeTerrainEx(null, FLocation.X, FLocation.Y, FLocation.L, 1, ChangeTerrain, null);
            Level = AoWHSMap.Current.MapContainer.MapLevels[FLocation.L];
            HNWalkData.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Level.Height > Walk.Y)
                {
                    if (!((AoWMapField)Level.GetField(Walk.X, Walk.Y)).GetMagicShieldActive())
                    {
                        Healed = false;
                        Army = ((AoWMapField)Level.GetField(Walk.X, Walk.Y)).GetArmyHS();
                        if (Army != null)
                        {
                            for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
                            {
                                UnitObj = Army.FArmy.GetUnit(I);
                                if (UnitObj.GetHitPoints() < UnitObj.GetHits())
                                {
                                    UnitObj.SetHitPoints((sbyte)(UnitObj.GetHitPoints() + 5));
                                    Healed = true;
                                }

                                if (UnitObj.GetAbilityEnabled(96))
                                {
                                    UnitObj.RemoveAbility(96);
                                    Healed = true;
                                }
                            }

                            Army.FArmy.Update();
                            bool healed = Healed;
                            if (healed)
                            {
                                var yhx = Army.Yhx;
                                var xhx = Army.Xhx;
                                healed = AoWHSMap.Current.VisibleForPlayer(xhx, yhx, Army.Lhx, AoWHSMap.Current.FSeatedPlayer);
                            }

                            if (healed)
                            {
                                bool condition = PlaySound || (byte)Army.FArmy.FPlayer == (byte)AoWHSMap.Current.FSeatedPlayer;
                                if (!condition)
                                {
                                    var yhx = Army.Yhx;
                                    var xhx = Army.Xhx;
                                    condition = AoWHSMap.Current.Watching(xhx, yhx, Army.Lhx, 1);
                                }

                                PlaySound = condition;
                                Anim = new ImageSequenceAnimation(null);
                                Anim.SetImageSequence(AoWHSSet.Current.FAbilityControl.GetAbility(47).FImageSequences.ImageSequences[0]);
                                Anim.FAnimationLevel = 27;
                                var armyYhx = Army.Yhx;
                                var armyXhx = Army.Xhx;
                                AoWHSMap.Current.PlaceHX(Anim, armyXhx, armyYhx, Army.Lhx);
                            }
                        }

                        Cloud = (HexagonSprite)Level.GetField(Walk.X, Walk.Y).FindOwnedChild(132990);
                        if (Cloud != null)
                        {
                            Cloud?.Dispose();
                        }
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }

            if (PlaySound)
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(47).FSFXLibrary.Play(0);
            }
        }

        if (value == 80)
        {
            FAnimation?.Dispose();
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // GlobalTargetSpells.pas:2141
    public void ShowAnimation(Animation Animation, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int X;
        int Y;
        int XShift;
        int YShift;
        int Speed;
        int Xhp = default;
        int Yhp = default;
        int Count;
        int Frame;
        if (AoWHSMap.Current.ViewL == FLocation.L)
        {
            HexGeometry.HXtoHP(FLocation.X, FLocation.Y, ref Xhp, ref Yhp);
            Xhp -= XhpSurf;
            Yhp -= YhpSurf;
            switch (GameTables.AoWEngine.FRegistry.GetDetailLevel())
            {
                case DetailLevel.Low:
                    Count = 50;
                    break;
                case DetailLevel.High:
                    Count = 160;
                    break;
                default:
                    Count = 100;
                    break;
            }

            RandSeed = AoWHSMap.Current.FInitialRandomSeed;
            for (int I = 0; I <= Count; I++)
            {
                X = Random(100) - 50;
                Y = Random(100) - 50;
                Speed = Random(5) + 5;
                var random = Random(30);
                Frame = Animation.FFrameCounter - random;
                if (Frame >= 0 && Frame < 50)
                {
                    Frame = Frame % 30;
                    X -= Round((double)(Frame * 4) * ((double)Speed * 0.2));
                    Y += Round((double)(Frame * 20) * ((double)Speed * 0.2));
                    if (Y < 500)
                    {
                        for (int J = 0; J <= 10; J++)
                        {
                            XShift = X - Round((double)(J * 2) * 0.1);
                            YShift = Y + Round((double)(J * 10) * 0.1);
                            AoWHSSet.Current.FSpellControl.GetSpell(73).FCastImages.Show(0, Xhp + XShift + 120, Yhp + YShift - 500 + 30, 0, SurfaceDesc);
                        }
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:2181
    public void ChangeTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
    {
        switch (TerrainType)
        {
            case 5:
                TerrainType = 4;
                break;
            case 2:
                TerrainType = 4;
                break;
            case 4:
                TerrainType = 1;
                break;
        }
    }

    // NetworkE.pas:230

}
