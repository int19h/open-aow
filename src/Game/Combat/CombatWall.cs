#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat;

// WallType. HitPoints. Locked
public class CombatWall : CombatObject
{
    private const DamageTypes AllDamageTypes = DamageTypes.Fire | DamageTypes.Cold | DamageTypes.Lightning | DamageTypes.Magic | DamageTypes.Poison | DamageTypes.Death | DamageTypes.Holy | DamageTypes.Physical | DamageTypes.Wall | DamageTypes.NonSpecific;

    // Combat.pas:1670
    public new const int ClassId = 131_759;

    // Combat.pas:1660
    public CombatWall(CustomNode owner) : base(owner)
    {
        Position = 128;
        ControlMode = CombatObjectControlModes.None;
    }

    private byte _wallType;
    private byte _hitPoints;
    private readonly bool _locked;

    // Combat.pas:1724
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _wallType = persister.RawByte("WallType", value: _wallType);
        _hitPoints = persister.Byte("HitPoints", value: _hitPoints);
    }

    // Combat.pas:1673
    public override int GetTargetPriority()
    {
        if (TargetPriorityInvalid)
        {
            int count = 0;
            for (int i = TargetedBy.Count - 1; i >= 0; i--)
            {
                if (TargetedBy.List[i].Second == 0)
                {
                    count += 1;
                }
            }

            TargetPriority = 0;
            for (int i = Combat.Sides[(int)CombatSideIndex.Defender].Count - 1; i >= 0; i--)
            {
                CombatObject objectValue = Combat.Sides[(int)CombatSideIndex.Defender].GetObjects(i);
                TargetPriority += objectValue.IncomingCombatValue - objectValue.IncomingWallCombatValue;
            }

            TargetPriority = (count + 2) * TargetPriority;
            TargetPriorityInvalid = false;
        }

        return TargetPriority;
    }

    // Combat.pas:1693
    public override int GetTargetStrength() => !_locked ? !Combat.CombatData.WallBreached ? 10 : 1 : 0;

    // Combat.pas:1752
    public override bool GetLocked() => _locked;

    // Combat.pas:1731
    public override sbyte GetDefense() => 0;

    // Combat.pas:1734
    public override sbyte GetResistance() => 3;

    // Combat.pas:1746
    public override DamageTypes GetImmunityTypes()
    {
        DamageTypes result = DamageTypes.Wall;
        return AllDamageTypes & ~result;
    }

    // Combat.pas:1740
    public override sbyte GetHits() => _wallType == 2 ? (sbyte)10 : (sbyte)5;

    // Combat.pas:1743
    public override sbyte HitPoints => (sbyte)_hitPoints;

    // Combat.pas:1755
    public override void SetHitPoints(byte value) => _hitPoints = value;

    // Combat.pas:1712
    public override void Activate(Battle combat)
    {
        base.Activate(combat);
        combat.CombatData.Wall = this;
    }

    // Combat.pas:1718
    public override void Deactivate()
    {
        Combat.CombatData.Wall = null;
        base.Deactivate();
    }

    // Combat.pas:1701
    public override int FastBattleRoundDistance() => 0;

    // Combat.pas:1764
    public override void Show(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        switch (_wallType)
        {
            case 1:
                GamePack.Current.CombatImages[21].Show(X + 15, Y + 5, lockedSurface: surface);
                break;
            case 2:
                GamePack.Current.CombatImages[22].Show(X + 15, Y + 5, lockedSurface: surface);
                break;
            default:
                break;
        }
        HitChance.ShowHitPercent(percent: HitChance.HitPointsToHitPercent(hits: GetHits(), hitPoints: (sbyte)_hitPoints), x: X, y: Y - 10, surface: surface);
    }

    // Combat.pas:1773
    public override int ShowAttackAnimation(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        Show(frame, direction, X, Y, surface);
        return 0;
    }

    // Combat.pas:1776
    public override int ShowHitAnimation(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        Show(frame, direction, X, Y, surface);
        return 0;
    }

    // Combat.pas:1758
    public override void DestroyObject()
    {
        base.DestroyObject();
        Combat.CombatData.WallBreached = true;
    }

    // Combat.pas:1737
    public override string IdString() => "Wall";

    // Combat.pas:1704
    public void Setup(byte id, byte wallType, byte combatPlayer)
    {
        Id = id;
        _wallType = wallType;
        _hitPoints = (byte)GetHits();
        SetCombatPlayer(combatPlayer);
    }
}
