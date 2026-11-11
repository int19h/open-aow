// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// FMovePoints. FUnitHS. FPositionIndex. FPositionGroup. FCanFlee. FDeathMethod
public partial class TacticalBattleUnit : CombatUnit
{
    public TacticalBattleUnit(ECustomNode owner) : base(owner)
    { }

    public int FMovePoints;
    public TacticalBattleUnitHexSprite FUnitHS;
    public byte FPositionIndex;
    public byte FPositionGroup;
    public bool FCanFlee;
    public DeathMethod FDeathMethod;

    // AoWTC.pas:2626
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
    }

    // AoWTC.pas:2612
    public override void NewTurn(byte CombatPlayer)
    {
        base.NewTurn(CombatPlayer);
        if (CombatPlayer == (byte)FCombatPlayer && GetEnabled())
        {
            FMovePoints = GetMoves();
        }
    }

    // AoWTC.pas:2629
    public override int ExecuteDamage(int Damage)
    {
        TacticalBattleCorpseHexSprite Corpse;
        ImageSequence Sequence;
        BattleEffectSprite Effect;
        float Speed = default;
        float VZ = default;
        float Gravity = default;
        float Z = default;
        int X = default;
        int Y = default;
        Rect R;
        void MakeBlood()
        {
            int Count;
            int X = default;
            int Y = default;
            int Direction;
            TacticalBattleBloodHexSprite Blood;
            if (GetEnabled() && FUnitHS != null && FUnit.GetBloodType() != BloodType.None)
            {
                var hitPoints = GetHitPoints();
                if (Damage >= hitPoints && (GetAbilityEnabled(131) || FDeathMethod == DeathMethod.Explode))
                {
                    Count = Random(5) + 10;
                }
                else
                {
                    Count = Random(Damage + 7) - 4;
                }

                while (Count > 0)
                {
                    Direction = Random(12);
                    if (Direction > 6)
                    {
                        Direction = 0;
                    }

                    var yhx = FUnitHS.GetYhx();
                    HexGeometry.CenterHNtoHX(FUnitHS.GetXhx(), yhx, Direction, ref X, ref Y);
                    if (CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 0 && CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 9)
                    {
                        Blood = new TacticalBattleBloodHexSprite(CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y]);
                        Blood.Activate();
                        Blood.FOffsetX = Random(32) - 32;
                        Blood.FOffsetY = Random(32) - 32;
                        Blood.FFrame = Random(AoWHSSet.Current.FImageSequences.ImageSequences[4].GetTotalSequenceFrames());
                    }

                    Count -= 1;
                }
            }
        }

        MakeBlood();
        int Result = base.ExecuteDamage(Damage);
        if (!GetEnabled())
        {
            if (FUnitHS != null)
            {
                if (GetAbilityEnabled(131))
                {
                    MakeHitBlood(this, 100);
                    for (int I = 0; I <= 9; I++)
                    {
                        Effect = CombatMap.Current.AddEffect(BattleEffectType.DecayComet);
                        Effect.FAbilityIndex = 52;
                        Effect.FSpellIndex = 106;
                        var yhx = FUnitHS.GetYhx();
                        HexGeometry.HXtoHP(FUnitHS.GetXhx(), yhx, ref X, ref Y);
                        var xyz = new Point3D();
                        Effect.FPath.Add(xyz);
                        Effect.FPath[0].X = X + 16;
                        Effect.FPath[0].Y = Y + 8;
                        for (int J = 0; J <= 10; J++)
                        {
                            Speed = (float)((double)(Random(50) + 50) * 0.05);
                            Gravity = (float)1.5;
                            VZ = (float)((double)(Random(25) + 25) * 0.05);
                            Z = 0;
                            var count = Effect.FPath.Count;
                            var path = Effect.FPath[count - 1];
                            var fPathCount = Effect.FPath.Count;
                            var pathZ = Effect.FPath[fPathCount - 1];
                            var count2 = Effect.FPath.Count;
                            var pathX = Effect.FPath[count2 - 1];
                            var random = Random(360);
                            EffectPaths.MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                        }
                    }
                }
                else
                {
                    var yhx = FUnitHS.GetYhx();
                    var xhx = FUnitHS.GetXhx();
                    bool condition = CombatMap.Current.MapContainer.MapLevels[0].Field[xhx, yhx].TerrainType != 0;
                    if (condition)
                    {
                        var fUnitHSYhx = FUnitHS.GetYhx();
                        var fUnitHSXhx = FUnitHS.GetXhx();
                        condition = CombatMap.Current.MapContainer.MapLevels[0].Field[fUnitHSXhx, fUnitHSYhx].TerrainType != 9;
                    }

                    if (condition)
                    {
                        var fUnitHSYhx = FUnitHS.GetYhx();
                        var fUnitHSXhx = FUnitHS.GetXhx();
                        Corpse = new TacticalBattleCorpseHexSprite(CombatMap.Current.MapContainer.MapLevels[0].Field[fUnitHSXhx, fUnitHSYhx]);
                        Corpse.FUnitGFX = FUnitHS.FUnitGFXResource;
                        Sequence = Corpse.FUnitGFX.FSequences.ImageSequences[30];
                        if (Sequence != null)
                        {
                            Corpse.FFrame = Random(Sequence.GetTotalSequenceFrames());
                        }

                        switch (FDeathMethod)
                        {
                            case DeathMethod.Normal:
                                switch (FUnit.GetBloodType())
                                {
                                    case BloodType.None:
                                        Corpse.FSequenceIndex = -1;
                                        break;
                                    default:
                                        {
                                            Corpse.FSequenceIndex = ((int)FUnit.GetUnitSize() & 127) + 30;
                                            if (Corpse.FSequenceIndex > 32)
                                            {
                                                Corpse.FSequenceIndex = 32;
                                            }
                                        }

                                        break;
                                }

                                break;
                            case DeathMethod.Explode:
                                Corpse.FSequenceIndex = -1;
                                break;
                        }

                        Corpse.FOffsetX = Random(10) - 5;
                        Corpse.FOffsetY = Random(10) - 5;
                        if (FUnit.GetUnitType() != UnitType.Machine)
                        {
                            Corpse.FUnit = this;
                        }

                        Corpse.Activate();
                    }
                }

                var yhx2 = FUnitHS.GetYhx();
                var xhx2 = FUnitHS.GetXhx();
                var yhx3 = FUnitHS.GetYhx();
                R = new Rect(FUnitHS.GetXhx(), yhx3, xhx2 + 1, yhx2 + 1);
                FUnitHS?.Dispose();
                FUnitHS = null;
                CombatMap.Current.TriggerMapChangedEvent(R, 0);
            }
        }
        else
        {
            if (FUnitHS != null)
            {
                var unitGFXResourceIndex = FUnit.GetUnitGFXResourceIndex();
                FUnitHS.SetUnitGFXResourceIndex(unitGFXResourceIndex);
            }
        }

        return Result;
    }

    // AoWTC.pas:2605
    public int GetPosValue()
    {
        int Result = GetAttack() + GetDamage() + GetDefense() + GetMoves() / 3;
        if (SupportCombatMode(Set<CombatMode>.Of(CombatMode.MediumRange)))
        {
            Result = Result - 100;
        }

        if (SupportCombatMode(Set<CombatMode>.Of(CombatMode.LongRange)))
        {
            Result = Result - 200;
        }

        return Result;
    }

    // AoWTC.pas:2618
    public void MovedTo(AoWMapField OldField, AoWMapField NewField, int MoveCost)
    {
        FMovePoints = FMovePoints - MoveCost;
        if (FMovePoints < 0)
        {
            FMovePoints = 0;
        }

        if (GetAbilityEnabled(43))
        {
            var yhx = FUnitHS.GetYhx();
            EffectPaths.CreateFire(false, 50, FUnitHS.GetXhx(), yhx);
        }

        CombatMap.Current.FUnitChangedEventsStore.Raise(FUnitHS);
    }

    internal static void MakeHitBlood(TacticalBattleUnit UnitObject, int Damage)
    {
        BattleEffectSprite Effect;
        Point3D Point;
        Effect = CombatMap.Current.AddEffect(BattleEffectType.Blood);
        Point = new Point3D();
        Effect.FPath.Add(Point);
        switch (Damage)
        {
            case >= 1 and <= 3:
                Point.Z = 1;
                break;
            case >= 4 and <= 7:
                Point.Z = 2;
                break;
            case >= 8 and <= 100:
                Point.Z = 3;
                break;
        }

        switch (UnitObject.FUnit.GetBloodType())
        {
            case BloodType.None:
                Point.Z = Point.Z + 5;
                break;
        }

        var yhx = UnitObject.FUnitHS.Yhx;
        HexGeometry.HXtoHP(UnitObject.FUnitHS.Xhx, yhx, ref Point.X, ref Point.Y);
        Point.X = Random(10) + Point.X + 11;
        var unitSize = UnitObject.FUnit.GetUnitSize();
        Point.Y = Random(10) + Point.Y + 9 - (int)unitSize * 10;
    }

    internal static CombatMoveTokenEvent CreateCombatMoveTE(TacticalBattleUnit UnitObject, MovePath Path)
    {
        CombatMoveTokenEvent Result = new CombatMoveTokenEvent(null);
        if (Result != null)
        {
            Result.Setup(UnitObject, Path);
        }

        return Result;
    }

    internal static bool MoveCombatUnit(TacticalBattleUnit UnitObject, MovePath Path)
    {
        int MovePoints;
        int Cost;
        int Index;
        CombatMoveTokenEvent Event;
        MovePath NewPath;
        bool Result = false;
        var player = UnitObject.GetPlayer();
        if (AoWHSMap.Current.FTokenManager.PlayerReady(player) && Path.Count > 1)
        {
            MovePoints = UnitObject.FMovePoints;
            Cost = 0;
            Index = Path.Count;
            while (Index != 0 && Cost <= MovePoints)
            {
                Index = Index - 1;
                Cost = Cost + Path[Index].Flag;
            }

            if (Cost > MovePoints)
            {
                Index = Index + 1;
            }

            if (Index < Path.Count - 1)
            {
                NewPath = new MovePath();
                for (int I = Index, last = Path.Count - 1; I <= last; I++)
                {
                    NewPath.Add(Path[I]);
                }

                Event = CreateCombatMoveTE(UnitObject, NewPath);
                if (Event != null)
                {
                    var unitObjectPlayer = UnitObject.GetPlayer();
                    AoWHSMap.Current.FTokenManager.GetTokenControl(unitObjectPlayer).AddEvent(Event);
                    Event.Release();
                    Result = true;
                }

                NewPath?.Dispose();
            }
        }

        return Result;
    }

    public enum DeathMethod : byte
    {
        Normal = 0, Explode = 1
    }

    // intf_P3_01.inc:244

    // intf_P3_01.inc:244

}
