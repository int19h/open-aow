#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Draw;

// Status. _showPriority. UnitGraphicsResource. Direction. MoveStep. FlagId. Shadow
public partial class UnitHexSprite(CustomNode? owner) : HexSprite(owner)
{

    public new const int ClassId = 131_604;

    public int Status { get; private set; }

    // visible, edit, selected, paused, instant move
    private int _showPriority = 1_744_830_464;

    public UnitGraphicsResource? UnitGraphicsResource { get; set; }

    public int Direction { get; set; } = 3;

    public int MoveStep { get; set; } = -1;
    // -1 when idle
    public byte FlagId { get; protected set; }

    private UnitShadowHexSprite? _shadow;

    // UnitHS.PAS:168
    public override void Dispose()
    {
        DestroyShadow();
        SetUnitGraphicsResourceIndex(-1);
        base.Dispose();
    }

    // UnitHS.PAS:219
    public override void SetOwner(CustomNode? owner)
    {
        if (owner != Owner)
        {
            _ = (Owner?.RemoveChild(this));

            SetOwnerWithoutNotify(owner);
            _ = (Owner?.AddChild(this));
        }
    }

    // UnitHS.PAS:208
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Direction = persister.Byte("Direction", value: Direction.TruncateToByte());
        if (Direction == 0)
        {
            Direction = 3;
        }
    }

    // UnitHS.PAS:655
    public override int Level => 26;

    // UnitHS.PAS:574
    public override bool Visible
    {
        get => (Status & 1) != 0;
        // UnitHS.PAS:569
        set => Status = value ? Status | 1 : Status & 0b1111_1110;
    }

    // UnitHS.PAS:567
    public override bool GetSelected() => (Status & 0b0100) != 0;

    // UnitHS.PAS:228
    public override sbyte HexX => (sbyte)Owner.HexX;

    // UnitHS.PAS:230
    public override sbyte HexY => (sbyte)Owner.HexY;

    // UnitHS.PAS:232
    public override sbyte HexLevel
    {
        get
        {
            Assert(Owner is not null, message: "Onwer isn't defined", file: "UnitHS.PAS", line: 557);
            return (sbyte)Owner.HexLevel;
        }
    }

    // UnitHS.PAS:576
    public override bool EditMode
    {
        get => (Status & 0b0010) != 0;
        // UnitHS.PAS:578
        set => Status = value ? Status | 0b0010 : Status & 0b1111_1101;
    }

    // UnitHS.PAS:651
    public override int GetShowPriority() => _showPriority;

    // UnitHS.PAS:692
    public override void Connect()
    {
        CalculateShowPriority();
        MapField? field = Owner;
        _ = field.RemoveChild(this);
        MapLevel level = field.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, field.HexX, field.HexY, radius: 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.X < level.Width && walk.Y >= 0 && walk.Y < level.Height)
            {
                _ = level.GetField(walk.X, walk.Y).AddHexSprite(this, walk.HexNumber);
                level.GetField(walk.X, walk.Y).UpdateMapField();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // UnitHS.PAS:716
    public override void Disconnect()
    {
        if (Owner is not null)
        {
            MapField field = Owner;
            MapLevel level = field.Owner;
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, field.HexX, field.HexY, radius: 0);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.X < level.Width && walk.Y >= 0 && walk.Y < level.Height)
                {
                    _ = level.GetField(walk.X, walk.Y).RemoveHexSprite(this);
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
        Owner.Owner.Map.ScannerIconsChangedRadius(Owner.HexX, Owner.HexY, Owner.HexLevel, radius: 0);
        return true;
    }

    // UnitHS.PAS:751
    public override void RemoveFromMap()
    {
        if (Owner is not null)
        {
            MapField field = Owner;
            if (Moving())
            {
                CancelMove();
            }

            Disconnect();
            field.UpdateMapField();
            field.Owner.Map.ScannerIconsChangedRadius(field.HexX, field.HexY, field.HexLevel, radius: 0);
        }
    }

    // UnitHS.PAS:809
    public override bool Place(MapContainer? map, int pixelX, int pixelY, int hexLevel)
    {
        HexGeometry.PixelToHex(pixelX, vertical: pixelY, out int x, out int y);
        return PlaceHex(map, x.TruncateToSByte(), y.TruncateToSByte(), hexLevel.TruncateToSByte());
    }

    // UnitHS.PAS:779
    public override void Remove()
    {
        MapField? field = Owner;
        RemoveFromMap();
        if (Visible)
        {
            _ = field.Owner.Map.DynamicVisibleHexSprite.RemoveChild(this);
            Visible = false;
        }
    }

    // UnitHS.PAS:986
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 16 && message.HexNumber == 0 && StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            Dispose();
        }
    }

    // UnitHS.PAS:678
    public override int CanMoveOver(in HexSpriteSenderInfo info) => info.HexNumber == 0 ? 0 : 1;

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
            Status |= 1;
            _ = visibilityList.AddSortChild(this, sortValue: _showPriority);
            // { Read Shadow directly. }
            if (_shadow is not null)
            {
                _ = visibilityList.AddSortChild(sortValue: _shadow.GetShowPriority(), child: _shadow);
            }
        }
    }

    // UnitHS.PAS:613
    public override bool CanSelect(int pixelX, int pixelY)
    {
        if (!Moving())
        {
            HexGeometry.PixelToHex(pixelX, vertical: pixelY, out int x, out int y);
            return Owner.HexX == x && Owner.HexY == y;
        }
        else
        {
            return false;
        }
    }

    // UnitHS.PAS:560
    public override void Select() => Status |= 0b0100;

    // UnitHS.PAS:562
    public override void Unselect()
    {
        Status &= 0b1111_1011;
        base.Unselect();
    }

    // UnitHS.PAS:627
    public virtual void CalculateShowPriority()
    {
        int x = default;
        int y = default;
        MapField? field = Owner;
        if (Moving() && Direction is >= 3 and <= 5)
        {
            HexGeometry.CenterHexNumberToHex(Owner.HexX, Owner.HexY, hexNumber: Direction, ref x, ref y);
            field = Owner.Owner.GetField(x, y);
            HexGeometry.HexToPixel(x, y, out x, out y);
        }
        else
        {
            HexGeometry.HexToPixel(field.HexX, field.HexY, out x, out y);
        }

        HexSprite? hexSprite = field.GetMaxLevelNoneTransparentHexSprite();
        _showPriority = 1_744_830_464 + ((y + 34 + 6) << 13) + x + 16;
        if (hexSprite != this && hexSprite is not null && hexSprite.GetShowPriority() >= _showPriority)
        {
            _showPriority = hexSprite.GetShowPriority() + 2;
        }

        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out x, out y);
        SortMapField(Owner.Owner.Map.MapContainer, x - 18, y - 32, Owner.HexLevel, 64, 64);
    }

    // UnitHS.PAS:766
    public virtual bool MoveTo(MapField? field, int moveCost)
    {
        if (Owner is not null)
        {
            RemoveFromMap();
        }

        SetOwnerWithoutNotify(field);
        CalculateShowPriority();
        _ = PlaceOnMap();
        _ = field.RemoveChild(this);
        SetOwnerWithoutNotify(null);
        SetOwner(field);
        return true;
    }

    // UnitHS.PAS:793
    public virtual bool PlaceHex(MapContainer? map, int hexX, int hexY, int hexLevel)
    {
        MapField field = map.GetField(hexX, hexY, hexLevel);
        SetOwnerWithoutNotify(field);
        CalculateShowPriority();
        bool success = PlaceOnMap();
        _ = field.RemoveChild(this);
        SetOwnerWithoutNotify(null);
        SetOwner(field);
        return success;
    }

    // UnitHS.PAS:659
    public virtual void UpdateFlagId() => FlagId = 32;

    // UnitHS.PAS:910
    public virtual int MoveFirst(int direction, bool instant)
    {
        if (MoveStep != -1)
        {
            CancelMove();
        }

        Status &= 0b1110_0111;
        if (instant)
        {
            Status |= 0b0001_0000;
        }

        Direction = direction.TruncateToByte();
        MoveStep = 0;
        CalculateShowPriority();
        return 0;
    }

    // UnitHS.PAS:974
    public virtual int GetMoveSteps()
    {
        return UnitGraphicsResource.MoveAnimationSpeed switch
        {
            0 => 12,
            1 => 10,
            2 => 8,
            3 => 6,
            _ => 8,
        };
    }

    // UnitHS.PAS:176
    public void CreateShadow()
    {
        if (_shadow is not null)
        {
            DestroyShadow();
        }

        if (StrategicMap.Current.ShowUnitShadows)
        {
            _shadow = new UnitShadowHexSprite(null)
            {
                Holder = CreationHolder(),
                UnitHexSprite = this
            };
        }
    }

    // UnitHS.PAS:185
    public void DestroyShadow()
    {
        if (_shadow is not null)
        {
            if (Visible)
            {
                _ = Owner.Owner.Map.DynamicVisibleHexSprite.RemoveChild(_shadow);
            }

            _shadow?.Dispose();
            _shadow = null;
        }
    }

    // UnitHS.PAS:584
    public int UnitGraphicsResourceIndex => UnitGraphicsResource is not null ? UnitGraphicsResource.Index : -1;

    // UnitHS.PAS:594
    public void SetUnitGraphicsResourceIndex(int value)
    {
        // { This eliminated copy preserves the original saved registers. }
        UnitGraphicsResource? resource = UnitGraphicsResource;
        if (resource is null || value != resource.Index)
        {
            UnitGraphicsResource? old = UnitGraphicsResource;
            _ = (old?.Release());

            resource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(value);
            UnitGraphicsResource = resource;
            _ = (resource?.AddRef());
        }
    }

    // UnitHS.PAS:902
    public void SetDirection(int value)
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
    public int GetMoveStatus()
    {
        if (MoveStep >= 0)
        {
            int moveSteps = GetMoveSteps();
            if (MoveStep < moveSteps)
            {
                int moveSteps2 = GetMoveSteps();
                return Round((double)(MoveStep * 100) / moveSteps2).TruncateToSByte();
            }
            else
            {
                return 100;
            }
        }
        else
        {
            return -1;
        }
    }

    // UnitHS.PAS:929
    public void SetMoveStatus(int value)
    {
        if (value >= 0)
        {
            MoveStep = (((GetMoveSteps() * value) + 50) / 100).TruncateToSByte();
            int moveSteps = GetMoveSteps();
            if (MoveStep > moveSteps)
            {
                MoveStep = GetMoveSteps().TruncateToSByte();
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
        if (MoveStep >= 0)
        {
            Status &= 0b1111_0111;
            int moveSteps = GetMoveSteps();
            if (MoveStep < moveSteps)
            {
                MoveStep = (MoveStep + 1).TruncateToSByte();
                int moveSteps2 = GetMoveSteps();
                return (sbyte)Round((double)(MoveStep * 100) / moveSteps2);
            }
            else
            {
                return 100;
            }
        }
        else
        {
            return -1;
        }
    }

    // UnitHS.PAS:956
    public bool Moving() => MoveStep != -1;

    // UnitHS.PAS:960
    public void PauseMove() => Status |= 0b1000;

    // UnitHS.PAS:964
    public void CancelMove()
    {
        if (MoveStep != -1)
        {
            MoveStep = -1;
            Status &= 0b1110_0111;
        }
    }
}
