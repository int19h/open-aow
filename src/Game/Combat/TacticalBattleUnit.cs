#nullable disable warnings
// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Combat;

// MovePoints. UnitHexSprite. PositionIndex. PositionGroup. CanFlee. DeathMethod
public class TacticalBattleUnit(CustomNode? owner) : CombatUnit(owner)
{
    public int MovePoints { get; set; }
    public TacticalBattleUnitHexSprite? UnitHexSprite { get; set; }
    public byte PositionIndex { get; set; }
    public byte PositionGroup { get; set; }
    public bool CanFlee { get; set; }
    public TacticalBattleUnitDeathMethod DeathMethod { get; set; }

    // AoWTC.pas:2626
    public override void MessageProc(ref EngineMessage message, EngineObject? sender) => base.MessageProc(ref message, sender);

    // AoWTC.pas:2612
    public override void NewTurn(byte combatPlayer)
    {
        base.NewTurn(combatPlayer);
        if (combatPlayer == (byte)CombatPlayer && GetEnabled())
        {
            MovePoints = Moves;
        }
    }

    // AoWTC.pas:2629
    public override int ExecuteDamage(int damage)
    {
        void MakeBlood()
        {
            if (GetEnabled() && UnitHexSprite is not null && Troop.BloodType != BloodType.None)
            {
                int hitPoints = HitPoints;
                int count = damage >= hitPoints && (GetAbilityEnabled(131) || DeathMethod == TacticalBattleUnitDeathMethod.Explode)
                    ? Random(5) + 10
                    : Random(damage + 7) - 4;

                int x2 = default;
                int y2 = default;
                while (count > 0)
                {
                    int direction = Random(12);
                    if (direction > 6)
                    {
                        direction = 0;
                    }

                    int hexY = UnitHexSprite.HexY;
                    HexGeometry.CenterHexNumberToHex(UnitHexSprite.HexX, hexY, hexNumber: direction, ref x2, ref y2);
                    if (CombatMap.Current.MapContainer.MapLevels[0].Field[x2, y2].TerrainType is not 0 and not 9)
                    {
                        TacticalBattleBloodHexSprite blood = new(CombatMap.Current.MapContainer.MapLevels[0].Field[x2, y2]);
                        blood.Activate();
                        blood.OffsetX = Random(32) - 32;
                        blood.OffsetY = Random(32) - 32;
                        blood.Frame = Random(GamePack.Current.ImageSequences.ImageSequences[4].TotalSequenceFrames);
                    }

                    count -= 1;
                }
            }
        }

        MakeBlood();
        int result = base.ExecuteDamage(damage);
        if (!GetEnabled())
        {
            if (UnitHexSprite is not null)
            {
                if (GetAbilityEnabled(131))
                {
                    MakeHitBlood(this, damage: 100);
                    for (int i = 0; i <= 9; i++)
                    {
                        BattleEffectSprite effect = CombatMap.Current.AddEffect(BattleEffectType.DecayComet);
                        effect.AbilityIndex = 52;
                        effect.SpellIndex = 106;
                        int hexY = UnitHexSprite.HexY;
                        HexGeometry.HexToPixel(UnitHexSprite.HexX, hexY, out int x, out int y);
                        Point3D xyz = new();
                        effect.Path.Add(xyz);
                        Point3DList path2 = effect.Path;
                        path2[0].X = x + 16;
                        effect.Path = path2;
                        Point3DList path3 = effect.Path;
                        path3[0].Y = y + 8;
                        effect.Path = path3;
                        for (int j = 0; j <= 10; j++)
                        {
                            float speed = (float)((Random(50) + 50) * 0.05);
                            float gravity = (float)1.5;
                            float vz = (float)((Random(25) + 25) * 0.05);
                            float z = 0;
                            int count = effect.Path.Count;
                            Point3D? path = effect.Path[count - 1];
                            int fPathCount = effect.Path.Count;
                            Point3D? pathZ = effect.Path[fPathCount - 1];
                            int count2 = effect.Path.Count;
                            Point3D? pathX = effect.Path[count2 - 1];
                            Point3DList path4 = effect.Path;
                            EffectPaths.MakeArcPath(angle: Random(360), x: pathX.X, y: path.Y - pathZ.Z, z: ref z, speed: speed, vz: ref vz, gravity: gravity, list: path4);
                            effect.Path = path4;
                        }
                    }
                }
                else
                {
                    int hexY = UnitHexSprite.HexY;
                    int hexX = UnitHexSprite.HexX;
                    bool condition = CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].TerrainType != 0;
                    if (condition)
                    {
                        int fUnitHexSpriteHexY = UnitHexSprite.HexY;
                        int fUnitHexSpriteHexX = UnitHexSprite.HexX;
                        condition = CombatMap.Current.MapContainer.MapLevels[0].Field[fUnitHexSpriteHexX, fUnitHexSpriteHexY].TerrainType != 9;
                    }

                    if (condition)
                    {
                        int fUnitHexSpriteHexY = UnitHexSprite.HexY;
                        int fUnitHexSpriteHexX = UnitHexSprite.HexX;
                        TacticalBattleCorpseHexSprite corpse = new(CombatMap.Current.MapContainer.MapLevels[0].Field[fUnitHexSpriteHexX, fUnitHexSpriteHexY])
                        {
                            UnitGraphics = UnitHexSprite.UnitGraphicsResource
                        };
                        ImageSequence? sequence = corpse.UnitGraphics.Sequences.ImageSequences[30];
                        if (sequence is not null)
                        {
                            corpse.Frame = Random(sequence.TotalSequenceFrames);
                        }

                        switch (DeathMethod)
                        {
                            case TacticalBattleUnitDeathMethod.Normal:
                                switch (Troop.BloodType)
                                {
                                    case BloodType.None:
                                        corpse.SequenceIndex = -1;
                                        break;
                                    case BloodType.Red:
                                    case BloodType.Blue:
                                    case BloodType.Green:
                                    default:
                                        {
                                            corpse.SequenceIndex = ((int)Troop.UnitSize & 0b0111_1111) + 30;
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
                            default:
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

                int hexY2 = UnitHexSprite.HexY;
                int hexX2 = UnitHexSprite.HexX;
                int hexY3 = UnitHexSprite.HexY;
                Rect r = new(UnitHexSprite.HexX, hexY3, hexX2 + 1, hexY2 + 1);
                UnitHexSprite?.Dispose();
                UnitHexSprite = null;
                CombatMap.Current.TriggerMapChangedEvent(r, L: 0);
            }
        }
        else
        {
            if (UnitHexSprite is not null)
            {
                int unitGraphicsResourceIndex = Troop.UnitGraphicsResourceIndex;
                UnitHexSprite.SetUnitGraphicsResourceIndex(unitGraphicsResourceIndex);
            }
        }

        return result;
    }

    // AoWTC.pas:2605
    public int GetPositionValue()
    {
        int result = GetAttack() + GetDamage() + GetDefense() + (Moves / 3);
        if (SupportCombatMode(CombatModes.MediumRange))
        {
            result -= 100;
        }

        if (SupportCombatMode(CombatModes.LongRange))
        {
            result -= 200;
        }

        return result;
    }

    // AoWTC.pas:2618
    public void MovedTo(MapField? oldField, MapField newField, int moveCost)
    {
        MovePoints -= moveCost;
        if (MovePoints < 0)
        {
            MovePoints = 0;
        }

        if (GetAbilityEnabled(43))
        {
            int hexY = UnitHexSprite.HexY;
            EffectPaths.CreateFire(50, UnitHexSprite.HexX, hexY, engine: GetEngine());
        }

        CombatMap.Current.OnUnitChanged(UnitHexSprite);
    }

    internal static void MakeHitBlood(TacticalBattleUnit unitObject, int damage)
    {
        BattleEffectSprite effect = CombatMap.Current.AddEffect(BattleEffectType.Blood);
        Point3D point = new();
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
            default:
                break;
        }

        switch (unitObject.Troop.BloodType)
        {
            case BloodType.None:
                point.Z += 5;
                break;
            case BloodType.Red:
            case BloodType.Blue:
            case BloodType.Green:
            default:
                break;
        }

        int hexY = unitObject.UnitHexSprite.HexY;
        _ = point.X;
        _ = point.Y;
        HexGeometry.HexToPixel(unitObject.UnitHexSprite.HexX, hexY, out int x, out int y);
        point.X = x;
        point.Y = y;
        point.X = Random(10) + point.X + 11;
        UnitSize unitSize = unitObject.Troop.UnitSize;
        point.Y = Random(10) + point.Y + 9 - ((int)unitSize * 10);
    }

    internal static CombatMoveTokenEvent? CreateCombatMoveTokenEvent(TacticalBattleUnit unitObject, MovePath path)
    {
        CombatMoveTokenEvent result = new(null) { Holder = unitObject };
        _ = (result?.Setup(unitObject, path));

        return result;
    }

    internal static bool MoveCombatUnit(TacticalBattleUnit unitObject, MovePath path)
    {
        bool result = false;
        sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(unitObject.GetPlayer());
        if (StrategicMap.Current.TokenManager.PlayerReady(PlayerId.FromOriginal(player)) && path.Count > 1)
        {
            int movePoints = unitObject.MovePoints;
            int cost = 0;
            int index = path.Count;
            while (index != 0 && cost <= movePoints)
            {
                index--;
                cost += path[index].MoveCost;
            }

            if (cost > movePoints)
            {
                index++;
            }

            if (index < path.Count - 1)
            {
                MovePath newPath = [];
                for (int i = index, last = path.Count - 1; i <= last; i++)
                {
                    _ = newPath.Add(path[i]);
                }

                CombatMoveTokenEvent? eventValue = CreateCombatMoveTokenEvent(unitObject, path: newPath);
                if (eventValue is not null)
                {
                    sbyte unitObjectPlayer = (sbyte)PlayerIdExtensions.ToOriginal(unitObject.GetPlayer());
                    StrategicMap.Current.TokenManager.GetTokenControl(PlayerId.FromOriginal(unitObjectPlayer)).AddEvent(eventValue);
                    _ = eventValue.Release();
                    result = true;
                }

                newPath?.Dispose();
            }
        }

        return result;
    }
}

public enum TacticalBattleUnitDeathMethod : byte
{
    Normal = 0, Explode = 1
}
