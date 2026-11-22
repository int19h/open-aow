#nullable disable warnings
// Translated from Animation.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Commands;

using Aow.Game.Net;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

// { ot=1 }
// hex position, display level, state, frame, dimensions, callbacks, tag, map
public class Animation(CustomNode? owner, ShowAnimationEvent? show = null, Action<Animation, int>? newFrame = null) : HexSprite(owner)
{
    public Animation(CustomNode? owner) : this(owner, show: null)
    {
    }

    // Animation.pas:143
    public new const int ClassId = 131_815;

    // The stored hex. GetHexX, GetHexY and GetHexLevel read it, and HexSprite.Location returns that.
    public override MapCoordinate Location { get; set; }

    public int AnimationLevel { get; set; } = 26;

    private States _state;

    public int FrameCounter { get; set; }

    public Rect Dimensions { get; internal set; }

    private readonly ShowAnimationEvent _show = show;

    private readonly Action<Animation, int> _newFrame = newFrame;

    public int Tag { get; set; }
    public GameMap? AnimationMap { get; set; }

    // Animation.pas:153
    public override ControlStyles GetControlStyle()
    {
        return AnimationControlMode().Contains(AnimationControlModes.Saveable)
            ? ControlStyles.SaveLoad
            : ControlStyles.None;
    }

    // Animation.pas:276
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Dimensions = persister.Record<RectImage, Rect>(name: "Dimensions", value: Dimensions);
        Location = persister.Coordinate(name: "Location", value: Location, width: CoordinateWidth.Byte);
        FrameCounter = persister.Int32("FrameCounter", value: FrameCounter);
    }

    // Animation.pas:194
    public override int Level => AnimationLevel;

    // Animation.pas:213
    public override bool Visible
    {
        get => _state.Contains(States.Visible);
        // Animation.pas:207
        set => _state = value
        ? _state | States.Visible
        : _state & ~States.Visible;
    }

    // Animation.pas:250
    public override bool GetSelected() => _state.Contains(States.Selected);

    // Animation.pas:261
    public override sbyte HexX => Location.X;

    // Animation.pas:266
    public override sbyte HexY => Location.Y;

    // Animation.pas:271
    public override sbyte HexLevel => Location.MapLevel;

    // Animation.pas:227
    public override bool EditMode
    {
        get => _state.Contains(States.EditMode);
        // Animation.pas:232
        set => _state = value
        ? _state | States.EditMode
        : _state & ~States.EditMode;
    }

    // Animation.pas:199
    public override int GetShowPriority()
    {
        HexGeometry.HexToPixel(Location.X, Location.Y, out int x, out int y);
        return (AnimationLevel << 26) + ((y + Dimensions.Bottom) << 13) + x;
    }

    // Animation.pas:315
    public override void Connect()
    {
        if (!_state.Contains(States.Placed))
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out int x, out int y);
            _ = AnimationControlMode().Contains(AnimationControlModes.OwnedByMap)
                ? PlaceOnMap(AnimationMap.MapContainer, x + Dimensions.Left, y + Dimensions.Top, Location.MapLevel, x + Dimensions.Right, y + Dimensions.Bottom)
                : PlaceUserOnMap(AnimationMap.MapContainer, x + Dimensions.Left, y + Dimensions.Top, Location.MapLevel, x + Dimensions.Right, y + Dimensions.Bottom);

            _state |= States.Placed;
        }
    }

    // Animation.pas:332
    public override void Disconnect()
    {
        if (_state.Contains(States.Placed))
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out int x, out int y);
            if (AnimationControlMode().Contains(AnimationControlModes.OwnedByMap))
            {
                RemoveFromMap(x + Dimensions.Left, y + Dimensions.Top, x + Dimensions.Right, y + Dimensions.Bottom);
            }
            else
            {
                RemoveUserFromMap(AnimationMap.MapContainer, x + Dimensions.Left, y + Dimensions.Top, Location.MapLevel, x + Dimensions.Right, y + Dimensions.Bottom);
            }

            _state &= ~States.Placed;
        }
    }

    // Animation.pas:383
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => _show?.Invoke(this, pixelXSurface, pixelYSurface, SurfaceDesc: lockedSurface);

    // Animation.pas:349
    public override bool Place(MapContainer? map, int pixelX, int pixelY, int hexLevel)
    {
        HexGeometry.PixelToHex(pixelX, vertical: pixelY, out int x, out int y);
        return PlaceHex(map, x.TruncateToSByte(), y.TruncateToSByte(), L: hexLevel.TruncateToSByte());
    }

    // Animation.pas:372
    public override void Remove()
    {
        base.Remove();
        if (Visible)
        {
            _ = AnimationLevel <= 15
                ? AnimationMap.StaticVisibleHexSprite.RemoveChild(this)
                : AnimationMap.DynamicVisibleHexSprite.RemoveChild(this);

            Visible = false;
        }
    }

    // Animation.pas:164
    public override void Activate()
    {
        base.Activate();
        _state |= States.Activated;
        if (AnimationControlMode().Contains(AnimationControlModes.LinkToNewFrame))
        {
            AnimationMap.FrameStarted += NewFrameEvent;
        }

        if (AnimationControlMode().Contains(AnimationControlModes.FixedVisible))
        {
            _ = AnimationMap.DynamicFixedVisibleHexSprite.AddChild(this);
        }

        if (AnimationControlMode().Contains(AnimationControlModes.LinkToOnDestroy))
        {
            AnimationMap.MapDestroying += DestroyEvent;
        }
    }

    // Animation.pas:176
    public override void Deactivate()
    {
        base.Deactivate();
        if (_state.Contains(States.Activated))
        {
            _state &= ~States.Activated;
            if (AnimationControlMode().Contains(AnimationControlModes.FixedVisible))
            {
                _ = AnimationMap.DynamicFixedVisibleHexSprite.RemoveChild(this);
                _ = AnimationMap.DynamicVisibleHexSprite.RemoveChild(this);
            }

            if (AnimationControlMode().Contains(AnimationControlModes.LinkToNewFrame))
            {
                AnimationMap.FrameStarted -= NewFrameEvent;
            }

            if (AnimationControlMode().Contains(AnimationControlModes.LinkToOnDestroy))
            {
                AnimationMap.MapDestroying -= DestroyEvent;
            }
        }
    }

    // Animation.pas:218
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!_state.Contains(States.Visible))
        {
            _state |= States.Visible;
            _ = visibilityList.AddSortChild(this, sortValue: GetShowPriority());
        }
    }

    // Animation.pas:238
    public override void Select()
    {
        base.Select();
        _state |= States.Selected;
    }

    // Animation.pas:244
    public override void Unselect()
    {
        base.Unselect();
        _state &= ~States.Selected;
    }

    // Animation.pas:286
    public virtual bool PlaceTokenEvent(AnimationTokenEvent eventValue, TokenExecutor executor) => PlaceHex(AnimationMap.MapContainer, Location.X, Location.Y, L: Location.MapLevel);

    // Animation.pas:291
    public virtual bool RemoveTokenEvent(AnimationTokenEvent eventValue, TokenExecutor executor)
    {
        MainRemove();
        return true;
    }

    // Animation.pas:297
    public virtual bool ExecuteTokenEvent(AnimationTokenEvent eventValue, TokenExecutor executor) => false;

    // Animation.pas:308
    public virtual bool CanPlace(MapContainer? map, int X, int Y, int L) => X >= 0 && Y >= 0 && L >= 0 && X < map.Width && Y < map.Height && L < map.LevelCount;

    // Animation.pas:357
    public virtual bool PlaceHex(MapContainer? map, int X, int Y, int L)
    {
        if (EditMode || CanPlace(map, X, Y, L))
        {
            AnimationMap = map.Map;
            Remove();
            Location = new MapCoordinate((sbyte)X, (sbyte)Y, (sbyte)L);
            Connect();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Animation.pas:302
    public virtual void NewFrame()
    {
        FrameCounter += 1;
        _newFrame?.Invoke(this, FrameCounter);
    }

    // Animation.pas:138
    public void DestroyEvent(object sender)
    {
        if (Owner is null)
        {
            Dispose();
        }
    }

    // Animation.pas:148
    public void NewFrameEvent(object sender) => NewFrame();

    // Animation.pas:255
    public void SetDimensions(Rect R)
    {
        Dimensions = !_state.Contains(States.Placed)
            ? R
            : throw new InvalidOperationException("Cannot change dimensions when object is places");
    }

    // Animation.pas:159
    public virtual AnimationControlModes AnimationControlMode() => AnimationControlModes.LinkToNewFrame | AnimationControlModes.FixedVisible | AnimationControlModes.LinkToOnDestroy;

    [Flags]
    public enum States : byte
    {
        None = 0,
        Visible = 1 << 0,
        EditMode = 1 << 1,
        Selected = 1 << 2,
        LinkedToTokenEvent = 1 << 3,
        Activated = 1 << 4,
        Placed = 1 << 5,
    }
}
