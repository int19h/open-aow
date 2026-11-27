#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Combat;

// WallType. HitPoints. Locked
public class CombatWall : CombatObject
{
    private const DamageTypes AllDamageTypes = DamageTypes.Fire | DamageTypes.Cold | DamageTypes.Lightning | DamageTypes.Magic | DamageTypes.Poison | DamageTypes.Death | DamageTypes.Holy | DamageTypes.Physical | DamageTypes.Wall | DamageTypes.NonSpecific;

    // Combat.pas:1670
    public new const int ClassId = 131_759;

    // Combat.pas:1660
    public CombatWall(CustomNode? owner) : base(owner)
    {
        Position = 128;
        ControlMode = CombatObjectControlModes.None;
    }

    private int _wallType;
    private int _hitPoints;
    private readonly bool _locked;

    // Combat.pas:1724
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _wallType = persister.RawByte("WallType", value: _wallType.TruncateToByte());
        _hitPoints = persister.Byte("HitPoints", value: _hitPoints.TruncateToByte());
    }

    // Combat.pas:1673
    public override int GetTargetPriority()
    {
        if (TargetPriorityInvalid)
        {
            int count = TargetedBy.Count(static entry => entry.Second == 0);
            TargetPriority = 0;
            TargetPriority = Combat.Sides[(int)CombatSideIndex.Defender].Sum(static item => item.IncomingCombatValue - item.IncomingWallCombatValue);
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
    public override int GetDefense() => 0;

    // Combat.pas:1734
    public override int GetResistance() => 3;

    // Combat.pas:1746
    public override DamageTypes GetImmunityTypes()
    {
        DamageTypes result = DamageTypes.Wall;
        return AllDamageTypes & ~result;
    }

    // Combat.pas:1740
    public override int GetHits() => _wallType == 2 ? 10 : 5;

    // Combat.pas:1743
    public override int HitPoints => _hitPoints.TruncateToSByte();

    // Combat.pas:1755
    public override void SetHitPoints(int value) => _hitPoints = value;

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
        HitChance.ShowHitPercent(percent: HitChance.HitPointsToHitPercent(hits: GetHits(), hitPoints: _hitPoints.TruncateToSByte()), x: X, y: Y - 10, surface: surface, engine: GetEngine()!);
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
    public void Setup(byte id, int wallType, byte combatPlayer)
    {
        Id = id;
        _wallType = wallType;
        _hitPoints = GetHits().TruncateToByte();
        SetCombatPlayer(combatPlayer);
    }
}
