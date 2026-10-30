// Translated from AoWLenLine.pas.
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
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;

namespace Aow.Units.AoWLenLine
{
    // vertical line image library
    public partial class TAoWLenLine : Aow.Units.AoWControl.TAOWControl
    {
        public TAoWLenLine(TComponent owner) : base(owner)
        { }

        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILLine;
        // AoWLenLine.pas:36
        public override void Validate()
        {
            if (FILLine != null)
            {
                FValid = true;
            }

            // { Original bug: Validate calls SetSize. }
            base.SetSize();
        }

        // AoWLenLine.pas:42
        public override void SetSize()
        {
            var call__1 = FILLine[0];
            FWinWidth = call__1.Width;
            base.SetSize();
        }

        // AoWLenLine.pas:56
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            try
            {
                if ((FValid == true) && (FVisible == true))
                {
                    var call__2 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                    FILLine.DrawILLv(0, call__2, ref SurfDesc);
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

        // AoWLenLine.pas:70
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWLenLine.pas:79
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWLenLine.pas:87
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }
        }

        // AoWLenLine.pas:95
        public override void CheckKeyPress(ref byte Key)
        {
            FHandled = false;
        }

        // AoWLenLine.pas:48
        public virtual void SetILLine(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != null)
            {
                if (Value.Count == 3)
                {
                    FILLine = Value;
                }
                else
                {
                    Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Lines must have 3 Images", ((AnsiString)"Error - ") + Name, (int)0);
                }
            }
            else
            {
                FILLine = null;
            }
        }

        [Published]
        public new int WinHeight { get => FWinHeight; set => SetHeight(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILLine { get => FILLine; set => SetILLine(value); }
    }

    public static partial class AoWLenLine
    {
        // AoWLenLine.pas:100
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Tools", [typeof(Aow.Units.AoWLenLine.TAoWLenLine)]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
