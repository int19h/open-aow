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

// MovePoints. UnitHexSprite. PositionIndex. PositionGroup. CanFlee. DeathMethod
public partial class TacticalBattleUnit : CombatUnit
{
    public TacticalBattleUnit(ECustomNode owner) : base(owner)
    { }

    public int MovePoints;
    public TacticalBattleUnitHexSprite UnitHexSprite;
    public byte PositionIndex;
    public byte PositionGroup;
    public bool CanFlee;
    public TacticalBattleUnitDeathMethod DeathMethod;

    // AoWTC.pas:2626
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
    }

    // AoWTC.pas:2612
    public override void NewTurn(byte combatPlayer)
    {
        base.NewTurn(combatPlayer);
        if (combatPlayer == (byte)CombatPlayer && GetEnabled())
        {
            MovePoints = GetMoves();
        }
    }

    // AoWTC.pas:2629
    public override int ExecuteDamage(int damage)
    {
        TacticalBattleCorpseHexSprite corpse;
        ImageSequence sequence;
        BattleEffectSprite effect;
        float speed = default;
        float vz = default;
        float gravity = default;
        float Z = default;
        int X = default;
        int Y = default;
        Rect R;
        void MakeBlood()
        {
            int count;
            int X = default;
            int Y = default;
            int direction;
            TacticalBattleBloodHexSprite blood;
            if (GetEnabled() && UnitHexSprite != null && Troop.GetBloodType() != BloodType.None)
            {
                var hitPoints = GetHitPoints();
                if (damage >= hitPoints && (GetAbilityEnabled(131) || DeathMethod == TacticalBattleUnitDeathMethod.Explode))
                {
                    count = Random(5) + 10;
                }
                else
                {
                    count = Random(damage + 7) - 4;
                }

                while (count > 0)
                {
                    direction = Random(12);
                    if (direction > 6)
                    {
                        direction = 0;
                    }

                    var hexY = UnitHexSprite.GetHexY();
                    HexGeometry.CenterHexNumberToHex(UnitHexSprite.GetHexX(), hexY, direction, ref X, ref Y);
                    if (CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 0 && CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 9)
                    {
                        blood = new TacticalBattleBloodHexSprite(CombatMap.Current.MapContainer.MapLevels[0].Field[X, Y]);
                        blood.Activate();
                        blood.OffsetX = Random(32) - 32;
                        blood.OffsetY = Random(32) - 32;
                        blood.Frame = Random(AoWHSSet.Current.ImageSequences.ImageSequences[4].GetTotalSequenceFrames());
                    }

                    count -= 1;
                }
            }
        }

        MakeBlood();
        int result = base.ExecuteDamage(damage);
        if (!GetEnabled())
        {
            if (UnitHexSprite != null)
            {
                if (GetAbilityEnabled(131))
                {
                    MakeHitBlood(this, 100);
                    for (int I = 0; I <= 9; I++)
                    {
                        effect = CombatMap.Current.AddEffect(BattleEffectType.DecayComet);
                        effect.AbilityIndex = 52;
                        effect.SpellIndex = 106;
                        var hexY = UnitHexSprite.GetHexY();
                        HexGeometry.HexToPixel(UnitHexSprite.GetHexX(), hexY, ref X, ref Y);
                        var xyz = new Point3D();
                        effect.Path.Add(xyz);
                        effect.Path[0].X = X + 16;
                        effect.Path[0].Y = Y + 8;
                        for (int J = 0; J <= 10; J++)
                        {
                            speed = (float)((double)(Random(50) + 50) * 0.05);
                            gravity = (float)1.5;
                            vz = (float)((double)(Random(25) + 25) * 0.05);
                            Z = 0;
                            var count = effect.Path.Count;
                            var path = effect.Path[count - 1];
                            var fPathCount = effect.Path.Count;
                            var pathZ = effect.Path[fPathCount - 1];
                            var count2 = effect.Path.Count;
                            var pathX = effect.Path[count2 - 1];
                            var random = Random(360);
                            EffectPaths.MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref speed, ref vz, ref gravity, ref effect.Path);
                        }
                    }
                }
                else
                {
                    var hexY = UnitHexSprite.GetHexY();
                    var hexX = UnitHexSprite.GetHexX();
                    bool condition = CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].TerrainType != 0;
                    if (condition)
                    {
                        var fUnitHexSpriteHexY = UnitHexSprite.GetHexY();
                        var fUnitHexSpriteHexX = UnitHexSprite.GetHexX();
                        condition = CombatMap.Current.MapContainer.MapLevels[0].Field[fUnitHexSpriteHexX, fUnitHexSpriteHexY].TerrainType != 9;
                    }

                    if (condition)
                    {
                        var fUnitHexSpriteHexY = UnitHexSprite.GetHexY();
                        var fUnitHexSpriteHexX = UnitHexSprite.GetHexX();
                        corpse = new TacticalBattleCorpseHexSprite(CombatMap.Current.MapContainer.MapLevels[0].Field[fUnitHexSpriteHexX, fUnitHexSpriteHexY]);
                        corpse.UnitGraphics = UnitHexSprite.UnitGraphicsResource;
                        sequence = corpse.UnitGraphics.Sequences.ImageSequences[30];
                        if (sequence != null)
                        {
                            corpse.Frame = Random(sequence.GetTotalSequenceFrames());
                        }

                        switch (DeathMethod)
                        {
                            case TacticalBattleUnitDeathMethod.Normal:
                                switch (Troop.GetBloodType())
                                {
                                    case BloodType.None:
                                        corpse.SequenceIndex = -1;
                                        break;
                                    default:
                                        {
                                            corpse.SequenceIndex = ((int)Troop.GetUnitSize() & 127) + 30;
                                            if (corpse.SequenceIndex > 32)
                                            {
                                                corpse.SequenceIndex = 32;
                                            }
                                        }

                                        break;
                                }

                                break;
                            case TacticalBattleUnitDeathMethod.Explode:
                                corpse.SequenceIndex = -1;
                                break;
                        }

                        corpse.OffsetX = Random(10) - 5;
                        corpse.OffsetY = Random(10) - 5;
                        if (Troop.GetUnitType() != UnitType.Machine)
                        {
                            corpse.Unit = this;
                        }

                        corpse.Activate();
                    }
                }

                var hexY2 = UnitHexSprite.GetHexY();
                var hexX2 = UnitHexSprite.GetHexX();
                var hexY3 = UnitHexSprite.GetHexY();
                R = new Rect(UnitHexSprite.GetHexX(), hexY3, hexX2 + 1, hexY2 + 1);
                UnitHexSprite?.Dispose();
                UnitHexSprite = null;
                CombatMap.Current.TriggerMapChangedEvent(R, 0);
            }
        }
        else
        {
            if (UnitHexSprite != null)
            {
                var unitGraphicsResourceIndex = Troop.GetUnitGraphicsResourceIndex();
                UnitHexSprite.SetUnitGraphicsResourceIndex(unitGraphicsResourceIndex);
            }
        }

        return result;
    }

    // AoWTC.pas:2605
    public int GetPositionValue()
    {
        int result = GetAttack() + GetDamage() + GetDefense() + GetMoves() / 3;
        if (SupportCombatMode(Set<CombatMode>.Of(CombatMode.MediumRange)))
        {
            result = result - 100;
        }

        if (SupportCombatMode(Set<CombatMode>.Of(CombatMode.LongRange)))
        {
            result = result - 200;
        }

        return result;
    }

    // AoWTC.pas:2618
    public void MovedTo(AoWMapField oldField, AoWMapField newField, int moveCost)
    {
        MovePoints = MovePoints - moveCost;
        if (MovePoints < 0)
        {
            MovePoints = 0;
        }

        if (GetAbilityEnabled(43))
        {
            var hexY = UnitHexSprite.GetHexY();
            EffectPaths.CreateFire(false, 50, UnitHexSprite.GetHexX(), hexY);
        }

        CombatMap.Current._unitChangedEventsStore.Raise(UnitHexSprite);
    }

    internal static void MakeHitBlood(TacticalBattleUnit unitObject, int damage)
    {
        BattleEffectSprite effect;
        Point3D point;
        effect = CombatMap.Current.AddEffect(BattleEffectType.Blood);
        point = new Point3D();
        effect.Path.Add(point);
        switch (damage)
        {
            case >= 1 and <= 3:
                point.Z = 1;
                break;
            case >= 4 and <= 7:
                point.Z = 2;
                break;
            case >= 8 and <= 100:
                point.Z = 3;
                break;
        }

        switch (unitObject.Troop.GetBloodType())
        {
            case BloodType.None:
                point.Z = point.Z + 5;
                break;
        }

        var hexY = unitObject.UnitHexSprite.HexY;
        HexGeometry.HexToPixel(unitObject.UnitHexSprite.HexX, hexY, ref point.X, ref point.Y);
        point.X = Random(10) + point.X + 11;
        var unitSize = unitObject.Troop.GetUnitSize();
        point.Y = Random(10) + point.Y + 9 - (int)unitSize * 10;
    }

    internal static CombatMoveTokenEvent CreateCombatMoveTokenEvent(TacticalBattleUnit unitObject, MovePath path)
    {
        CombatMoveTokenEvent result = new CombatMoveTokenEvent(null);
        if (result != null)
        {
            result.Setup(unitObject, path);
        }

        return result;
    }

    internal static bool MoveCombatUnit(TacticalBattleUnit unitObject, MovePath path)
    {
        int movePoints;
        int cost;
        int index;
        CombatMoveTokenEvent eventValue;
        MovePath newPath;
        bool result = false;
        var player = unitObject.GetPlayer();
        if (AoWHSMap.Current.TokenManager.PlayerReady(player) && path.Count > 1)
        {
            movePoints = unitObject.MovePoints;
            cost = 0;
            index = path.Count;
            while (index != 0 && cost <= movePoints)
            {
                index = index - 1;
                cost = cost + path[index].Tag;
            }

            if (cost > movePoints)
            {
                index = index + 1;
            }

            if (index < path.Count - 1)
            {
                newPath = new MovePath();
                for (int I = index, last = path.Count - 1; I <= last; I++)
                {
                    newPath.Add(path[I]);
                }

                eventValue = CreateCombatMoveTokenEvent(unitObject, newPath);
                if (eventValue != null)
                {
                    var unitObjectPlayer = unitObject.GetPlayer();
                    AoWHSMap.Current.TokenManager.GetTokenControl(unitObjectPlayer).AddEvent(eventValue);
                    eventValue.Release();
                    result = true;
                }

                newPath?.Dispose();
            }
        }

        return result;
    }

    // intf_P3_01.inc:244

    // intf_P3_01.inc:244

}

public enum TacticalBattleUnitDeathMethod : byte
{
    Normal = 0, Explode = 1
}
