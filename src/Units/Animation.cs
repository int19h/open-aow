// Translated from Animation.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Animation
{
    public enum TAnimationControlModeElement : byte
    {
        amLinkToNewFrame = 0,
        amFixedVisible = 1,
        amSelectable = 2,
        amSaveable = 3,
        amOwnedByMap = 4,
        amLinkToOnDestroy = 5
    }

    public enum TAnimationStateElement : byte
    {
        asVisible = 0,
        asEditMode = 1,
        asSelected = 2,
        asLinkedToTE = 3,
        asActivated = 4,
        asPlaced = 5
    }

    public enum TTwinkleModeElement : byte
    {
        tmRotate = 0,
        tmExpand = 1,
        tmExplode = 2,
        tmImplode = 3
    }

    // { ot=1 }
    // hex position, display level, state, frame, dimensions, callbacks, tag, map
    public partial class TAnimation : THexagonSprite
    {
        public TAnimation(TECustomNode owner) : base(owner)
        {
            FAnimationLevel = 26;
        }

        // Animation.pas:143
        public new const int ClassID = 131815;

        public sbyte FXhx;
        public sbyte FYhx;
        public sbyte FLhx;
        public byte FAnimationLevel;
        public TSet<TAnimationStateElement> FState;
        public int FFrameCounter;
        public TRect FDimensions;
        public TShowAnimationEvent FOnShow;
        public TNewFrameAnimationEvent FOnNewFrame;
        public int FTag;
        public THSMap FAnimationMap;
        // Animation.pas:133
        public override void Destroy()
        {
            base.Destroy();
        }

        // Animation.pas:153
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            if (AnimationControlMode().Contains(TAnimationControlModeElement.amSaveable))
            {
                Result = TSet<TEControlStyleElement>.Of(TEControlStyleElement.csSaveLoad);
            }
            else
            {
                Result = TSet<TEControlStyleElement>.Of();
            }

            return Result;
        }

        // Animation.pas:276
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Bytes.Of(ref FDimensions, 16), 16, 10);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 11);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 12);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 13);
            Stream.rwInteger(ref FFrameCounter, 14);
        }

        // Animation.pas:127


        // Animation.pas:194
        public override byte GetLevel()
        {
            byte Result = default;
            Result = FAnimationLevel;
            return Result;
        }

        // Animation.pas:207
        public override void SetVisible(bool Value)
        {
            if (Value)
            {
                FState = FState + TSet<TAnimationStateElement>.Of(TAnimationStateElement.asVisible);
            }
            else
            {
                FState = FState - TSet<TAnimationStateElement>.Of(TAnimationStateElement.asVisible);
            }
        }

        // Animation.pas:213
        public override bool GetVisible()
        {
            bool Result = default;
            Result = FState.Contains(TAnimationStateElement.asVisible);
            return Result;
        }

        // Animation.pas:250
        public override bool GetSelected()
        {
            bool Result = default;
            Result = FState.Contains(TAnimationStateElement.asSelected);
            return Result;
        }

        // Animation.pas:261
        public override sbyte GetXhx()
        {
            sbyte Result = default;
            Result = FXhx;
            return Result;
        }

        // Animation.pas:266
        public override sbyte GetYhx()
        {
            sbyte Result = default;
            Result = FYhx;
            return Result;
        }

        // Animation.pas:271
        public override sbyte GetLhx()
        {
            sbyte Result = default;
            Result = FLhx;
            return Result;
        }

        // Animation.pas:227
        public override bool GetEditMode()
        {
            bool Result = default;
            Result = FState.Contains(TAnimationStateElement.asEditMode);
            return Result;
        }

        // Animation.pas:232
        public override void SetEditMode(bool Value)
        {
            if (Value)
            {
                FState = FState + TSet<TAnimationStateElement>.Of(TAnimationStateElement.asEditMode);
            }
            else
            {
                FState = FState - TSet<TAnimationStateElement>.Of(TAnimationStateElement.asEditMode);
            }
        }

        // Animation.pas:199
        public override int GetShowPriority()
        {
            int Result = default;
            int X = default;
            int Y = default;
            HXtoHP(FXhx, FYhx, ref X, ref Y);
            Result = (FAnimationLevel << 26) + ((Y + FDimensions.Bottom) << 13) + X;
            return Result;
        }

        // Animation.pas:315
        public override void Connect()
        {
            int X = default;
            int Y = default;
            if (!FState.Contains(TAnimationStateElement.asPlaced))
            {
                HXtoHP(FXhx, FYhx, ref X, ref Y);
                if (AnimationControlMode().Contains(TAnimationControlModeElement.amOwnedByMap))
                {
                    PlaceOnMapEx(FAnimationMap.MapContainer, X + FDimensions.Left, Y + FDimensions.Top, FLhx, X + FDimensions.Right, Y + FDimensions.Bottom);
                }
                else
                {
                    PlaceUserOnMapEx(FAnimationMap.MapContainer, X + FDimensions.Left, Y + FDimensions.Top, FLhx, X + FDimensions.Right, Y + FDimensions.Bottom);
                }

                FState = FState + TSet<TAnimationStateElement>.Of(TAnimationStateElement.asPlaced);
            }
        }

        // Animation.pas:332
        public override void Disconnect()
        {
            int X = default;
            int Y = default;
            if (FState.Contains(TAnimationStateElement.asPlaced))
            {
                HXtoHP(FXhx, FYhx, ref X, ref Y);
                if (AnimationControlMode().Contains(TAnimationControlModeElement.amOwnedByMap))
                {
                    RemoveFromMapEx(X + FDimensions.Left, Y + FDimensions.Top, X + FDimensions.Right, Y + FDimensions.Bottom);
                }
                else
                {
                    RemoveUserFromMapEx(FAnimationMap.MapContainer, X + FDimensions.Left, Y + FDimensions.Top, FLhx, X + FDimensions.Right, Y + FDimensions.Bottom);
                }

                FState = FState - TSet<TAnimationStateElement>.Of(TAnimationStateElement.asPlaced);
            }
        }

        // Animation.pas:383
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            if (FOnShow != null)
            {
                FOnShow(this, XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // Animation.pas:349
        public override bool Place(TMapContainer Map, int Xhp, int Yhp, int Lhx)
        {
            bool Result = default;
            int X = default;
            int Y = default;
            HPtoHX(Xhp, Yhp, ref X, ref Y);
            Result = PlaceHX(Map, (sbyte)X, (sbyte)Y, (sbyte)Lhx);
            return Result;
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
            FState = FState + TSet<TAnimationStateElement>.Of(TAnimationStateElement.asActivated);
            if (AnimationControlMode().Contains(TAnimationControlModeElement.amLinkToNewFrame))
            {
                FAnimationMap.NewFrameEvents.Add(NewFrameEvent);
            }

            if (AnimationControlMode().Contains(TAnimationControlModeElement.amFixedVisible))
            {
                FAnimationMap.DynamicFixedVisibleHS.AddChild(this);
            }

            if (AnimationControlMode().Contains(TAnimationControlModeElement.amLinkToOnDestroy))
            {
                FAnimationMap.DestroyEvents.Add(DestroyEvent);
            }
        }

        // Animation.pas:176
        public override void Deactivate()
        {
            base.Deactivate();
            if (FState.Contains(TAnimationStateElement.asActivated))
            {
                FState = FState - TSet<TAnimationStateElement>.Of(TAnimationStateElement.asActivated);
                if (AnimationControlMode().Contains(TAnimationControlModeElement.amFixedVisible))
                {
                    FAnimationMap.DynamicFixedVisibleHS.RemoveChild(this);
                    FAnimationMap.DynamicVisibleHS.RemoveChild(this);
                }

                if (AnimationControlMode().Contains(TAnimationControlModeElement.amLinkToNewFrame))
                {
                    FAnimationMap.NewFrameEvents.Remove(NewFrameEvent);
                }

                if (AnimationControlMode().Contains(TAnimationControlModeElement.amLinkToOnDestroy))
                {
                    FAnimationMap.DestroyEvents.Remove(DestroyEvent);
                }
            }
        }

        // Animation.pas:218
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if (!FState.Contains(TAnimationStateElement.asVisible))
            {
                FState = FState + TSet<TAnimationStateElement>.Of(TAnimationStateElement.asVisible);
                var call__1 = GetShowPriority();
                VisibilityList.AddSortChild(this, call__1);
            }
        }

        // Animation.pas:238
        public override void Select()
        {
            base.Select();
            FState = FState + TSet<TAnimationStateElement>.Of(TAnimationStateElement.asSelected);
        }

        // Animation.pas:244
        public override void Unselect()
        {
            base.Unselect();
            FState = FState - TSet<TAnimationStateElement>.Of(TAnimationStateElement.asSelected);
        }

        // Animation.pas:286
        public virtual bool PlaceTE(TAnimationTE Event, TTokenExecuter Executer)
        {
            bool Result = default;
            var call__2 = PlaceHX(FAnimationMap.MapContainer, FXhx, FYhx, FLhx);
            Result = call__2;
            return Result;
        }

        // Animation.pas:291
        public virtual bool RemoveTE(TAnimationTE Event, TTokenExecuter Executer)
        {
            bool Result = default;
            MainRemove();
            Result = true;
            return Result;
        }

        // Animation.pas:297
        public virtual bool ExecuteTE(TAnimationTE Event, TTokenExecuter Executer)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // Animation.pas:308
        public virtual bool CanPlace(TMapContainer Map, sbyte X, sbyte Y, sbyte L)
        {
            bool Result = default;
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
        public virtual bool PlaceHX(TMapContainer Map, sbyte X, sbyte Y, sbyte L)
        {
            bool Result = default;
            var call__3 = GetEditMode();
            bool bool__4 = call__3;
            if (!bool__4)
            {
                var call__5 = CanPlace(Map, X, Y, L);
                bool__4 = call__5;
            }

            if (bool__4)
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
        public void DestroyEvent(TObject Sender)
        {
            if (Owner == null)
            {
                Destroy();
            }
        }

        // Animation.pas:148
        public void NewFrameEvent(TObject Sender)
        {
            NewFrame();
        }

        // Animation.pas:255
        public void SetDimensions(TRect R)
        {
            if (!FState.Contains(TAnimationStateElement.asPlaced))
            {
                FDimensions = R;
            }
            else
            {
                throw Exception.Create((AnsiString)"Cannot change dimensions when object is places");
            }
        }

        // Animation.pas:43


        // Animation.pas:159
        public virtual TSet<TAnimationControlModeElement> AnimationControlMode()
        {
            TSet<TAnimationControlModeElement> Result = default;
            Result = TSet<TAnimationControlModeElement>.Of(TAnimationControlModeElement.amLinkToNewFrame, TAnimationControlModeElement.amFixedVisible, TAnimationControlModeElement.amLinkToOnDestroy);
            return Result;
        }


    }

    // state, animation
    public partial class TAnimationTE : TTokenEvent
    {
        public TAnimationTE(TECustomNode owner) : base(owner)
        {}

        // Animation.pas:388
        public new const int ClassID = 135433;

        public TAnimationTEState FState;
        public TAnimation FAnimation;
        // Animation.pas:399
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwCreateEObject(ref FAnimation, null, 15);
        }

        // Animation.pas:405
        public override TExecuteResult Execute(TTokenExecuter TE)
        {
            TExecuteResult Result = default;
            Result = base.Execute(TE);
            if (FAnimation != null)
            {
                if (FState == TAnimationTEState.atIdle)
                {
                    var call__6 = FAnimation.PlaceTE(this, TE);
                    if (call__6)
                    {
                        FState = TAnimationTEState.atPlaying;
                    }
                    else
                    {
                        Result = TExecuteResult.erDoneHalt;
                    }
                }

                if (FState == TAnimationTEState.atPlaying)
                {
                    var call__7 = FAnimation.ExecuteTE(this, TE);
                    if (!call__7)
                    {
                        FAnimation.RemoveTE(this, TE);
                        FState = TAnimationTEState.atDone;
                    }
                }

                if (FState == TAnimationTEState.atDone)
                {
                    Result = TExecuteResult.erDone;
                }
            }
            else
            {
                Result = TExecuteResult.erDone;
            }

            return Result;
        }

        // Animation.pas:393
        public void Initialize(TAnimation Animation)
        {
            if (FAnimation != null)
            {
                FAnimation.Free();
            }

            FAnimation = Animation;
        }

    }

    // sequence, pixel offsets, last frame, loop flag
    public partial class TImageSequenceAnimation : TAnimation
    {
        public TImageSequenceAnimation(TECustomNode owner) : base(owner)
        {
            FFrameCount = -1;
            FXhpOffset = 0;
            FYhpOffset = 0;
        }

        public TImageSequence FImageSequence;
        public int FXhpOffset;
        public int FYhpOffset;
        public int FFrameCount;
        public bool FLooped;
        // Animation.pas:424


        // Animation.pas:493
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TImageSequence Sequence = default;
            int X = default;
            int Y = default;
            int Index = default;
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            var call__8 = GetLhx();
            if (call__8 == FAnimationMap.ViewL)
            {
                HXtoHP(FXhx, FYhx, ref X, ref Y);
                XhpSurf = X - XhpSurf + 16 + FXhpOffset;
                YhpSurf = Y - YhpSurf + 16 + FYhpOffset;
                if (!FLooped)
                {
                    Sequence = FImageSequence;
                    while (Sequence != null)
                    {
                        var call__9 = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                        Index = call__9;
                        if (Index != -1)
                        {
                            var call__10 = Sequence.ImageLibrary.ImageExist(Index);
                            if (call__10)
                            {
                                var call__11 = Sequence.ImageLibrary[Index];
                                call__11.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                            }
                        }

                        Sequence = Sequence.Next;
                    }
                }
                else
                {
                    Sequence = FImageSequence;
                    while (Sequence != null)
                    {
                        var call__12 = Sequence.GetFrameIndex(0, FFrameCounter);
                        Index = call__12;
                        if (Index != -1)
                        {
                            var call__13 = Sequence.ImageLibrary[Index];
                            call__13.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }
                }
            }
        }

        // Animation.pas:487
        public override void NewFrame()
        {
            base.NewFrame();
            if (FFrameCounter == FFrameCount)
            {
                Destroy();
            }
        }

        // Animation.pas:432
        public void UpdateDimensions()
        {
            TImageSequence Sequence = default;
            TRect R = default;
            FDimensions.Left = 0;
            FDimensions.Right = 0;
            FDimensions.Top = 0;
            FDimensions.Bottom = 0;
            Sequence = FImageSequence;
            while (Sequence != null)
            {
                Sequence.GetDimensions(ref R);
                UnionRect(ref FDimensions, FDimensions, R);
                Sequence = Sequence.Next;
            }

            FDimensions.Left += 16;
            FDimensions.Right += 16;
            FDimensions.Top += 16;
            FDimensions.Bottom += 16;
        }

        // Animation.pas:454
        public void SetImageSequence(TImageSequence Sequence)
        {
            TImageSequence Current = default;
            int Frames = default;
            FImageSequence = Sequence;
            UpdateDimensions();
            if (!FLooped)
            {
                Current = FImageSequence;
                FFrameCount = -1;
                while (Current != null)
                {
                    Frames = Current.GetTotalSequenceFrames();
                    if (Frames > FFrameCount)
                    {
                        FFrameCount = Frames;
                    }

                    Current = Current.Next;
                }

                if (FFrameCount != -1)
                {
                    FFrameCount += 1;
                }
            }
        }

        // Animation.pas:475
        public void SetXhpOffset(int Value)
        {
            FXhpOffset = Value;
            UpdateDimensions();
        }

        // Animation.pas:481
        public void SetYhpOffset(int Value)
        {
            FYhpOffset = Value;
            UpdateDimensions();
        }

        // Animation.pas:98

    }

    // sequence, particle count, radius, lifetime, random start delay, mode
    public partial class TTwinkleAnimation : TAnimation
    {
        public TTwinkleAnimation(TECustomNode owner) : base(owner)
        {
            FAnimationLevel = 28;
        }

        public TImageSequence FImageSequence;
        public int FCount;
        public int FRadius;
        public int FDuration;
        public int FStartDelay;
        public TSet<TTwinkleModeElement> FMode;
        // Animation.pas:532


        // Animation.pas:538
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TImageSequence Sequence = default;
            int OldSeed = default;
            int Delay = default;
            int X = default;
            int Y = default;
            int DX = default;
            int DY = default;
            int I = default;
            int Radius = default;
            int Angle = default;
            int Index = default;
            OldSeed = RandSeed;
            HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf + 16;
            YhpSurf = Y - YhpSurf + 16;
            var call__14 = GetXhx();
            var call__15 = GetYhx();
            RandSeed = call__14 + AoWHSMap.FInitialRandomSeed + call__15 * 3;
            {
                I = 0;
                int end__16 = FCount - 1;
                for (; I <= end__16; I += 1)
                {
                    var call__17 = Random(FRadius);
                    Radius = call__17;
                    Angle = Random(360);
                    var call__18 = Random(FStartDelay);
                    Delay = call__18;
                    if (FFrameCounter - Delay >= 0)
                    {
                        if (FMode.Contains(TTwinkleModeElement.tmExpand))
                        {
                            Radius = Round((double)(Radius * FFrameCounter) / FDuration) + Radius / 2;
                        }

                        if (FMode.Contains(TTwinkleModeElement.tmExplode))
                        {
                            Radius += Round((FFrameCounter - Delay) * (FFrameCounter - Delay));
                        }

                        if (FMode.Contains(TTwinkleModeElement.tmImplode))
                        {
                            Radius += Round(FDuration - (FFrameCounter - Delay));
                        }

                        if (FMode.Contains(TTwinkleModeElement.tmRotate))
                        {
                            Angle += FFrameCounter * 4;
                        }

                        DegreesRadToCo(Angle, Radius, ref DX, ref DY);
                        Sequence = FImageSequence;
                        while (Sequence != null)
                        {
                            var call__19 = Sequence.GetUnloopedFrameIndex(FFrameCounter - Delay);
                            Index = call__19;
                            var call__20 = Sequence.ImageLibrary.ImageExist(Index);
                            if (call__20)
                            {
                                var call__21 = Sequence.ImageLibrary[Index];
                                call__21.Show(XhpSurf + Sequence.dXshift + DX, YhpSurf + Sequence.dYshift + DY, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }
                    }
                }
            }

            RandSeed = OldSeed;
            if (FFrameCounter >= FDuration)
            {
                Destroy();
            }
        }

        // Animation.pas:575
        public void Setup(TImageSequence Sequence, TSet<TTwinkleModeElement> Mode, int Count, int Radius, int Duration, int StartDelay)
        {
            FImageSequence = Sequence;
            FCount = Count;
            FRadius = Radius;
            FMode = Mode;
            FStartDelay = StartDelay;
            FDuration = Duration;
        }

        // Animation.pas:117

    }

    public delegate void TShowAnimationEvent(TAnimation Animation, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc);
    public delegate void TNewFrameAnimationEvent(TAnimation Animation, int FrameCounter);
    public enum TAnimationTEState : byte
    {
        atIdle = 0,
        atPlaying = 1,
        atDone = 2
    }

    public static partial class Animation
    {
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
