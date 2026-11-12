// Translated from Animation.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Commands;

using Aow.Game.Net;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

// { ot=1 }
// hex position, display level, state, frame, dimensions, callbacks, tag, map
public partial class Animation : HexSprite
{
    public Animation(ECustomNode owner) : base(owner)
    {
        AnimationLevel = 26;
    }

    // Animation.pas:143
    public new const int ClassId = 131815;

    public sbyte _hexX;
    public sbyte _hexY;
    public sbyte _hexLevel;

    public byte AnimationLevel { get; set; }

    private Set<StateElement> _state;
    public int FrameCounter;
    public Rect Dimensions;

    public ShowAnimationEvent OnShow { get; set; }
    public NewFrameAnimationEvent OnNewFrame { get; set; }
    public int Tag { get; set; }
    public HSMap AnimationMap { get; set; }

    // Animation.pas:153
    public override Set<ControlStyleElement> GetControlStyle()
    {
        Set<ControlStyleElement> result;
        if (AnimationControlMode().Contains(AnimationControlModeElement.Saveable))
        {
            result = Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad);
        }
        else
        {
            result = Set<ControlStyleElement>.Of();
        }

        return result;
    }

    // Animation.pas:276
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwData(Bytes.Of(ref Dimensions, 16), 16, 10);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _hexX), 11);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _hexY), 12);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _hexLevel), 13);
        stream.rwInteger(ref FrameCounter, 14);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Record(persister, "Dimensions", ref Dimensions);
        PersistSupport.UnsignedByte(persister, "HexX", ref _hexX);
        PersistSupport.UnsignedByte(persister, "HexY", ref _hexY);
        PersistSupport.UnsignedByte(persister, "HexLevel", ref _hexLevel);
        persister.Int32("FrameCounter", ref FrameCounter);
    }

    // Animation.pas:127

    // Animation.pas:194
    public override byte GetLevel() => AnimationLevel;

    // Animation.pas:207
    public override void SetVisible(bool value)
    {
        if (value)
        {
            _state = _state + Set<StateElement>.Of(StateElement.Visible);
        }
        else
        {
            _state = _state - Set<StateElement>.Of(StateElement.Visible);
        }
    }

    // Animation.pas:213
    public override bool GetVisible() => _state.Contains(StateElement.Visible);

    // Animation.pas:250
    public override bool GetSelected() => _state.Contains(StateElement.Selected);

    // Animation.pas:261
    public override sbyte GetHexX() => _hexX;

    // Animation.pas:266
    public override sbyte GetHexY() => _hexY;

    // Animation.pas:271
    public override sbyte GetHexLevel() => _hexLevel;

    // Animation.pas:227
    public override bool GetEditMode() => _state.Contains(StateElement.EditMode);

    // Animation.pas:232
    public override void SetEditMode(bool value)
    {
        if (value)
        {
            _state = _state + Set<StateElement>.Of(StateElement.EditMode);
        }
        else
        {
            _state = _state - Set<StateElement>.Of(StateElement.EditMode);
        }
    }

    // Animation.pas:199
    public override int GetShowPriority()
    {
        int X = default;
        int Y = default;
        HexGeometry.HexToPixel(_hexX, _hexY, ref X, ref Y);
        return (AnimationLevel << 26) + ((Y + Dimensions.Bottom) << 13) + X;
    }

    // Animation.pas:315
    public override void Connect()
    {
        int X = default;
        int Y = default;
        if (!_state.Contains(StateElement.Placed))
        {
            HexGeometry.HexToPixel(_hexX, _hexY, ref X, ref Y);
            if (AnimationControlMode().Contains(AnimationControlModeElement.OwnedByMap))
            {
                PlaceOnMap(AnimationMap.MapContainer, X + Dimensions.Left, Y + Dimensions.Top, _hexLevel, X + Dimensions.Right, Y + Dimensions.Bottom);
            }
            else
            {
                PlaceUserOnMap(AnimationMap.MapContainer, X + Dimensions.Left, Y + Dimensions.Top, _hexLevel, X + Dimensions.Right, Y + Dimensions.Bottom);
            }

            _state = _state + Set<StateElement>.Of(StateElement.Placed);
        }
    }

    // Animation.pas:332
    public override void Disconnect()
    {
        int X = default;
        int Y = default;
        if (_state.Contains(StateElement.Placed))
        {
            HexGeometry.HexToPixel(_hexX, _hexY, ref X, ref Y);
            if (AnimationControlMode().Contains(AnimationControlModeElement.OwnedByMap))
            {
                RemoveFromMap(X + Dimensions.Left, Y + Dimensions.Top, X + Dimensions.Right, Y + Dimensions.Bottom);
            }
            else
            {
                RemoveUserFromMap(AnimationMap.MapContainer, X + Dimensions.Left, Y + Dimensions.Top, _hexLevel, X + Dimensions.Right, Y + Dimensions.Bottom);
            }

            _state = _state - Set<StateElement>.Of(StateElement.Placed);
        }
    }

    // Animation.pas:383
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (OnShow != null)
        {
            OnShow(this, pixelXSurface, pixelYSurface, lockedSurface);
        }
    }

    // Animation.pas:349
    public override bool Place(MapContainer map, int pixelX, int pixelY, int hexLevel)
    {
        int X = default;
        int Y = default;
        HexGeometry.PixelToHex(pixelX, pixelY, ref X, ref Y);
        return PlaceHex(map, (sbyte)X, (sbyte)Y, (sbyte)hexLevel);
    }

    // Animation.pas:372
    public override void Remove()
    {
        base.Remove();
        if (GetVisible())
        {
            if (AnimationLevel <= 15)
            {
                AnimationMap.StaticVisibleHexSprite.RemoveChild(this);
            }
            else
            {
                AnimationMap.DynamicVisibleHexSprite.RemoveChild(this);
            }

            SetVisible(false);
        }
    }

    // Animation.pas:164
    public override void Activate()
    {
        base.Activate();
        _state = _state + Set<StateElement>.Of(StateElement.Activated);
        if (AnimationControlMode().Contains(AnimationControlModeElement.LinkToNewFrame))
        {
            AnimationMap.NewFrameEvents += NewFrameEvent;
        }

        if (AnimationControlMode().Contains(AnimationControlModeElement.FixedVisible))
        {
            AnimationMap.DynamicFixedVisibleHexSprite.AddChild(this);
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
            _state = _state - Set<StateElement>.Of(StateElement.Activated);
            if (AnimationControlMode().Contains(AnimationControlModeElement.FixedVisible))
            {
                AnimationMap.DynamicFixedVisibleHexSprite.RemoveChild(this);
                AnimationMap.DynamicVisibleHexSprite.RemoveChild(this);
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
            _state = _state + Set<StateElement>.Of(StateElement.Visible);
            visibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // Animation.pas:238
    public override void Select()
    {
        base.Select();
        _state = _state + Set<StateElement>.Of(StateElement.Selected);
    }

    // Animation.pas:244
    public override void Unselect()
    {
        base.Unselect();
        _state = _state - Set<StateElement>.Of(StateElement.Selected);
    }

    // Animation.pas:286
    public virtual bool PlaceTokenEvent(AnimationTokenEvent eventValue, TokenExecutor executor) => PlaceHex(AnimationMap.MapContainer, _hexX, _hexY, _hexLevel);

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
        bool result;
        if (X < 0 || Y < 0 || L < 0 || X >= map.Width || Y >= map.Height || L >= map.LevelCount)
        {
            result = false;
        }
        else
        {
            result = true;
        }

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
            _hexX = X;
            _hexY = Y;
            _hexLevel = L;
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
        if (OnNewFrame != null)
        {
            OnNewFrame(this, FrameCounter);
        }
    }

    // Animation.pas:138
    public void DestroyEvent(object sender)
    {
        if (Owner == null)
        {
            Dispose();
        }
    }

    // Animation.pas:148
    public void NewFrameEvent(object sender)
    {
        NewFrame();
    }

    // Animation.pas:255
    public void SetDimensions(Rect R)
    {
        if (!_state.Contains(StateElement.Placed))
        {
            Dimensions = R;
        }
        else
        {
            throw new InvalidOperationException("Cannot change dimensions when object is places");
        }
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
