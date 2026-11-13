// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// Status. _showPriority. UnitGraphicsResource. Direction. MoveStep. FlagId. Shadow
public partial class UnitHexSprite : HexSprite
{
    public UnitHexSprite(ECustomNode owner) : base(owner)
    {
        MoveStep = -1;
        Direction = 3;
        _showPriority = 1744830464;
    }

    public new const int ClassId = 131604;

    public byte Status { get; private set; }

    // visible, edit, selected, paused, instant move
    private int _showPriority;

    public UnitGraphicsResource UnitGraphicsResource { get; set; }

    public byte Direction;

    public sbyte MoveStep { get; set; }
    // -1 when idle
    public byte FlagId { get; protected set; }

    private UnitShadowHexSprite _shadow;

    // UnitHS.PAS:168
    public override void Dispose()
    {
        DestroyShadow();
        SetUnitGraphicsResourceIndex(-1);
        base.Dispose();
    }

    // UnitHS.PAS:219
    public override void SetOwner(ECustomNode owner)
    {
        if (owner != Owner)
        {
            if (Owner != null)
            {
                Owner.RemoveChild(this);
            }

            SetOwnerWithoutNotify(owner);
            if (Owner != null)
            {
                Owner.AddChild(this);
            }
        }
    }

    // UnitHS.PAS:208
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Direction, 9);
        if (!stream.StorageMode().Contains(StorageModeElement.SetupCompare) && Direction == 0)
        {
            Direction = 3;
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Direction = persister.Byte("Direction", value: Direction);
        if (Direction == 0)
        {
            Direction = 3;
        }
    }

    // UnitHS.PAS:610
    public override void TimerEvent(int tick, EngineObject sender)
    {
    }

    // UnitHS.PAS:655
    public override byte GetLevel() => 26;

    // UnitHS.PAS:569
    public override void SetVisible(bool value)
    {
        if (value)
        {
            Status = (byte)(Status | 1);
        }
        else
        {
            Status = (byte)(Status & 254);
        }
    }

    // UnitHS.PAS:574
    public override bool GetVisible() => (Status & 1) != 0;

    // UnitHS.PAS:567
    public override bool GetSelected() => (Status & 4) != 0;

    // UnitHS.PAS:228
    public override sbyte GetHexX() => Owner.HexX;

    // UnitHS.PAS:230
    public override sbyte GetHexY() => Owner.HexY;

    // UnitHS.PAS:232
    public override sbyte GetHexLevel()
    {
        Assert(Owner != null, "Onwer isn't defined", "UnitHS.PAS", 557);
        return Owner.HexLevel;
    }

    // UnitHS.PAS:576
    public override bool GetEditMode() => (Status & 2) != 0;

    // UnitHS.PAS:578
    public override void SetEditMode(bool value)
    {
        if (value)
        {
            Status = (byte)(Status | 2);
        }
        else
        {
            Status = (byte)(Status & 253);
        }
    }

    // UnitHS.PAS:651
    public override int GetShowPriority() => _showPriority;

    // UnitHS.PAS:692
    public override void Connect()
    {
        MapField field;
        MapLevel level;
        HexNumberWalkData walk = default;
        CalculateShowPriority();
        field = Owner;
        field.RemoveChild(this);
        level = field.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, field.HexX, field.HexY, 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.X < level.Width && walk.Y >= 0 && walk.Y < level.Height)
            {
                level.GetField(walk.X, walk.Y).AddHexSprite(this, walk.HexNumber);
                level.GetField(walk.X, walk.Y).UpdateMapField();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // UnitHS.PAS:716
    public override void Disconnect()
    {
        MapField field;
        MapLevel level;
        HexNumberWalkData walk = default;
        if (Owner != null)
        {
            field = Owner;
            level = field.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, field.HexX, field.HexY, 0);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.X < level.Width && walk.Y >= 0 && walk.Y < level.Height)
                {
                    level.GetField(walk.X, walk.Y).RemoveHexSprite(this);
                    level.GetField(walk.X, walk.Y).UpdateMapField();
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            SetOwner(null);
        }
    }

    // UnitHS.PAS:214
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        base.TerrainChanged(info);
        CalculateShowPriority();
    }

    // UnitHS.PAS:742
    public override bool PlaceOnMap()
    {
        Connect();
        Owner.Owner.Map.ScannerIconsChangedRadius(Owner.HexX, Owner.HexY, Owner.HexLevel, 0);
        return true;
    }

    // UnitHS.PAS:751
    public override void RemoveFromMap()
    {
        MapField field;
        if (Owner != null)
        {
            field = Owner;
            if (Moving())
            {
                CancelMove();
            }

            Disconnect();
            field.UpdateMapField();
            field.Owner.Map.ScannerIconsChangedRadius(field.HexX, field.HexY, field.HexLevel, 0);
        }
    }

    // UnitHS.PAS:809
    public override bool Place(MapContainer map, int pixelX, int pixelY, int hexLevel)
    {
        int X = default;
        int Y = default;
        HexGeometry.PixelToHex(pixelX, pixelY, out X, out Y);
        return PlaceHex(map, (sbyte)X, (sbyte)Y, (sbyte)hexLevel);
    }

    // UnitHS.PAS:779
    public override void Remove()
    {
        MapField field;
        field = Owner;
        RemoveFromMap();
        if (GetVisible())
        {
            field.Owner.Map.DynamicVisibleHexSprite.RemoveChild(this);
            SetVisible(false);
        }
    }

    // UnitHS.PAS:986
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 16 && message.HexNumber == 0 && StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            Dispose();
        }
    }

    // UnitHS.PAS:678
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        if (info.HexNumber == 0)
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
        UpdateFlagId();
    }

    // UnitHS.PAS:671
    public override void Deactivate()
    {
        DestroyShadow();
        base.Deactivate();
    }

    // UnitHS.PAS:685
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber <= 7)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOverFlag;
        }
    }

    // UnitHS.PAS:195
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if ((Status & 1) == 0)
        {
            Status = (byte)(Status | 1);
            visibilityList.AddSortChild(this, _showPriority);
            // { Read Shadow directly. }
            if (_shadow != null)
            {
                var showPriority = _shadow.GetShowPriority();
                visibilityList.AddSortChild(_shadow, showPriority);
            }
        }
    }

    // UnitHS.PAS:613
    public override bool CanSelect(int pixelX, int pixelY)
    {
        bool result;
        int X = default;
        int Y = default;
        if (!Moving())
        {
            HexGeometry.PixelToHex(pixelX, pixelY, out X, out Y);
            if (Owner.HexX != X || Owner.HexY != Y)
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // UnitHS.PAS:560
    public override void Select()
    {
        Status = (byte)(Status | 4);
    }

    // UnitHS.PAS:562
    public override void Unselect()
    {
        Status = (byte)(Status & 251);
        base.Unselect();
    }

    // UnitHS.PAS:627
    public virtual void CalculateShowPriority()
    {
        MapField field;
        HexSprite hexSprite;
        int X = default;
        int Y = default;
        field = Owner;
        if (Moving() && Set<byte>.WithBounds(0, 255).WithRange(3, 5).Contains(Direction))
        {
            HexGeometry.CenterHexNumberToHex(Owner.HexX, Owner.HexY, Direction, ref X, ref Y);
            field = Owner.Owner.GetField(X, Y);
            HexGeometry.HexToPixel(X, Y, out X, out Y);
        }
        else
        {
            HexGeometry.HexToPixel(field.HexX, field.HexY, out X, out Y);
        }

        hexSprite = field.GetMaxLevelNoneTransparentHexSprite();
        _showPriority = 1744830464 + ((Y + 34 + 6) << 13) + (X + 16);
        if (hexSprite != this && hexSprite != null && hexSprite.GetShowPriority() >= _showPriority)
        {
            _showPriority = hexSprite.GetShowPriority() + 2;
        }

        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out X, out Y);
        SortMapField(Owner.Owner.Map.MapContainer, X - 18, Y - 32, Owner.HexLevel, 64, 64);
    }

    // UnitHS.PAS:766
    public virtual bool MoveTo(MapField field, int moveCost)
    {
        if (Owner != null)
        {
            RemoveFromMap();
        }

        SetOwnerWithoutNotify(field);
        CalculateShowPriority();
        PlaceOnMap();
        field.RemoveChild(this);
        SetOwnerWithoutNotify(null);
        SetOwner(field);
        return true;
    }

    // UnitHS.PAS:793
    public virtual bool PlaceHex(MapContainer map, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapField field;
        bool success;
        field = map.GetField(hexX, hexY, hexLevel);
        SetOwnerWithoutNotify(field);
        CalculateShowPriority();
        success = PlaceOnMap();
        field.RemoveChild(this);
        SetOwnerWithoutNotify(null);
        SetOwner(field);
        return success;
    }

    // UnitHS.PAS:659
    public virtual void UpdateFlagId()
    {
        FlagId = 32;
    }

    // UnitHS.PAS:910
    public virtual sbyte MoveFirst(int direction, bool instant)
    {
        if (MoveStep != -1)
        {
            CancelMove();
        }

        Status = (byte)(Status & 231);
        if (instant)
        {
            Status = (byte)(Status | 16);
        }

        Direction = (byte)direction;
        MoveStep = 0;
        CalculateShowPriority();
        return 0;
    }

    // UnitHS.PAS:974
    public virtual int GetMoveSteps()
    {
        int result;
        switch (UnitGraphicsResource.MoveAnimationSpeed)
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
                result = 8;
                break;
        }

        return result;
    }

    // UnitHS.PAS:176
    public void CreateShadow()
    {
        if (_shadow != null)
        {
            DestroyShadow();
        }

        if (StrategicMap.Current.ShowUnitShadows)
        {
            _shadow = new UnitShadowHexSprite(null) { Holder = CreationHolder() };
            _shadow.UnitHexSprite = this;
        }
    }

    // UnitHS.PAS:185
    public void DestroyShadow()
    {
        if (_shadow != null)
        {
            if (GetVisible())
            {
                Owner.Owner.Map.DynamicVisibleHexSprite.RemoveChild(_shadow);
            }

            _shadow?.Dispose();
            _shadow = null;
        }
    }

    // UnitHS.PAS:584
    public int GetUnitGraphicsResourceIndex()
    {
        if (UnitGraphicsResource != null)
        {
            return UnitGraphicsResource.GetIndex();
        }

        return -1;
    }

    // UnitHS.PAS:594
    public void SetUnitGraphicsResourceIndex(int value)
    {
        UnitGraphicsResource resource;
        UnitGraphicsResource old;
        // { This eliminated copy preserves the original saved registers. }
        resource = UnitGraphicsResource;
        if (resource == null || value != resource.GetIndex())
        {
            old = UnitGraphicsResource;
            if (old != null)
            {
                old.Release();
            }

            resource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(value);
            UnitGraphicsResource = resource;
            if (resource != null)
            {
                resource.AddRef();
            }
        }
    }

    // UnitHS.PAS:902
    public void SetDirection(byte value)
    {
        if (value != Direction)
        {
            if (Moving())
            {
                CancelMove();
            }

            Direction = value;
        }
    }

    // UnitHS.PAS:920
    public sbyte GetMoveStatus()
    {
        sbyte result;
        if (MoveStep >= 0)
        {
            var moveSteps = GetMoveSteps();
            if (MoveStep < moveSteps)
            {
                var moveSteps2 = GetMoveSteps();
                result = (sbyte)Round((double)(MoveStep * 100) / moveSteps2);
            }
            else
            {
                result = 100;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // UnitHS.PAS:929
    public void SetMoveStatus(sbyte value)
    {
        if (value >= 0)
        {
            MoveStep = (sbyte)((GetMoveSteps() * value + 50) / 100);
            var moveSteps = GetMoveSteps();
            if (MoveStep > moveSteps)
            {
                MoveStep = (sbyte)GetMoveSteps();
                return;
            }
        }
        else
        {
            MoveStep = -1;
        }
    }

    // UnitHS.PAS:940
    public sbyte MoveNext()
    {
        sbyte result;
        if (MoveStep >= 0)
        {
            Status = (byte)(Status & 247);
            var moveSteps = GetMoveSteps();
            if (MoveStep < moveSteps)
            {
                MoveStep = (sbyte)(MoveStep + 1);
                var moveSteps2 = GetMoveSteps();
                result = (sbyte)Round((double)(MoveStep * 100) / moveSteps2);
            }
            else
            {
                result = 100;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // UnitHS.PAS:956
    public bool Moving() => MoveStep != -1;

    // UnitHS.PAS:960
    public void PauseMove()
    {
        Status = (byte)(Status | 8);
    }

    // UnitHS.PAS:964
    public void CancelMove()
    {
        if (MoveStep != -1)
        {
            MoveStep = -1;
            Status = (byte)(Status & 231);
        }
    }

    // intf_P2_03.inc:74

}
