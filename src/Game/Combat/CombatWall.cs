// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat;

// WallType. HitPoints. Locked
public partial class CombatWall : CombatObject
{
    // Combat.pas:1670
    public new const int ClassId = 131759;

    // Combat.pas:1660
    public CombatWall(ECustomNode owner) : base(owner)
    {
        Position = 128;
        ControlMode = Set<CombatObjectControlModeElement>.Of();
    }

    private byte _wallType;
    private byte _hitPoints;
    private bool _locked;

    // Combat.pas:1724
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwData(Bytes.Of(ref _wallType, 1), 1, 20);
        stream.rwByte(ref _hitPoints, 21);
    }

    // Combat.pas:1673
    public override int GetTargetPriority()
    {
        int count;
        CombatObject objectValue;
        if (TargetPriorityInvalid)
        {
            count = 0;
            for (int I = TargetedBy.Count - 1; I >= 0; I--)
            {
                if (TargetedBy.List[I].Second == 0)
                {
                    count += 1;
                }
            }

            TargetPriority = 0;
            for (int I = Combat.Sides[(int)CombatSideIndex.Defender].Count - 1; I >= 0; I--)
            {
                objectValue = Combat.Sides[(int)CombatSideIndex.Defender].GetObjects(I);
                TargetPriority += objectValue.IncomingCombatValue - objectValue.IncomingWallCombatValue;
            }

            TargetPriority = (count + 2) * TargetPriority;
            TargetPriorityInvalid = false;
        }

        return TargetPriority;
    }

    // Combat.pas:1693
    public override int GetTargetStrength()
    {
        int result;
        if (!_locked)
        {
            if (!Combat.CombatData.WallBreached)
            {
                result = 10;
            }
            else
            {
                result = 1;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Combat.pas:1752
    public override bool GetLocked() => _locked;

    // Combat.pas:1731
    public override sbyte GetDefense() => 0;

    // Combat.pas:1734
    public override sbyte GetResistance() => 3;

    // Combat.pas:1746
    public override Set<DamageType> GetImmunityTypes()
    {
        Set<DamageType> result = Set<DamageType>.Of(DamageType.Wall);
        return Set<DamageType>.Of().WithRange(0, (DamageType)9) - result;
    }

    // Combat.pas:1740
    public override sbyte GetHits()
    {
        sbyte result;
        if (_wallType == 2)
        {
            result = 10;
        }
        else
        {
            result = 5;
        }

        return result;
    }

    // Combat.pas:1743
    public override sbyte GetHitPoints() => (sbyte)_hitPoints;

    // Combat.pas:1755
    public override void SetHitPoints(byte value)
    {
        _hitPoints = value;
    }

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
                GamePack.Current.CombatImages[21].Show(X + 15, Y + 5, surface);
                break;
            case 2:
                GamePack.Current.CombatImages[22].Show(X + 15, Y + 5, surface);
                break;
        }

        var hits = GetHits();
        var hitPointsToHitPercent = HitChance.HitPointsToHitPercent((sbyte)_hitPoints, hits);
        HitChance.ShowHitPercent(X, Y - 10, surface, hitPointsToHitPercent);
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

    // intf_P3_01.inc:316

}
