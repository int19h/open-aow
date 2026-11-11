// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// FStatus. FShowPriority. FUnitGFXResource. FDirection. FMoveStep. FFlagID. FShadow
public partial class UnitHexSprite : HexSprite
{
    public UnitHexSprite(ECustomNode owner) : base(owner)
    {
        FMoveStep = -1;
        FDirection = 3;
        FShowPriority = 1744830464;
    }

    public new const int ClassID = 131604;

    public byte FStatus;
    // visible, edit, selected, paused, instant move
    public int FShowPriority;
    public UnitGraphicsResource FUnitGFXResource;
    public byte FDirection;
    public sbyte FMoveStep;
    // -1 when idle
    public byte FFlagID;
    public UnitShadowHexSprite FShadow;

    // UnitHS.PAS:168
    public override void Dispose()
    {
        DestroyShadow();
        SetUnitGFXResourceIndex(-1);
        base.Dispose();
    }

    // UnitHS.PAS:219
    public override void SetOwner(ECustomNode AOwner)
    {
        if (AOwner != FOwner)
        {
            if (FOwner != null)
            {
                FOwner.RemoveChild(this);
            }

            FOwner = AOwner;
            if (FOwner != null)
            {
                FOwner.AddChild(this);
            }
        }
    }

    // UnitHS.PAS:208
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FDirection, 9);
        if (!Stream.StorageMode().Contains(StorageModeElement.SetupCompare) && FDirection == 0)
        {
            FDirection = 3;
        }
    }

    // UnitHS.PAS:610
    public override void TimerEvent(int Tick, EngineObject Sender)
    {
    }

    // UnitHS.PAS:655
    public override byte GetLevel() => 26;

    // UnitHS.PAS:569
    public override void SetVisible(bool Value)
    {
        if (Value)
        {
            FStatus = (byte)(FStatus | 1);
        }
        else
        {
            FStatus = (byte)(FStatus & 254);
        }
    }

    // UnitHS.PAS:574
    public override bool GetVisible() => (FStatus & 1) != 0;

    // UnitHS.PAS:567
    public override bool GetSelected() => (FStatus & 4) != 0;

    // UnitHS.PAS:228
    public override sbyte GetXhx() => ((MapField)Owner).Xhx;

    // UnitHS.PAS:230
    public override sbyte GetYhx() => ((MapField)Owner).Yhx;

    // UnitHS.PAS:232
    public override sbyte GetLhx()
    {
        Assert(Owner != null, "Onwer isn't defined", "UnitHS.PAS", 557);
        return ((MapField)Owner).Lhx;
    }

    // UnitHS.PAS:576
    public override bool GetEditMode() => (FStatus & 2) != 0;

    // UnitHS.PAS:578
    public override void SetEditMode(bool Value)
    {
        if (Value)
        {
            FStatus = (byte)(FStatus | 2);
        }
        else
        {
            FStatus = (byte)(FStatus & 253);
        }
    }

    // UnitHS.PAS:651
    public override int GetShowPriority() => FShowPriority;

    // UnitHS.PAS:692
    public override void Connect()
    {
        MapField Field;
        MapLevel Level;
        HexNumberWalkData Walk = default;
        CalcShowPriority();
        Field = (MapField)Owner;
        Field.RemoveChild(this);
        Level = (MapLevel)Field.Owner;
        HexNumberWalkData.InitHNWalk(ref Walk, Field.Xhx, Field.Yhx, 0);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.X < Level.Width && Walk.Y >= 0 && Walk.Y < Level.Height)
            {
                Level.GetField(Walk.X, Walk.Y).AddHS(this, Walk.HN);
                Level.GetField(Walk.X, Walk.Y).UpdateMapField();
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }
    }

    // UnitHS.PAS:716
    public override void Disconnect()
    {
        MapField Field;
        MapLevel Level;
        HexNumberWalkData Walk = default;
        if (Owner != null)
        {
            Field = (MapField)Owner;
            Level = (MapLevel)Field.Owner;
            HexNumberWalkData.InitHNWalk(ref Walk, Field.Xhx, Field.Yhx, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.X < Level.Width && Walk.Y >= 0 && Walk.Y < Level.Height)
                {
                    Level.GetField(Walk.X, Walk.Y).RemoveHS(this);
                    Level.GetField(Walk.X, Walk.Y).UpdateMapField();
                }

                HexNumberWalkData.WalkToNextHN(ref Walk);
            }

            SetOwner(null);
        }
    }

    // UnitHS.PAS:214
    public override void TerrainChanged(in HexSpriteTerrainInfo Info)
    {
        base.TerrainChanged(Info);
        CalcShowPriority();
    }

    // UnitHS.PAS:742
    public override bool PlaceOnMap()
    {
        Connect();
        ((GameMap)((MapLevel)Owner.Owner).Map).ScannerIconsChangedRad(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ((MapField)Owner).Lhx, 0);
        return true;
    }

    // UnitHS.PAS:751
    public override void RemoveFromMap()
    {
        MapField Field;
        if (Owner != null)
        {
            Field = (MapField)Owner;
            if (Moving())
            {
                CancelMove();
            }

            Disconnect();
            Field.UpdateMapField();
            ((GameMap)((MapLevel)Field.Owner).Map).ScannerIconsChangedRad(Field.Xhx, Field.Yhx, Field.Lhx, 0);
        }
    }

    // UnitHS.PAS:809
    public override bool Place(MapContainer Map, int Xhp, int Yhp, int Lhx)
    {
        int X = default;
        int Y = default;
        HexGeometry.HPtoHX(Xhp, Yhp, ref X, ref Y);
        return PlaceHX(Map, (sbyte)X, (sbyte)Y, (sbyte)Lhx);
    }

    // UnitHS.PAS:779
    public override void Remove()
    {
        MapField Field;
        Field = (MapField)Owner;
        RemoveFromMap();
        if (GetVisible())
        {
            ((MapLevel)Field.Owner).Map.DynamicVisibleHS.RemoveChild(this);
            SetVisible(false);
        }
    }

    // UnitHS.PAS:986
    public override void MapFieldMsgProc(ref MapFieldMessage Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 16 && Msg.HN == 0 && AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            Dispose();
        }
    }

    // UnitHS.PAS:678
    public override int CanMoveOver(in HexSpriteSenderInfo Info)
    {
        if (Info.HN == 0)
        {
            return 0;
        }

        return 1;
    }

    // UnitHS.PAS:663
    public override void Activate()
    {
        base.Activate();
        CreateShadow();
        UpdateFlagID();
    }

    // UnitHS.PAS:671
    public override void Deactivate()
    {
        DestroyShadow();
        base.Deactivate();
    }

    // UnitHS.PAS:685
    public override void UpdateMapField(in HexSpriteMapFieldInfo Info)
    {
        if (Info.HN <= 7)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 8192;
        }
    }

    // UnitHS.PAS:195
    public override void MakeVisible(VisibilityList VisibilityList)
    {
        if ((FStatus & 1) == 0)
        {
            FStatus = (byte)(FStatus | 1);
            VisibilityList.AddSortChild(this, FShowPriority);
            // { Read Shadow directly. }
            if (FShadow != null)
            {
                var showPriority = FShadow.GetShowPriority();
                VisibilityList.AddSortChild(FShadow, showPriority);
            }
        }
    }

    // UnitHS.PAS:613
    public override bool CanSelect(int Xhp, int Yhp)
    {
        bool Result;
        int X = default;
        int Y = default;
        if (!Moving())
        {
            HexGeometry.HPtoHX(Xhp, Yhp, ref X, ref Y);
            if (((MapField)Owner).Xhx != X || ((MapField)Owner).Yhx != Y)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // UnitHS.PAS:560
    public override void Select()
    {
        FStatus = (byte)(FStatus | 4);
    }

    // UnitHS.PAS:562
    public override void Unselect()
    {
        FStatus = (byte)(FStatus & 251);
        base.Unselect();
    }

    // UnitHS.PAS:627
    public virtual void CalcShowPriority()
    {
        MapField Field;
        HexSprite HS;
        int X = default;
        int Y = default;
        Field = (MapField)Owner;
        if (Moving() && Set<byte>.WithBounds(0, 255).WithRange(3, 5).Contains(FDirection))
        {
            HexGeometry.CenterHNtoHX(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, FDirection, ref X, ref Y);
            Field = ((MapLevel)Owner.Owner).GetField(X, Y);
            HexGeometry.HXtoHP(X, Y, ref X, ref Y);
        }
        else
        {
            HexGeometry.HXtoHP(Field.Xhx, Field.Yhx, ref X, ref Y);
        }

        HS = Field.GetMaxLevelNoneTransparentHS();
        FShowPriority = 1744830464 + ((Y + 34 + 6) << 13) + (X + 16);
        if (HS != this && HS != null && HS.GetShowPriority() >= FShowPriority)
        {
            FShowPriority = HS.GetShowPriority() + 2;
        }

        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
        SortMapFieldEx(((MapLevel)Owner.Owner).Map.MapContainer, X - 18, Y - 32, ((MapField)Owner).Lhx, 64, 64);
    }

    // UnitHS.PAS:766
    public virtual bool MoveTo(MapField Field, int MoveCost)
    {
        if (Owner != null)
        {
            RemoveFromMap();
        }

        FOwner = Field;
        CalcShowPriority();
        PlaceOnMap();
        Field.RemoveChild(this);
        FOwner = null;
        SetOwner(Field);
        return true;
    }

    // UnitHS.PAS:793
    public virtual bool PlaceHX(MapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        MapField Field;
        bool Success;
        Field = Map.GetField(Xhx, Yhx, Lhx);
        FOwner = Field;
        CalcShowPriority();
        Success = PlaceOnMap();
        Field.RemoveChild(this);
        FOwner = null;
        SetOwner(Field);
        return Success;
    }

    // UnitHS.PAS:659
    public virtual void UpdateFlagID()
    {
        FFlagID = 32;
    }

    // UnitHS.PAS:910
    public virtual sbyte MoveFirst(int Direction, bool Instant)
    {
        if (FMoveStep != -1)
        {
            CancelMove();
        }

        FStatus = (byte)(FStatus & 231);
        if (Instant)
        {
            FStatus = (byte)(FStatus | 16);
        }

        FDirection = (byte)Direction;
        FMoveStep = 0;
        CalcShowPriority();
        return 0;
    }

    // UnitHS.PAS:974
    public virtual int GetMoveSteps()
    {
        int Result;
        switch (FUnitGFXResource.F_UnitGFXResource_30)
        {
            case 0:
                return 12;
            case 1:
                return 10;
            case 2:
                return 8;
            case 3:
                return 6;
            default:
                Result = 8;
                break;
        }

        return Result;
    }

    // UnitHS.PAS:176
    public void CreateShadow()
    {
        if (FShadow != null)
        {
            DestroyShadow();
        }

        if (AoWHSMap.Current.FShowUnitShadows)
        {
            FShadow = new UnitShadowHexSprite(null);
            FShadow.FUnitHS = this;
        }
    }

    // UnitHS.PAS:185
    public void DestroyShadow()
    {
        if (FShadow != null)
        {
            if (GetVisible())
            {
                ((MapLevel)Owner.Owner).Map.DynamicVisibleHS.RemoveChild(FShadow);
            }

            FShadow?.Dispose();
            FShadow = null;
        }
    }

    // UnitHS.PAS:584
    public int GetUnitGFXResourceIndex()
    {
        if (FUnitGFXResource != null)
        {
            return FUnitGFXResource.GetIndex();
        }

        return -1;
    }

    // UnitHS.PAS:594
    public void SetUnitGFXResourceIndex(int Value)
    {
        UnitGraphicsResource Resource;
        UnitGraphicsResource Old;
        // { This eliminated copy preserves the original saved registers. }
        Resource = FUnitGFXResource;
        if (Resource == null || Value != Resource.GetIndex())
        {
            Old = FUnitGFXResource;
            if (Old != null)
            {
                Old.Release();
            }

            Resource = AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Value);
            FUnitGFXResource = Resource;
            if (Resource != null)
            {
                Resource.AddRef();
            }
        }
    }

    // UnitHS.PAS:902
    public void SetDirection(byte Value)
    {
        if (Value != FDirection)
        {
            if (Moving())
            {
                CancelMove();
            }

            FDirection = Value;
        }
    }

    // UnitHS.PAS:920
    public sbyte GetMoveStatus()
    {
        sbyte Result;
        if (FMoveStep >= 0)
        {
            var moveSteps = GetMoveSteps();
            if (FMoveStep < moveSteps)
            {
                var moveSteps2 = GetMoveSteps();
                Result = (sbyte)Round((double)(FMoveStep * 100) / moveSteps2);
            }
            else
            {
                Result = 100;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // UnitHS.PAS:929
    public void SetMoveStatus(sbyte Value)
    {
        if (Value >= 0)
        {
            FMoveStep = (sbyte)((GetMoveSteps() * Value + 50) / 100);
            var moveSteps = GetMoveSteps();
            if (FMoveStep > moveSteps)
            {
                FMoveStep = (sbyte)GetMoveSteps();
                return;
            }
        }
        else
        {
            FMoveStep = -1;
        }
    }

    // UnitHS.PAS:940
    public sbyte MoveNext()
    {
        sbyte Result;
        if (FMoveStep >= 0)
        {
            FStatus = (byte)(FStatus & 247);
            var moveSteps = GetMoveSteps();
            if (FMoveStep < moveSteps)
            {
                FMoveStep = (sbyte)(FMoveStep + 1);
                var moveSteps2 = GetMoveSteps();
                Result = (sbyte)Round((double)(FMoveStep * 100) / moveSteps2);
            }
            else
            {
                Result = 100;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // UnitHS.PAS:956
    public bool Moving() => FMoveStep != -1;

    // UnitHS.PAS:960
    public void PauseMove()
    {
        FStatus = (byte)(FStatus | 8);
    }

    // UnitHS.PAS:964
    public void CancelMove()
    {
        if (FMoveStep != -1)
        {
            FMoveStep = -1;
            FStatus = (byte)(FStatus & 231);
        }
    }

    // intf_P2_03.inc:74

}
