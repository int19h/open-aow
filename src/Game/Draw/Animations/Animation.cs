#nullable disable
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
public partial class Animation(CustomNode owner) : HexSprite(owner)
{

    // Animation.pas:143
    public new const int ClassId = 131815;

    // The stored hex. GetHexX, GetHexY and GetHexLevel read it, and HexSprite.Location returns that.
    public override MapCoordinate Location { get; set; }

    public byte AnimationLevel { get; set; } = 26;

    private Set<StateElement> _state;

    public int FrameCounter { get; set; }

    public Rect Dimensions { get; internal set; }

    public event ShowAnimationEvent OnShow;

    internal void SetShowHandler(ShowAnimationEvent handler) => OnShow = handler;

    public event NewFrameAnimationEvent OnNewFrame;

    public int Tag { get; set; }
    public GameMap AnimationMap { get; set; }

    // Animation.pas:153
    public override Set<ControlStyleElement> GetControlStyle()
    {
        Set<ControlStyleElement> result = AnimationControlMode().Contains(AnimationControlModeElement.Saveable)
            ? Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad)
            : Set<ControlStyleElement>.Of();
        return result;
    }

    // Animation.pas:276
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Dimensions = PersistSupport.Record<RectImage, Rect>(persister, name: "Dimensions", value: Dimensions);
        Location = PersistSupport.Coordinate(persister, name: "Location", value: Location, width: CoordinateWidth.Byte);
        FrameCounter = persister.Int32("FrameCounter", value: FrameCounter);
    }

    // Animation.pas:127

    // Animation.pas:194
    public override byte GetLevel() => AnimationLevel;

    // Animation.pas:207
    public override void SetVisible(bool value) => _state = value
        ? _state + Set<StateElement>.Of(StateElement.Visible)
        : _state - Set<StateElement>.Of(StateElement.Visible);

    // Animation.pas:213
    public override bool GetVisible() => _state.Contains(StateElement.Visible);

    // Animation.pas:250
    public override bool GetSelected() => _state.Contains(StateElement.Selected);

    // Animation.pas:261
    public override sbyte GetHexX() => Location.X;

    // Animation.pas:266
    public override sbyte GetHexY() => Location.Y;

    // Animation.pas:271
    public override sbyte GetHexLevel() => Location.MapLevel;

    // Animation.pas:227
    public override bool GetEditMode() => _state.Contains(StateElement.EditMode);

    // Animation.pas:232
    public override void SetEditMode(bool value) => _state = value
        ? _state + Set<StateElement>.Of(StateElement.EditMode)
        : _state - Set<StateElement>.Of(StateElement.EditMode);

    // Animation.pas:199
    public override int GetShowPriority()
    {
        HexGeometry.HexToPixel(Location.X, Location.Y, out int x, out int y);
        return (AnimationLevel << 26) + ((y + Dimensions.Bottom) << 13) + x;
    }

    // Animation.pas:315
    public override void Connect()
    {
        if (!_state.Contains(StateElement.Placed))
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out int x, out int y);
            _ = AnimationControlMode().Contains(AnimationControlModeElement.OwnedByMap)
                ? PlaceOnMap(AnimationMap.MapContainer, x + Dimensions.Left, y + Dimensions.Top, Location.MapLevel, x + Dimensions.Right, y + Dimensions.Bottom)
                : PlaceUserOnMap(AnimationMap.MapContainer, x + Dimensions.Left, y + Dimensions.Top, Location.MapLevel, x + Dimensions.Right, y + Dimensions.Bottom);

            _state += Set<StateElement>.Of(StateElement.Placed);
        }
    }

    // Animation.pas:332
    public override void Disconnect()
    {
        if (_state.Contains(StateElement.Placed))
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out int x, out int y);
            if (AnimationControlMode().Contains(AnimationControlModeElement.OwnedByMap))
            {
                RemoveFromMap(x + Dimensions.Left, y + Dimensions.Top, x + Dimensions.Right, y + Dimensions.Bottom);
            }
            else
            {
                RemoveUserFromMap(AnimationMap.MapContainer, x + Dimensions.Left, y + Dimensions.Top, Location.MapLevel, x + Dimensions.Right, y + Dimensions.Bottom);
            }

            _state -= Set<StateElement>.Of(StateElement.Placed);
        }
    }

    // Animation.pas:383
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => OnShow?.Invoke(this, pixelXSurface, pixelYSurface, lockedSurface);

    // Animation.pas:349
    public override bool Place(MapContainer map, int pixelX, int pixelY, int hexLevel)
    {
        HexGeometry.PixelToHex(pixelX, pixelY, out int x, out int y);
        return PlaceHex(map, (sbyte)x, (sbyte)y, (sbyte)hexLevel);
    }

    // Animation.pas:372
    public override void Remove()
    {
        base.Remove();
        if (GetVisible())
        {
            _ = AnimationLevel <= 15
                ? AnimationMap.StaticVisibleHexSprite.RemoveChild(this)
                : AnimationMap.DynamicVisibleHexSprite.RemoveChild(this);

            SetVisible(false);
        }
    }

    // Animation.pas:164
    public override void Activate()
    {
        base.Activate();
        _state += Set<StateElement>.Of(StateElement.Activated);
        if (AnimationControlMode().Contains(AnimationControlModeElement.LinkToNewFrame))
        {
            AnimationMap.NewFrameEvents += NewFrameEvent;
        }

        if (AnimationControlMode().Contains(AnimationControlModeElement.FixedVisible))
        {
            _ = AnimationMap.DynamicFixedVisibleHexSprite.AddChild(this);
        }

        if (AnimationControlMode().Contains(AnimationControlModeElement.LinkToOnDestroy))
        {
            AnimationMap.DestroyEvents += DestroyEvent;
        }
    }

    // Animation.pas:176
    public override void Deactivate()
    {
        base.Deactivate();
        if (_state.Contains(StateElement.Activated))
        {
            _state -= Set<StateElement>.Of(StateElement.Activated);
            if (AnimationControlMode().Contains(AnimationControlModeElement.FixedVisible))
            {
                _ = AnimationMap.DynamicFixedVisibleHexSprite.RemoveChild(this);
                _ = AnimationMap.DynamicVisibleHexSprite.RemoveChild(this);
            }

            if (AnimationControlMode().Contains(AnimationControlModeElement.LinkToNewFrame))
            {
                AnimationMap.NewFrameEvents -= NewFrameEvent;
            }

            if (AnimationControlMode().Contains(AnimationControlModeElement.LinkToOnDestroy))
            {
                AnimationMap.DestroyEvents -= DestroyEvent;
            }
        }
    }

    // Animation.pas:218
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!_state.Contains(StateElement.Visible))
        {
            _state += Set<StateElement>.Of(StateElement.Visible);
            _ = visibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // Animation.pas:238
    public override void Select()
    {
        base.Select();
        _state += Set<StateElement>.Of(StateElement.Selected);
    }

    // Animation.pas:244
    public override void Unselect()
    {
        base.Unselect();
        _state -= Set<StateElement>.Of(StateElement.Selected);
    }

    // Animation.pas:286
    public virtual bool PlaceTokenEvent(AnimationTokenEvent eventValue, TokenExecutor executor) => PlaceHex(AnimationMap.MapContainer, Location.X, Location.Y, Location.MapLevel);

    // Animation.pas:291
    public virtual bool RemoveTokenEvent(AnimationTokenEvent eventValue, TokenExecutor executor)
    {
        MainRemove();
        return true;
    }

    // Animation.pas:297
    public virtual bool ExecuteTokenEvent(AnimationTokenEvent eventValue, TokenExecutor executor) => false;

    // Animation.pas:308
    public virtual bool CanPlace(MapContainer map, sbyte X, sbyte Y, sbyte L)
    {
        bool result = X >= 0 && Y >= 0 && L >= 0 && X < map.Width && Y < map.Height && L < map.LevelCount;
        return result;
    }

    // Animation.pas:357
    public virtual bool PlaceHex(MapContainer map, sbyte X, sbyte Y, sbyte L)
    {
        bool result;
        if (GetEditMode() || CanPlace(map, X, Y, L))
        {
            AnimationMap = map.Map;
            Remove();
            Location = new MapCoordinate(X, Y, L);
            Connect();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Animation.pas:302
    public virtual void NewFrame()
    {
        FrameCounter += 1;
        OnNewFrame?.Invoke(this, FrameCounter);
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
        Dimensions = !_state.Contains(StateElement.Placed)
            ? R
            : throw new InvalidOperationException("Cannot change dimensions when object is places");
    }

    // Animation.pas:43

    // Animation.pas:159
    public virtual Set<AnimationControlModeElement> AnimationControlMode() => Set<AnimationControlModeElement>.Of(AnimationControlModeElement.LinkToNewFrame, AnimationControlModeElement.FixedVisible, AnimationControlModeElement.LinkToOnDestroy);

    public enum StateElement : byte
    {
        Visible = 0, EditMode = 1, Selected = 2, LinkedToTokenEvent = 3, Activated = 4, Placed = 5
    }

    public delegate void NewFrameAnimationEvent(Animation Animation, int FrameCounter);

}
