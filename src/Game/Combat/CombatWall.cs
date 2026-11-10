// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat;

// FWallType. FHitPoints. FLocked
public partial class CombatWall : CombatObject
{
    // Combat.pas:1670
    public new const int ClassID = 131759;

    // Combat.pas:1660
    public CombatWall(ECustomNode owner) : base(owner)
    {
        FPosition = 128;
        FControlMode = Set<CombatObjectControlModeElement>.Of();
    }

    public byte FWallType;
    public byte FHitPoints;
    public bool FLocked;

    // Combat.pas:1724
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FWallType, 1), 1, 20);
        Stream.rwByte(ref FHitPoints, 21);
    }

    // Combat.pas:1673
    public override int GetTargetPriority()
    {
        int Count;
        CombatObject Obj;
        if (FTargetPriorityInvalid)
        {
            Count = 0;
            for (int I = FTargetedBy.Count - 1; I >= 0; I--)
            {
                if (FTargetedBy.List[I].B == 0)
                {
                    Count += 1;
                }
            }

            FTargetPriority = 0;
            for (int I = FCombat.FSides[(int)CombatSideIndex.Defender].Count - 1; I >= 0; I--)
            {
                Obj = FCombat.FSides[(int)CombatSideIndex.Defender].GetObjects(I);
                FTargetPriority += Obj.FIncomingCombatValue - Obj.FIncomingWallCombatValue;
            }

            FTargetPriority = (Count + 2) * FTargetPriority;
            FTargetPriorityInvalid = false;
        }

        return FTargetPriority;
    }

    // Combat.pas:1693
    public override int GetTargetStrength()
    {
        int Result;
        if (!FLocked)
        {
            if (!FCombat.FCombatData.FWallBreached)
            {
                Result = 10;
            }
            else
            {
                Result = 1;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Combat.pas:1752
    public override bool GetLocked() => FLocked;

    // Combat.pas:1731
    public override sbyte GetDefense() => 0;

    // Combat.pas:1734
    public override sbyte GetResistance() => 3;

    // Combat.pas:1746
    public override Set<DamageType> GetImmunityTypes()
    {
        Set<DamageType> Result = Set<DamageType>.Of(DamageType.Wall);
        return Set<DamageType>.Of().WithRange(0, (DamageType)9) - Result;
    }

    // Combat.pas:1740
    public override sbyte GetHits()
    {
        sbyte Result;
        if (FWallType == 2)
        {
            Result = 10;
        }
        else
        {
            Result = 5;
        }

        return Result;
    }

    // Combat.pas:1743
    public override sbyte GetHitPoints() => (sbyte)FHitPoints;

    // Combat.pas:1755
    public override void SetHitPoints(byte Value)
    {
        FHitPoints = Value;
    }

    // Combat.pas:1712
    public override void Activate(Battle ACombat)
    {
        base.Activate(ACombat);
        ACombat.FCombatData.FWall = this;
    }

    // Combat.pas:1718
    public override void Deactivate()
    {
        FCombat.FCombatData.FWall = null;
        base.Deactivate();
    }

    // Combat.pas:1701
    public override int fcRoundDistance() => 0;

    // Combat.pas:1764
    public override void Show(int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        switch (FWallType)
        {
            case 1:
                AoWHSSet.Current.FCombatImages[21].Show(X + 15, Y + 5, Surface);
                break;
            case 2:
                AoWHSSet.Current.FCombatImages[22].Show(X + 15, Y + 5, Surface);
                break;
        }

        var hits = GetHits();
        var hitPointsToHitPcnt = HitChance.HitPointsToHitPcnt((sbyte)FHitPoints, hits);
        HitChance.ShowHitPcnt(X, Y - 10, Surface, hitPointsToHitPcnt);
    }

    // Combat.pas:1773
    public override int ShowAttackAnimation(int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        Show(Frame, Direction, X, Y, Surface);
        return 0;
    }

    // Combat.pas:1776
    public override int ShowHitAnimation(int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        Show(Frame, Direction, X, Y, Surface);
        return 0;
    }

    // Combat.pas:1758
    public override void DestroyObject()
    {
        base.DestroyObject();
        FCombat.FCombatData.FWallBreached = true;
    }

    // Combat.pas:1737
    public override string IDStr() => "Wall";

    // Combat.pas:1704
    public void Setup(byte ID, byte WallType, byte CombatPlayer)
    {
        FID = ID;
        FWallType = WallType;
        FHitPoints = (byte)GetHits();
        SetCombatPlayer(CombatPlayer);
    }

    // intf_P3_01.inc:316

}
