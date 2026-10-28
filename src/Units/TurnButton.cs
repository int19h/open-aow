// Translated from TurnButton.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWControl;
using static Aow.Units.AoWControl.AoWControl;

namespace Aow.Units.TurnButton
{
    // { ot=1 }
    // { TAOWTurnButton: instance size $144; $114 library, $118 current player, $11C last player,
//     $120 animating, $124 animation phase, $128 frame offset, $12C done event,
//     $134 change event, $13C checked, $140 animation frame. }
    public partial class TAOWTurnButton : Aow.Units.AoWControl.TAOWControl
    {
        public Aow.Units.ImageLib.TImageLibrary FImageLib;
        // { $114 }
        public int FCurPlayer;
        // { $118 }
        public int FLastPlayer;
        // { $11C }
        public bool FAnimating;
        // { $120 }
        public int FAnimationPhase;
        // { $124 }
        public int FFrameOffset;
        // { $128 }
        public Aow.Rtl.TNotifyEvent FOnAnimDone;
        // { $12C }
        public Aow.Rtl.TNotifyEvent FOnPlayerChange;
        // { $134 }
        public bool FButtonChecked;
        // { $13C }
        public int FAnimationFrame;
        // TurnButton.pas:54
        // { $140 }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            FFrameOffset = 11;
        }

        // TurnButton.pas:60
        public override void Validate()
        {
            if (FImageLib != null)
            {
                FValid = true;
            }

            base.Validate();
        }

        // TurnButton.pas:66
        public override void SetSize()
        {
            var call__1 = FImageLib[0];
            FWinWidth = call__1.Width;
            var call__2 = FImageLib[0];
            FWinHeight = call__2.Height;
            base.SetSize();
        }

        // TurnButton.pas:79
        public override void Update()
        {
            base.Update();
            if (FVisible)
            {
                switch (FAnimationPhase)
                {
                    case 0:
                        if (FLastPlayer != FCurPlayer)
                        {
                            FAnimationFrame = FLastPlayer * 7;
                            FFrameOffset = 11;
                            FAnimationPhase = 1;
                            FAnimating = true;
                            FDrawn = false;
                        }

                        break;
                    case 1:
                        FAnimationFrame += 1;
                        FDrawn = false;
                        if (((FLastPlayer * 7) + 5) < FAnimationFrame)
                        {
                            FAnimationFrame = 0;
                            FAnimationPhase = 2;
                            FFrameOffset = 0;
                        }

                        break;
                    case 2:
                        FAnimationFrame += 1;
                        FDrawn = false;
                        if (FAnimationFrame > 10)
                        {
                            FAnimationPhase = 3;
                            FAnimationFrame = (FCurPlayer * 7) + 5;
                            FFrameOffset = 11;
                        }

                        break;
                    case 3:
                        FAnimationFrame -= 1;
                        FDrawn = false;
                        if ((FCurPlayer * 7) > FAnimationFrame)
                        {
                            FAnimationFrame = FCurPlayer * 7;
                            FAnimationPhase = 0;
                            FFrameOffset = 11;
                            FLastPlayer = FCurPlayer;
                            FAnimating = false;
                            if (FOnAnimDone != null)
                            {
                                FOnAnimDone(this);
                            }
                        }

                        break;
                }
            }
        }

        // TurnButton.pas:132
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            try
            {
                if ((FVisible == true) && (FValid == true))
                {
                    if (FOnPreDraw != null)
                    {
                        FOnPreDraw(this, SurfDesc);
                    }

                    if (!FAnimating && (FButtonChecked == true))
                    {
                        var call__3 = FImageLib[(FFrameOffset + (FCurPlayer * 7)) + 6];
                        call__3.ShowClipped(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, SurfDesc);
                    }
                    else
                    {
                        var call__4 = FImageLib[FFrameOffset + FAnimationFrame];
                        call__4.ShowClipped(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, SurfDesc);
                    }

                    if (FOnDraw != null)
                    {
                        FOnDraw(this, SurfDesc);
                    }
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Draw", ((AnsiString)"Error - ") + Name, (int)0);
            }
        }

        // TurnButton.pas:151
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if ((Button == Aow.Rtl.TMouseButton.mbLeft) && !FAnimating)
                {
                    if (FButtonChecked)
                    {
                        FButtonChecked = false;
                    }
                    else
                    {
                        FButtonChecked = true;
                    }

                    FDrawn = false;
                    if (FOnPlayerChange != null)
                    {
                        FOnPlayerChange(this);
                    }

                    if (FOnMouseDown != null)
                    {
                        FOnMouseDown(this, Button, Shift, X, Y);
                    }
                }
            }
        }

        // TurnButton.pas:169
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // TurnButton.pas:180
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            if ((((X >= (FAOWWindow.FWinLeft + FWinLeft)) && (X <= (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - 1))) && (Y >= (FAOWWindow.FWinTop + FWinTop))) && (Y <= (((FAOWWindow.FWinTop + FWinTop) + FWinHeight) - 1)))
            {
                FHandled = true;
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }
        }

        // TurnButton.pas:73
        public virtual void SetILB(Aow.Units.ImageLib.TImageLibrary Value)
        {
            if (Value != null)
            {
                FImageLib = Value;
            }
            else
            {
                FImageLib = null;
            }
        }

        [Published]
        public Aow.Units.ImageLib.TImageLibrary ImageLib { get => FImageLib; set => SetILB(value); }

        [Published]
        public int CurPlayer { get => FCurPlayer; set => FCurPlayer = value; }

        [Published]
        public int LastPlayer { get => FLastPlayer; set => FLastPlayer = value; }

        [Published]
        public bool Animating { get => FAnimating; set => FAnimating = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnAnimDone { get => FOnAnimDone; set => FOnAnimDone = value; }

        [Published]
        public new Aow.Rtl.TNotifyEvent OnChange { get => FOnPlayerChange; set => FOnPlayerChange = value; }

        [Published]
        public bool Checked { get => FButtonChecked; set => FButtonChecked = value; }

        // TurnButton.pas:31
        public static new Aow.Units.TurnButton.TAOWTurnButton Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.TurnButton.TAOWTurnButton o = new Aow.Units.TurnButton.TAOWTurnButton();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public enum TButtonType : byte
    {
        btText = 0,
        btIcon = 1
    }

    public static partial class TurnButton
    {
        // TurnButton.pas:192
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Tools", [typeof(Aow.Units.TurnButton.TAOWTurnButton)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
