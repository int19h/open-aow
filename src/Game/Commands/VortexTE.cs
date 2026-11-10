// Translated from GlobalTargetSpells.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells.GlobalTarget;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// angle, animation, selected army identity
public partial class VortexTE : GlobalTargetSpellTE
{
    // GlobalTargetSpells.pas:1905
    public new const int ClassID = 135973;

    public int FAngle;
    public Animation FAnimation;
    public ArmyID FArmyID;

    // GlobalTargetSpells.pas:1899
    public override void Dispose()
    {
        FArmyID?.Dispose();
        base.Dispose();
    }

    // GlobalTargetSpells.pas:1939
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwCreateEObject(ref FArmyID, null, 40);
    }

    // GlobalTargetSpells.pas:1893
    public VortexTE(ECustomNode owner) : base(owner)
    {
        FArmyID = null;
    }

    // GlobalTargetSpells.pas:1991
    public override bool Process(AbstractUnit source, int value)
    {
        bool Result = default;
        Animation Anim = default;
        ArmyEventLog Log = default;
        byte Selection = default;
        bool Damaged = default;
        int Damage = default;
        ArmyHS Army = default;
        AbstractUnit UnitObj = default;
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

            Anim = new Animation(null);
            FAnimation = Anim;
            Anim.FAnimationLevel = 27;
            Anim.FOnShow = ShowAnimation;
            Anim.FAnimationMap = AoWHSMap.Current;
            Anim.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
            Anim.Activate();
        }

        if (value == 80)
        {
            if (FArmyID != null && FArmyID.RetrieveArmy(ref Army, ref Selection))
            {
                Log = new ArmyEventLog(null);
                Log.FArmyText = AbilityRules.TranslateRStr(VortexDamageRStr);
                Log.FPlayer = source.FPlayer;
                var vortex = AbilityRules.TranslateRStr(GameText.VortexRStr);
                Log.FArmyMessages.Add(vortex);
                Log.FArmyIcon = 2;
                Log.FArmyView.Setup(Army.FArmy, 255);
                Army.FArmy.BeginUpdate();
                Damaged = false;
                for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
                {
                    UnitObj = Army.FArmy.GetUnit(I);
                    if (!UnitObj.GetMoveTypes().Contains(MoveType.Flying))
                    {
                        if (UnitObj.GetAbilityEnabled(2))
                        {
                            Damage = 7;
                        }
                        else
                        {
                            if (UnitObj.GetAbilityEnabled(23))
                            {
                                Damage = 10;
                            }
                            else
                            {
                                Damage = 0;
                            }
                        }

                        Damage = UnitObj.ExecuteDamageRole(8, Damage, Set<DamageType>.Of(DamageType.Physical));
                        if (Damage > UnitObj.GetHitPoints())
                        {
                            Damage = UnitObj.GetHitPoints();
                        }

                        if (Damage > 0)
                        {
                            UnitObj.SetHitPoints((sbyte)(UnitObj.GetHitPoints() - Damage));
                            Damaged = true;
                        }
                    }
                    else
                    {
                        Damage = 0;
                    }

                    Log.FDamageValues.Add((byte)Damage);
                    Log.FDamageTypes.Add(7);
                }

                if (Damaged)
                {
                    Log.ValidateGameOver();
                    var yhx = Army.Yhx;
                    var xhx = Army.Xhx;
                    EventLog.DistributeLocationEventLog(Log, Army.FArmy.FPlayer, xhx, yhx, Army.Lhx, true);
                }

                Log.Release();
                Army.FArmy.Validate();
                Army.FArmy.ValidateTerrainEx(((MapField)Army.Owner).TerrainType, ((MapField)Army.Owner).OverlayType, source.FPlayer);
                Army.FArmy.EndUpdate();
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

    // GlobalTargetSpells.pas:1945
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
        int Count;
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
                if (Animation.FFrameCounter >= (int)(((uint)(200 - I)) >> 2) && Animation.FFrameCounter <= (int)(((uint)I) >> 4) + 68)
                {
                    Radius = Round((double)((double)2.997 - Math.Log((double)((double)(I * 19) * 0.005) + 1)) * 35) + 18;
                    Height = -16;
                    XShift = 16;
                    var pi = Math.PI;
                    Angle = (float)((double)((double)-(Direction + FAngle) * pi) / 180);
                    if (GameTables.AoWEngine.FAnimationDetail == 0)
                    {
                        Count = 1;
                    }
                    else
                    {
                        if (GameTables.AoWEngine.FAnimationDetail == 1)
                        {
                            Count = (int)(((uint)(200 - I)) >> 5);
                        }
                        else
                        {
                            Count = (int)(((uint)(200 - I)) >> 4);
                        }
                    }

                    for (int J = Count; J >= 0; J--)
                    {
                        X = Round((double)Math.Cos(Angle) * Radius) + XShift;
                        Y = Round((double)((double)Math.Sin(Angle) * Radius) / 1.75) + Height;
                        AoWHSSet.Current.FSpellControl.GetSpell(70).FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                        Angle = (float)((double)0.035 + Angle);
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:242
    public override bool Validate(Hero source) => base.Validate(source) && (FArmyID == null || FArmyID.Valid());

    private static readonly string VortexDamageRStr = "Vortex Damage";
}
