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
public partial class Animation : HexagonSprite
{
    public Animation(ECustomNode owner) : base(owner)
    {
        FAnimationLevel = 26;
    }

    // Animation.pas:143
    public new const int ClassID = 131815;

    public sbyte FXhx;
    public sbyte FYhx;
    public sbyte FLhx;
    public byte FAnimationLevel;
    public Set<StateElement> FState;
    public int FFrameCounter;
    public Rect FDimensions;
    public ShowAnimationEvent FOnShow;
    public NewFrameAnimationEvent FOnNewFrame;
    public int FTag;
    public HSMap FAnimationMap;

    // Animation.pas:153
    public override Set<EControlStyleElement> GetControlStyle()
    {
        Set<EControlStyleElement> Result;
        if (AnimationControlMode().Contains(AnimationControlModeElement.Saveable))
        {
            Result = Set<EControlStyleElement>.Of(EControlStyleElement.SaveLoad);
        }
        else
        {
            Result = Set<EControlStyleElement>.Of();
        }

        return Result;
    }

    // Animation.pas:276
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FDimensions, 16), 16, 10);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 11);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 12);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 13);
        Stream.rwInteger(ref FFrameCounter, 14);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Record(persister, "Dimensions", ref FDimensions);
        PersistSupport.UnsignedByte(persister, "Xhx", ref FXhx);
        PersistSupport.UnsignedByte(persister, "Yhx", ref FYhx);
        PersistSupport.UnsignedByte(persister, "Lhx", ref FLhx);
        persister.Int("FrameCounter", ref FFrameCounter);
    }

    // Animation.pas:127

    // Animation.pas:194
    public override byte GetLevel() => FAnimationLevel;

    // Animation.pas:207
    public override void SetVisible(bool Value)
    {
        if (Value)
        {
            FState = FState + Set<StateElement>.Of(StateElement.Visible);
        }
        else
        {
            FState = FState - Set<StateElement>.Of(StateElement.Visible);
        }
    }

    // Animation.pas:213
    public override bool GetVisible() => FState.Contains(StateElement.Visible);

    // Animation.pas:250
    public override bool GetSelected() => FState.Contains(StateElement.Selected);

    // Animation.pas:261
    public override sbyte GetXhx() => FXhx;

    // Animation.pas:266
    public override sbyte GetYhx() => FYhx;

    // Animation.pas:271
    public override sbyte GetLhx() => FLhx;

    // Animation.pas:227
    public override bool GetEditMode() => FState.Contains(StateElement.EditMode);

    // Animation.pas:232
    public override void SetEditMode(bool Value)
    {
        if (Value)
        {
            FState = FState + Set<StateElement>.Of(StateElement.EditMode);
        }
        else
        {
            FState = FState - Set<StateElement>.Of(StateElement.EditMode);
        }
    }

    // Animation.pas:199
    public override int GetShowPriority()
    {
        int X = default;
        int Y = default;
        HexGeometry.HXtoHP(FXhx, FYhx, ref X, ref Y);
        return (FAnimationLevel << 26) + ((Y + FDimensions.Bottom) << 13) + X;
    }

    // Animation.pas:315
    public override void Connect()
    {
        int X = default;
        int Y = default;
        if (!FState.Contains(StateElement.Placed))
        {
            HexGeometry.HXtoHP(FXhx, FYhx, ref X, ref Y);
            if (AnimationControlMode().Contains(AnimationControlModeElement.OwnedByMap))
            {
                PlaceOnMapEx(FAnimationMap.MapContainer, X + FDimensions.Left, Y + FDimensions.Top, FLhx, X + FDimensions.Right, Y + FDimensions.Bottom);
            }
            else
            {
                PlaceUserOnMapEx(FAnimationMap.MapContainer, X + FDimensions.Left, Y + FDimensions.Top, FLhx, X + FDimensions.Right, Y + FDimensions.Bottom);
            }

            FState = FState + Set<StateElement>.Of(StateElement.Placed);
        }
    }

    // Animation.pas:332
    public override void Disconnect()
    {
        int X = default;
        int Y = default;
        if (FState.Contains(StateElement.Placed))
        {
            HexGeometry.HXtoHP(FXhx, FYhx, ref X, ref Y);
            if (AnimationControlMode().Contains(AnimationControlModeElement.OwnedByMap))
            {
                RemoveFromMapEx(X + FDimensions.Left, Y + FDimensions.Top, X + FDimensions.Right, Y + FDimensions.Bottom);
            }
            else
            {
                RemoveUserFromMapEx(FAnimationMap.MapContainer, X + FDimensions.Left, Y + FDimensions.Top, FLhx, X + FDimensions.Right, Y + FDimensions.Bottom);
            }

            FState = FState - Set<StateElement>.Of(StateElement.Placed);
        }
    }

    // Animation.pas:383
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        if (FOnShow != null)
        {
            FOnShow(this, XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    // Animation.pas:349
    public override bool Place(MapContainer Map, int Xhp, int Yhp, int Lhx)
    {
        int X = default;
        int Y = default;
        HexGeometry.HPtoHX(Xhp, Yhp, ref X, ref Y);
        return PlaceHX(Map, (sbyte)X, (sbyte)Y, (sbyte)Lhx);
    }

    // Animation.pas:372
    public override void Remove()
    {
        base.Remove();
        if (GetVisible())
        {
            if (FAnimationLevel <= 15)
            {
                FAnimationMap.StaticVisibleHS.RemoveChild(this);
            }
            else
            {
                FAnimationMap.DynamicVisibleHS.RemoveChild(this);
            }

            SetVisible(false);
        }
    }

    // Animation.pas:164
    public override void Activate()
    {
        base.Activate();
        FState = FState + Set<StateElement>.Of(StateElement.Activated);
        if (AnimationControlMode().Contains(AnimationControlModeElement.LinkToNewFrame))
        {
            FAnimationMap.NewFrameEvents += NewFrameEvent;
        }

        if (AnimationControlMode().Contains(AnimationControlModeElement.FixedVisible))
        {
            FAnimationMap.DynamicFixedVisibleHS.AddChild(this);
        }

        if (AnimationControlMode().Contains(AnimationControlModeElement.LinkToOnDestroy))
        {
            FAnimationMap.DestroyEvents += DestroyEvent;
        }
    }

    // Animation.pas:176
    public override void Deactivate()
    {
        base.Deactivate();
        if (FState.Contains(StateElement.Activated))
        {
            FState = FState - Set<StateElement>.Of(StateElement.Activated);
            if (AnimationControlMode().Contains(AnimationControlModeElement.FixedVisible))
            {
                FAnimationMap.DynamicFixedVisibleHS.RemoveChild(this);
                FAnimationMap.DynamicVisibleHS.RemoveChild(this);
            }

            if (AnimationControlMode().Contains(AnimationControlModeElement.LinkToNewFrame))
            {
                FAnimationMap.NewFrameEvents -= NewFrameEvent;
            }

            if (AnimationControlMode().Contains(AnimationControlModeElement.LinkToOnDestroy))
            {
                FAnimationMap.DestroyEvents -= DestroyEvent;
            }
        }
    }

    // Animation.pas:218
    public override void MakeVisible(VisibilityList VisibilityList)
    {
        if (!FState.Contains(StateElement.Visible))
        {
            FState = FState + Set<StateElement>.Of(StateElement.Visible);
            VisibilityList.AddSortChild(this, GetShowPriority());
        }
    }

    // Animation.pas:238
    public override void Select()
    {
        base.Select();
        FState = FState + Set<StateElement>.Of(StateElement.Selected);
    }

    // Animation.pas:244
    public override void Unselect()
    {
        base.Unselect();
        FState = FState - Set<StateElement>.Of(StateElement.Selected);
    }

    // Animation.pas:286
    public virtual bool PlaceTE(AnimationTE Event, TokenExecuter Executer) => PlaceHX(FAnimationMap.MapContainer, FXhx, FYhx, FLhx);

    // Animation.pas:291
    public virtual bool RemoveTE(AnimationTE Event, TokenExecuter Executer)
    {
        MainRemove();
        return true;
    }

    // Animation.pas:297
    public virtual bool ExecuteTE(AnimationTE Event, TokenExecuter Executer) => false;

    // Animation.pas:308
    public virtual bool CanPlace(MapContainer Map, sbyte X, sbyte Y, sbyte L)
    {
        bool Result;
        if (X < 0 || Y < 0 || L < 0 || X >= Map.Width || Y >= Map.Height || L >= Map.LevelCount)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Animation.pas:357
    public virtual bool PlaceHX(MapContainer Map, sbyte X, sbyte Y, sbyte L)
    {
        bool Result;
        if (GetEditMode() || CanPlace(Map, X, Y, L))
        {
            FAnimationMap = Map.Map;
            Remove();
            FXhx = X;
            FYhx = Y;
            FLhx = L;
            Connect();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Animation.pas:302
    public virtual void NewFrame()
    {
        FFrameCounter += 1;
        if (FOnNewFrame != null)
        {
            FOnNewFrame(this, FFrameCounter);
        }
    }

    // Animation.pas:138
    public void DestroyEvent(object Sender)
    {
        if (Owner == null)
        {
            Dispose();
        }
    }

    // Animation.pas:148
    public void NewFrameEvent(object Sender)
    {
        NewFrame();
    }

    // Animation.pas:255
    public void SetDimensions(Rect R)
    {
        if (!FState.Contains(StateElement.Placed))
        {
            FDimensions = R;
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
        Visible = 0, EditMode = 1, Selected = 2, LinkedToTE = 3, Activated = 4, Placed = 5
    }

    public delegate void NewFrameAnimationEvent(Animation Animation, int FrameCounter);

}
