// Translated from GlobalTargetSpells.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Ai.Groups;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.GlobalTarget;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// angle, animation, selected army identity
public partial class TornadoTE : GlobalTargetSpellTE
{
    // GlobalTargetSpells.pas:1559
    public new const int ClassID = 135972;

    public int FAngle;
    public Animation FAnimation;
    public ArmyID FArmyID;

    // GlobalTargetSpells.pas:1553
    public override void Dispose()
    {
        FArmyID?.Dispose();
        base.Dispose();
    }

    // GlobalTargetSpells.pas:1593
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwCreateEObject(ref FArmyID, null, 40);
    }

    // GlobalTargetSpells.pas:1547
    public TornadoTE(ECustomNode owner) : base(owner)
    {
        FArmyID = null;
    }

    // GlobalTargetSpells.pas:1660
    public override bool Process(AbstractUnit source, int value)
    {
        bool Result = default;
        UnitUserList MovedUnits = default;
        byte Owner = default;
        byte Selection = default;
        XYL Location = default;
        int Chance = default;
        bool Exposed = default;
        ArmyHS Army = default;
        AbstractUnit UnitObj = default;
        AoWMapField MapF = default;
        ArmyEventLog Log = default;
        ImageSequenceAnimation Anim = default;
        ArmyHS Placed = default;
        AIGroup Group = default;
        if (value == 0)
        {
            FAngle = 0;
            var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
            AoWHSMap.Current.ValidateObservation(source.FPlayer, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2);
            if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
            {
                AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
            }

            Anim = (ImageSequenceAnimation)new Animation(null);
            FAnimation = Anim;
            Anim.FAnimationLevel = 27;
            Anim.FOnShow = ShowAnimation;
            Anim.FAnimationMap = AoWHSMap.Current;
            Anim.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
            Anim.Activate();
        }

        if (value == 80)
        {
            MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
            if (FArmyID != null && !MapF.GetMagicShieldActive())
            {
                FArmyID.RetrieveArmy(ref Army, ref Selection);
                Exposed = !(Set<byte>.WithBounds(0, 255).With(1).Contains((byte)MapF.OverlayType) || MapF.FindNoneTransparentHS(typeof(Structure)) != null);
                for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
                {
                    if ((Selection & (1 << I)) != 0)
                    {
                        UnitObj = Army.FArmy.GetUnit(I);
                        switch (UnitObj.GetUnitLevel())
                        {
                            case 1:
                                Chance = 90;
                                break;
                            case 2:
                                Chance = 70;
                                break;
                            case 3:
                                Chance = 50;
                                break;
                            case 4:
                                Chance = 30;
                                break;
                            default:
                                Chance = 30;
                                break;
                        }

                        if (!Exposed)
                        {
                            Chance = Chance / 2;
                        }

                        if (Chance < AoWHSMap.Current.Random(100))
                        {
                            Selection = (byte)(Selection & ~(1 << I));
                        }
                    }
                }

                if (Army.FArmy.Transporter(255) != null)
                {
                    if (Army.FArmy.Transporter(Selection) != null)
                    {
                        Selection = 255;
                    }
                    else
                    {
                        Selection = 0;
                    }
                }

                if (Selection != 0)
                {
                    Log = new ArmyEventLog(null);
                    Log.FPlayer = source.FPlayer;
                    var tornado = AbilityRules.TranslateRStr(GameText.TornadoRStr);
                    Log.FArmyText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XCastRStr), [tornado]);
                    var tornadoText = AbilityRules.TranslateRStr(GameText.TornadoRStr);
                    Log.FArmyMessages.Add(tornadoText);
                    Log.FArmyIcon = 2;
                    Log.FArmyView.Setup(Army.FArmy, Selection);
                    var yhx = Army.Yhx;
                    var xhx = Army.Xhx;
                    EventLog.DistributeLocationEventLog(Log, (sbyte)FEventPlayerIndex, xhx, yhx, Army.Lhx, true);
                    Log.Release();
                    Location.Packed = ((Tornado)AoWHSSet.Current.FSpellControl.GetSpell(68)).CalculateWarpLocation(Army, Selection);
                    if (Location.Packed != 16777215)
                    {
                        if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
                        {
                            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(1);
                        }

                        if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.VisibleForPlayer(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, AoWHSMap.Current.FSeatedPlayer))
                        {
                            if (AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1] != null)
                            {
                                Anim = new ImageSequenceAnimation(null);
                                Anim.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1]);
                                AoWHSMap.Current.PlaceHX(Anim, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
                            }
                        }

                        if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.VisibleForPlayer(Location.X, Location.Y, Location.L, AoWHSMap.Current.FSeatedPlayer))
                        {
                            if (AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1] != null)
                            {
                                Anim = new ImageSequenceAnimation(null);
                                Anim.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1]);
                                AoWHSMap.Current.PlaceHX(Anim, Location.X, Location.Y, Location.L);
                            }
                        }

                        MovedUnits = new UnitUserList(null);
                        Owner = (byte)Army.FArmy.FPlayer;
                        MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(Location.Packed);
                        if (Army.FArmy.ValidateSelection(255) != Selection)
                        {
                            for (int I = Army.FArmy.Count - 1; I >= 0; I--)
                            {
                                if ((Selection & (1 << I)) != 0)
                                {
                                    UnitObj = Army.FArmy.GetUnit(I);
                                    MovedUnits.AddUnit(UnitObj);
                                    Placed = UnitObj.Place(Location.X, Location.Y, Location.L, (sbyte)(byte)Army.FArmy.FPlayer, Set<UnitPlaceModeElement>.Of());
                                    if (Placed != null)
                                    {
                                        Placed.FArmy.UpdateExploration();
                                    }
                                }
                            }
                        }
                        else
                        {
                            MovedUnits.AddUnits(Army.FArmy);
                            Army.MoveTo(MapF, 0);
                        }

                        if (Owner == 0)
                        {
                            var unitsToIndependentRelation = UnitList.UnitsToIndependentRelation(MovedUnits, 5);
                            Group = AoWHSMap.Current.FAIGroupManager.CreateGroup(0, unitsToIndependentRelation, GroupBehavior.Hunt);
                            Group.AddUnits(MovedUnits);
                            ((HuntAG)Group).FTargetUnitIDs.Add(source.FUnitID);
                            Group.Release();
                        }

                        MovedUnits?.Dispose();
                    }
                }
            }

            FAnimation?.Dispose();
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // GlobalTargetSpells.pas:1599
    public void ShowAnimation(Animation Animation, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        int XShift;
        int Height;
        int X;
        int Y;
        float Angle;
        int Direction;
        int Radius;
        int Last;
        if (AoWHSMap.Current.ViewL == Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer)
        {
            HexGeometry.HXtoHP(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
            Xhp -= XhpSurf;
            Yhp -= YhpSurf;
            FAngle = FAngle + 20 + FAnimation.FFrameCounter / 5;
            RandSeed = AoWHSMap.Current.FInitialRandomSeed;
            for (int I = 0; I <= 200; I++)
            {
                Direction = Random(360);
                if (Animation.FFrameCounter >= (int)(((uint)I) >> 2) && Animation.FFrameCounter <= (int)(((uint)I) >> 4) + 68)
                {
                    Radius = Round((double)((double)2.997 - Math.Log((double)((double)(I * 19) * 0.005) + 1)) * 35) + 7;
                    Height = 200 - I - 16;
                    XShift = Round((double)((double)Math.Sin((double)((double)FAnimation.FFrameCounter * 0.1) + (double)I * 0.02) * (200 - I)) * 0.25) + 16;
                    var pi = Math.PI;
                    Angle = (float)((double)((double)(Direction + FAngle) * pi) / 180);
                    if (GameTables.AoWEngine.FAnimationDetail == 0)
                    {
                        Last = 1;
                    }
                    else
                    {
                        if (GameTables.AoWEngine.FAnimationDetail == 1)
                        {
                            Last = (int)(((uint)(200 - I)) >> 5);
                        }
                        else
                        {
                            Last = (int)(((uint)(200 - I)) >> 4);
                        }
                    }

                    for (int Count = Last; Count >= 0; Count--)
                    {
                        X = Round((double)Math.Cos(Angle) * Radius) + XShift;
                        Y = Round((double)((double)Math.Sin(Angle) * Radius) * 0.5) + Height;
                        AoWHSSet.Current.FSpellControl.GetSpell(68).FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                        Angle = (float)((double)0.035 + Angle);
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:215
    public override bool Validate(Hero source) => base.Validate(source) && (FArmyID == null || FArmyID.Valid());
}
